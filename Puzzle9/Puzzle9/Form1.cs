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
        private Label[] numbers = new Label[9];
        public Form1()
        {
            InitializeComponent();
            initialize();


        }

        public void initialize()
        {
            //arreglo que almaccecna los label de cada ficha
            numbers[0] = Piece1;
            numbers[1] = Piece2;
            numbers[2] = Piece3;
            numbers[3] = Piece4;
            numbers[4] = Piece5;
            numbers[5] = Piece6;
            numbers[6] = Piece7;
            numbers[7] = Piece8;
            numbers[8] = PieceVoid;
        }

        private Point MouseDownLocation; //posicion en la que se hace click al seleccionar una ficha
        private Label selected=null;   //selecciona el label del objeto clickeado
        private Point aux1, aux2;
        private Label auxLabel;
        private bool made;  
        private int position;  //auxiliar para indicar la posicion en el arreglo del numero seleccionado
        
        private void ObjClicked(object sender, MouseEventArgs e)  
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
                selected = sender as Label;
                aux1 = selected.Location;  //guarda la posicion de la fucha antes de ser movida
                selected.BringToFront();
                for(int i = 0; i < 9; i++) //encuentra la posicion en el arreglo de la ficha
                {
                    if (aux1 == numbers[i].Location)
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

        private void ObjChangePosition(object sender, MouseEventArgs e)
        {
            made=false;
            if (selected != null)
            {
                for (int i = 0; i < 9; i++)
                {
                    /* condicional que verifica que cuando el mouse sea levantado, 
                     * sea dentro de los limites de alguna de las fichas diferentes a la seleccionada*/
                    if ((selected.Left + e.X) > numbers[i].Left && (selected.Left + e.X) < (numbers[i].Left + numbers[i].Width) && (selected.Top + e.Y) > numbers[i].Top && (selected.Top + e.Y) < (numbers[i].Top + numbers[i].Height) && made==false && i!=position)
                    {
                        //se actualizan las posiciones de los labels visualmente y en el arreglo
                        aux2 = numbers[i].Location;
                        selected.Location = aux2;
                        numbers[i].Location = aux1;
                        auxLabel = numbers[i];
                        numbers[i] = numbers[position];
                        numbers[position] = auxLabel;
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
