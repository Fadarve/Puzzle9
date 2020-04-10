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
        private static Point[] locations = new Point[9];
        public Form1()
        {
            InitializeComponent();
            initialize();


        }

        public void initialize()
        {
            numbers[0] = Piece1;
            numbers[1] = Piece2;
            numbers[2] = Piece3;
            numbers[3] = Piece4;
            numbers[4] = Piece5;
            numbers[5] = Piece6;
            numbers[6] = Piece7;
            numbers[7] = Piece8;
            numbers[8] = PieceVoid;

            for(int i = 0; i < 9; i++)
            {
                locations[i] = numbers[i].Location;
            }
        }

        private Point MouseDownLocation;
        private Label selected=null;
        private Point aux1, aux2;
        private Label auxLabel;
        private bool made;
        private int position;
        
        private void ObjClicked(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
                selected = sender as Label;
                aux1 = selected.Location;
                selected.BringToFront();
                for(int i = 0; i < 9; i++)
                {
                    if (aux1 == numbers[i].Location)
                    {
                        position = i;
                    }
                }
               
            }
        }

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
                    if ((selected.Left + e.X) > numbers[i].Left && (selected.Left + e.X) < (numbers[i].Left + numbers[i].Width) && (selected.Top + e.Y) > numbers[i].Top && (selected.Top + e.Y) < (numbers[i].Top + numbers[i].Height) && made==false && i!=position)
                    {
                        aux2 = locations[i];
                        selected.Location = aux2;
                        numbers[i].Location = aux1;
                        auxLabel = numbers[i];
                        numbers[i] = numbers[position];
                        numbers[position] = auxLabel;
                        made = true;
                    }
                }
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
