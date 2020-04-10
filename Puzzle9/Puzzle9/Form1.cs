using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace puzzle99
{
    
    public partial class Form1 : Form
    {
        private Label[] numbers1 = new Label[81];  //arreglo que almacena todos los labels en orden
        List<Label> SelectNumbers = new List<Label>();  //lista que almacena los label que se mostraran
        private int n;
        public Form1()
        {
            InitializeComponent();
            initialize();
        }
        public void initialize()
        {
            //arreglo que almaccecna los label de cada ficha


            CmbTamanyos.Items.Add("3x3");
            CmbTamanyos.Items.Add("4x4");
            CmbTamanyos.Items.Add("5x5");
            CmbTamanyos.Items.Add("6x6");
            CmbTamanyos.Items.Add("7x7");
            CmbTamanyos.Items.Add("8x8");
            CmbTamanyos.Items.Add("9x9");
            n = 3;  //se fija inicialmente el tamano de n en 3
            CreateLabels();
            CmbTamanyos.SelectedIndex = 0;
           
        }

        private Point MouseDownLocation; //posicion en la que se hace click al seleccionar una ficha
        private Label selected = null;   //selecciona el label del objeto clickeado
        private Point aux1, aux2;
        private Label auxLabel;
        private bool made;
        private int position;  //auxiliar para indicar la posicion en el arreglo del numero seleccionado


        //funcion que crea autmaticamente todos los 81 labels necesarios para el tamano maxmo del puzzle
        public void CreateLabels() 
        {
            for(int i=0; i < 81; i++)
            {
                Label label = new Label();
                label.Name = "Piece" + (i+1).ToString();
                label.AutoSize = false;
                label.BackColor = System.Drawing.SystemColors.AppWorkspace;
                label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                label.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                label.Location = new System.Drawing.Point(240, 40);
                label.Size = new System.Drawing.Size(50, 50);
                label.TabIndex = 0;
                label.Text = (i+1).ToString();
                label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                label.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ObjClicked);
                label.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ObjMove);
                label.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ObjChangePosition);
                numbers1[i] = label;
                numbers1[i].Visible = false;
                Controls.Add(numbers1[i]);
            }

            numbers1[80].Text = " ";
            numbers1[80].BackColor = System.Drawing.SystemColors.ControlDarkDark;
        }

        //funcion que reinicia la lista, le asigna los numeros dependiendo de n y los dibuja en pantalla
        public void DrawPieces()
        {
            for (int i=0;i<SelectNumbers.Count;i++)
            {
                SelectNumbers[i].Visible = false;
            }

            SelectNumbers.Clear();

            for(int i = 0;i<n*n;i++)
            {
                if (i!=(n*n)-1)
                {
                    SelectNumbers.Add(numbers1[i]);
                }
                else {
                    SelectNumbers.Add(numbers1[80]);
                }
            }

            int county = 0, posX=40,posY=40;

            for(int i = 0; i < n*n; i++)
            {
                SelectNumbers[i].Location = new System.Drawing.Point(posX, posY);
                SelectNumbers[i].Visible = true;
                posX += 60;
                county++;
                if (county == n)
                {
                    posX = 40;
                    posY += 60;
                    county = 0;
                }

            }
        }
        
        private void ObjClicked(object sender, MouseEventArgs e)  
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
                selected = sender as Label;
                aux1 = selected.Location;  //guarda la posicion de la fucha antes de ser movida
                selected.BringToFront();
                for(int i = 0; i < n*n; i++) //encuentra la posicion en el arreglo de la ficha
                {
                    if (aux1 == SelectNumbers[i].Location)
                    {
                        position = i;
                    }
                }
               
            }
        }

        //funcion que actualiza la posicion de la ficha mientras el mouse se mueve
        private void ObjMove(object sender, MouseEventArgs e)  
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                selected.Left = e.X + selected.Left - MouseDownLocation.X;
                selected.Top = e.Y + selected.Top - MouseDownLocation.Y;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //actualiza el valor de n y vuelve a dibujar
        private void CmbTamanyos_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(CmbTamanyos.SelectedIndex)
            {
                case 0:
                    n = 3;
                    break;
                case 1:
                    n = 4;
                    break;
                case 2:
                    n = 5;
                    break;
                case 3:
                    n = 6;
                    break;
                case 4:
                    n = 7;
                    break;
                case 5:
                    n = 8;
                    break;
                case 6:
                    n = 9;
                    break;
            }

            DrawPieces();
        }

        private void ObjChangePosition(object sender, MouseEventArgs e)
        {
            made=false;
            if (selected != null)
            {
                for (int i = 0; i < n*n; i++)
                {
                    /* condicional que verifica que cuando el mouse sea levantado, 
                     * sea dentro de los limites de alguna de las fichas diferentes a la seleccionada*/
                    if ((selected.Left + e.X) > SelectNumbers[i].Left && (selected.Left + e.X) < (SelectNumbers[i].Left + SelectNumbers[i].Width) && (selected.Top + e.Y) > SelectNumbers[i].Top && (selected.Top + e.Y) < (SelectNumbers[i].Top + SelectNumbers[i].Height) && made==false && i!=position)
                    {
                        //se actualizan las posiciones de los labels visualmente y en el arreglo

                        aux2 = SelectNumbers[i].Location;
                        selected.Location = aux2;
                        SelectNumbers[i].Location = aux1;
                        auxLabel = SelectNumbers[i];
                        SelectNumbers[i] = SelectNumbers[position];
                        SelectNumbers[position] = auxLabel;

                        made = true;  //indica si ya se realizo el cambio para que la funcion no vuelva a entrar al if
                    }
                }
                /* si no se encontró concordancia con ninguna de las otras fichas, 
                   devuelve la seleccionada a su posicion inicial*/
                if (made==false)
                {
                    selected.Location = aux1;
                    made = true;
                }
            }
            selected = null;
        } 
    }
}
