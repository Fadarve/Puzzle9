using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace puzzle99
{
    public unsafe struct Estado
    {
        public List<int> Ldatos;
        public int h; //Valor que calcula cuantas fichas estan fuera de lugar 
        public int g; //nivel donde esta el nodo
        public int padre; //Posicion del padre en el arbol (nivel)
        public int vacio;
        public int tamk;
        public int tam;
        public int nmoves; //Cantidad de movimientos posible
        public int move_anterior;
        public List<char> Lmoves;

        public void setvacio(int valor) { this.vacio = valor; }
        public void setmove_anterior(int valor) { this.move_anterior = valor; }
        public void seth(int valor) { this.h = valor; }
        public void setnmoves(int valor) { this.nmoves = valor; }

        public void setLmoves(char m0, char m1, char m2, char m3) { this.Lmoves[0] = m0; this.Lmoves[1] = m1; this.Lmoves[2] = m2; this.Lmoves[3] = m3; }


    }

    public unsafe struct Arbol
    {
        public List<Estado> Lestados;
    }
    public partial class Form1 : Form
    {
        private Label[] numbers1 = new Label[81];  //arreglo que almacena todos los labels en orden
        List<Label> SelectNumbers = new List<Label>();  //lista que almacena los label que se mostraran
        List<Label> ListaSolucion = new List<Label>();

        private int n;

        public Form1()
        {
            InitializeComponent();
            initialize();
        }
        public void initialize()
        {
            //arreglo que almaccecna los label de cada ficha
            CmbTamanyos.Items.Add("3x3     (9 piezas)");
            CmbTamanyos.Items.Add("4x4     (16 piezas)");
            CmbTamanyos.Items.Add("5x5     (25 piezas)");
            CmbTamanyos.Items.Add("6x6     (36 piezas)");
            CmbTamanyos.Items.Add("7x7     (49 piezas)");
            CmbTamanyos.Items.Add("8x8     (64 piezas)");
            CmbTamanyos.Items.Add("9x9     (81 piezas)");
            n = 3;  //se fija inicialmente el tamano de n en 3
            CreateLabels();
            CmbTamanyos.SelectedIndex = 0;
            rBtnEstInicial.Checked = true;
        }

        ///Listas///

        //Rompecabezas
        unsafe void intercambio_fichas(List<int> Lfichas, int ps1, int ps2)
        {
            if (Lfichas.Count != 0)
            {
                int aux = Lfichas[ps1];
                Lfichas[ps1] = Lfichas[ps2];
                Lfichas[ps2] = aux;
            }
            else
            {
                Console.Write("La lista no tiene elementos");
            }


        }

        //Arbol => (2)
        List<Estado> Arbol = new List<Estado>();

        List<Estado> RutaSolucion = new List<Estado>();

        public unsafe int ancho_padre = 0;
        public unsafe int ancho_ultimo = 0; //siempre apunta al ultimo de la lista de estados
        int profundidad = 1;
        int desfasados = -1;
        int nivel = 1;

        unsafe void agregar_estado(List<Estado> lestados, List<int> ldatos, int h, int ps_vacio, int tamk = 0, int tam = 0, int niv = 0, int ps_padre = -1, char r = 'n', char le = 'n', char u = 'n', char d = 'n', int nm = 0, int ma = -1, int opc = 'f')
        {
            List<char> movimientos = new List<char>() { r, le, u, d };
            Estado nuevoEstado = new Estado()
            { Ldatos = ldatos, Lmoves = movimientos, h = h, vacio = ps_vacio, tamk = tamk, tam = tam, g = niv, padre = ps_padre, nmoves = nm, move_anterior = ma };
            if (opc == 's')
            {
                lestados.Insert(1, nuevoEstado);
            }
            else { lestados.Add(nuevoEstado); }

        }

        unsafe int comparar_estados_ham(List<Estado> arbol, Estado estadoActual, List<int> final)
        {

            int i = 0;
            int h = 0;
            while (i < n * n)
            {
                //cout << acceder_lista(c1, i) << " vs " << acceder_lista(final, i) << endl;
                if (estadoActual.Ldatos[i] == final[i]) { h = h; }
                else { h = h + 1; }
                i++;
            }
            //cout << endl;

            return h;

        }

        unsafe void buscar_movimientos(Estado estadoActual)
        {

            //cout << "Nueva busqueda" << endl;
            //cout << "direccion: "<< p <<", posicion: " << p->vacio << ", tamk: " << p->tamk << ", tam: " << p->tam << endl;

            int n2 = n - 1;
            int t = estadoActual.Ldatos.Count - 1;
            int v = estadoActual.vacio;

            //Esquinas superiores
            Estado auxestado = estadoActual;
            if (v == 0)
            {
                //cout << "Tiene dos movimientos (esquinas) sup izq " << endl;

                auxestado.setLmoves('r', 'n', 'n', 'd');
                auxestado.setnmoves(2);
                estadoActual = auxestado;
            }
            else if (v == n2)
            {
                //cout << "Tiene dos movimientos (esquinas) sup der " << endl;
                auxestado.setLmoves('n', 'l', 'n', 'd');
                auxestado.setnmoves(2);
                estadoActual = auxestado;

            }
            //Esquinas inferiores
            else if (v == t - n2)
            {
                //cout << "Tiene dos movimientos (esquinas) inf izq" << endl;
                auxestado.setnmoves(2);
                estadoActual = auxestado;
                auxestado.setLmoves('r', 'n', 'u', 'n');

                estadoActual = auxestado;
            }
            else if (v == t)
            {
                //cout << "Tiene dos movimientos (esquinas) inf der" << endl;
                auxestado.setLmoves('n', 'l', 'u', 'n');
                auxestado.setnmoves(2);
                estadoActual = auxestado;

            }
            //Bordes horizontales
            else if (v > 0 && v < n2)
            {
                //cout << "Tiene tres movimientos (bordes horizontales) sup" << endl;
                auxestado.setLmoves('r', 'l', 'n', 'd');
                auxestado.setnmoves(3);
                estadoActual = auxestado;

            }
            else if (v > t - n2 && v < t)
            {
                //cout << "Tiene tres movimientos (bordes horizontales) inf" << endl;
                auxestado.setLmoves('r', 'l', 'u', 'n');
                auxestado.setnmoves(3);
                estadoActual = auxestado;

            }
            //Bordes verticales
            else if (v % (n2 + 1) == 0)
            {
                //cout << "Tiene tres movimientos (bordes vertical - izq)" << endl;
                auxestado.setLmoves('r', 'n', 'u', 'd');
                auxestado.setnmoves(3);
                estadoActual = auxestado;

            }
            else if ((v + 1) % (n2 + 1) == 0)
            {
                //cout << "Tiene tres movimientos (bordes vertical - der)" << endl;
                auxestado.setLmoves('n', 'l', 'u', 'd');
                auxestado.setnmoves(3);
                estadoActual = auxestado;

            }
            //Demas posiciones
            else
            {
                //cout << "Tiene cuatro movimientos (central) - restringido a tres por el movimiento anterios" << endl;
                auxestado.setLmoves('r', 'l', 'u', 'd');
                auxestado.setnmoves(4);
                estadoActual = auxestado;

            }


            if (estadoActual.move_anterior == 0)
            {

                auxestado.setLmoves('n', auxestado.Lmoves[1], auxestado.Lmoves[2], auxestado.Lmoves[3]);
                auxestado.setnmoves(auxestado.nmoves - 1);
                estadoActual = auxestado;
            }

            else if (estadoActual.move_anterior == 1)
            {
                auxestado.setLmoves(auxestado.Lmoves[0], 'n', auxestado.Lmoves[2], auxestado.Lmoves[3]);
                auxestado.setnmoves(auxestado.nmoves - 1);
                estadoActual = auxestado;
            }
            else if (estadoActual.move_anterior == 2)
            {
                auxestado.setLmoves(auxestado.Lmoves[0], auxestado.Lmoves[1], 'n', auxestado.Lmoves[3]);
                auxestado.setnmoves(auxestado.nmoves - 1);
                estadoActual = auxestado;
            }
            else if (estadoActual.move_anterior == 3)
            {
                auxestado.setLmoves(auxestado.Lmoves[0], auxestado.Lmoves[1], auxestado.Lmoves[2], 'n');
                auxestado.setnmoves(auxestado.nmoves - 1);
                estadoActual = auxestado;
            }


        }
        unsafe void intercambio_2(List<Estado> l, List<int> final)
        {
            int i = 0;

            if (l.Count != 0)
            {

                for (int k = 0; k < 4; k++)
                {

                    if (l[ancho_padre].Lmoves[k] != 'n')
                    {
                        List<int> copia = new List<int>(l[ancho_padre].Ldatos);

                        nivel++;
                        agregar_estado(l, copia, 0, l[ancho_padre].vacio, l[ancho_padre].tamk, l[ancho_padre].tam, nivel, l[ancho_padre].g);

                        ancho_ultimo++;
                        Estado auxestado = l[ancho_ultimo];


                        switch (l[ancho_padre].Lmoves[k])
                        {

                            case 'r':
                                //Derecha


                                intercambio_fichas(l[ancho_ultimo].Ldatos, l[ancho_ultimo].vacio, l[ancho_ultimo].vacio + 1);
                                auxestado.setvacio(l[ancho_ultimo].vacio + 1);
                                auxestado.setmove_anterior(1);
                                l[ancho_ultimo] = auxestado;

                                break;
                            case 'l':
                                //Izquierda
                                intercambio_fichas(l[ancho_ultimo].Ldatos, l[ancho_ultimo].vacio, l[ancho_ultimo].vacio - 1);
                                auxestado.setvacio(l[ancho_ultimo].vacio - 1);
                                auxestado.setmove_anterior(0);
                                l[ancho_ultimo] = auxestado;

                                break;
                            case 'u':
                                //Arriba
                                intercambio_fichas(l[ancho_ultimo].Ldatos, l[ancho_ultimo].vacio, l[ancho_ultimo].vacio - l[ancho_ultimo].tamk);
                                auxestado.setvacio(l[ancho_ultimo].vacio - l[ancho_ultimo].tamk);
                                auxestado.setmove_anterior(3);
                                l[ancho_ultimo] = auxestado;


                                break;
                            case 'd':
                                //Abajo
                                intercambio_fichas(l[ancho_ultimo].Ldatos, l[ancho_ultimo].vacio, l[ancho_ultimo].vacio + l[ancho_ultimo].tamk);
                                auxestado.setvacio(l[ancho_ultimo].vacio + l[ancho_ultimo].tamk);
                                auxestado.setmove_anterior(2);
                                l[ancho_ultimo] = auxestado;

                                break;
                            default:
                                break;

                        }

                        //Define los posibles moviemientos del kernel
                        buscar_movimientos(l[ancho_ultimo]);

                        //Define el costo
                        desfasados = comparar_estados_ham(l, l[ancho_ultimo], final);

                        Estado auxestado2 = l[ancho_ultimo];
                        auxestado2.seth(desfasados);
                        l[ancho_ultimo] = auxestado2;

                        //l[ancho_ultimo].seth(50);
                        //desfasados = l[ancho_ultimo].h;
                        if (desfasados == 0) { break; }
                    }


                }

                //Establece el siguiente nodo (por ancho) como nuevo padre 
                ancho_padre++;

            }
            else
            {
                //cout << "La lista no tiene elementos" << endl;
            }


        }

        unsafe void ruta(List<Estado> arb, List<Estado> sol)
        {

            int j = 0;
            int i = 1;
            int f = ancho_ultimo;
            while (arb[f].padre > -1)
            {
                i = 1;

                //Agrega el nodo padre del resultado a la lista que sigue el camino solucion
                agregar_estado(
                    sol,
                    arb[f].Ldatos,
                    arb[f].h,
                    arb[f].vacio,
                    arb[f].tamk,
                    arb[f].tam,
                    arb[f].g,
                    arb[f].padre,
                    arb[f].Lmoves[0],
                    arb[f].Lmoves[1],
                    arb[f].Lmoves[2],
                    arb[f].Lmoves[3],
                    arb[f].nmoves,
                    arb[f].move_anterior,
                    's'
                );
                f = arb[f].padre - 1;

                j++;

                profundidad++;

            }


        }

        unsafe void iniciarLogica(List<Label> SNumbers)
        {
            ////////////////////////PARAMETROS INICIALES///////////////////////////////
            //Inicializa las posicion inicial y final

            //cout << "\n POSICION INICIAL" << endl << endl;

            //se reinicializan todos los valores globales
            List<int> inicial = new List<int>();
            List<int> final = new List<int>();
            Arbol.Clear();
            RutaSolucion.Clear();
            ancho_padre = 0;
            ancho_ultimo = 0;
            profundidad = 1;
            desfasados = -1;
            nivel = 1;

            //se rellenan las listas conforme la posicion de los label
            for (int i = 0; i < n * n; i++)
            {

                inicial.Add(Convert.ToInt32(SNumbers[i].Name));
                final.Add(Convert.ToInt32(ListaSolucion[i].Name));

            }

            /////////////////////////ARBOL////////////////////////////////


            agregar_estado(Arbol, inicial, -1, inicial.IndexOf(0), n, n * n, nivel);
            buscar_movimientos(Arbol[0]);

            ////////////////////////SOLUCION////////////////////////////////

            agregar_estado(RutaSolucion, inicial, -1, inicial.IndexOf(0), n, n * n, nivel);
            buscar_movimientos(RutaSolucion[0]);

            while (desfasados != 0)
            {

                intercambio_2(Arbol, final);

            }
            ruta(Arbol, RutaSolucion);


        }

        private Point MouseDownLocation; //posicion en la que se hace click al seleccionar una ficha
        private Label selected = null;   //selecciona el label del objeto clickeado
        private Point aux1, aux2, auxLoc, auxLoc2;
        private Label auxLabel;
        private bool made;
        private int position;  //auxiliar para indicar la posicion en el arreglo del numero seleccionado

        private bool IsSolvable; //Para verificar la paridad del algoritmo


        //Función que crea automáticamente todos los 81 labels necesarios para el tamaño maxmo del puzzle
        public void CreateLabels()
        {
            for (int i = 0; i < 81; i++)
            {
                Label label = new Label();
                label.Name = (i + 1).ToString();
                label.AutoSize = false;
                label.BackColor = System.Drawing.SystemColors.AppWorkspace;
                label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                label.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                label.Location = new System.Drawing.Point(240, 40);
                label.Size = new System.Drawing.Size(50, 50);
                label.TabIndex = 0;
                label.Text = (i + 1).ToString();
                label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                label.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ObjClicked);
                label.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ObjMove);
                label.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ObjChangePosition);
                numbers1[i] = label;
                numbers1[i].Visible = false;
                Controls.Add(numbers1[i]);
            }
            numbers1[80].Name = "0";
            numbers1[80].Text = " ";
            numbers1[80].BackColor = System.Drawing.SystemColors.ControlDarkDark;
        }

        //Función que reinicia la lista, le asigna los numeros dependiendo de n y los dibuja en pantalla
        public void DrawPieces()
        {
            for (int i = 0; i < SelectNumbers.Count; i++)
            {
                SelectNumbers[i].Visible = false;
            }

            SelectNumbers.Clear();
            ListaSolucion.Clear();

            for (int i = 0; i < n * n; i++)
            {
                if (i != (n * n) - 1)
                {
                    SelectNumbers.Add(numbers1[i]);
                    ListaSolucion.Add(numbers1[i]);
                }
                else
                {
                    SelectNumbers.Add(numbers1[80]);
                    ListaSolucion.Add(numbers1[80]);
                }
            }

            DrawFromSelected(SelectNumbers);
        }

        // Dibuja loc controles adicionales del tablero
        public void drawControlls()
        {
            this.botonResolver.Visible = true;
        }

        private void ObjClicked(object sender, MouseEventArgs e)
        {
            selected = sender as Label;
            if (rBtnEstInicial.Checked == true) { ClickauxFunc(ref SelectNumbers, e); }
            else { ClickauxFunc(ref ListaSolucion, e); }

        }

        public void ClickauxFunc(ref List<Label> AuxList, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;

                aux1 = selected.Location;  //guarda la posicion de la ficha antes de ser movida
                selected.BringToFront();
                for (int i = 0; i < n * n; i++) //encuentra la posicion en el arreglo de la ficha
                {
                    if (aux1 == AuxList[i].Location)
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

        private void ObjChangePosition(object sender, MouseEventArgs e)
        {
            if (rBtnEstInicial.Checked == true) { ChangePositions(ref SelectNumbers, e); }
            else { ChangePositions(ref ListaSolucion, e); }
        }

        public void ChangePositions(ref List<Label> ListaL, MouseEventArgs e)
        {
            made = false;
            if (selected != null)
            {
                for (int i = 0; i < n * n; i++)
                {
                    /* condicional que verifica que cuando el mouse sea levantado, 
                     * sea dentro de los limites de alguna de las fichas diferentes a la seleccionada*/
                    if ((selected.Left + e.X) > ListaL[i].Left && (selected.Left + e.X) < (ListaL[i].Left + ListaL[i].Width) && (selected.Top + e.Y) > ListaL[i].Top && (selected.Top + e.Y) < (ListaL[i].Top + ListaL[i].Height) && made == false && i != position)
                    {
                        //se actualizan las posiciones de los labels visualmente y en el arreglo

                        aux2 = ListaL[i].Location;
                        selected.Location = aux2;
                        ListaL[i].Location = aux1;

                        auxLabel = ListaL[i];

                        ListaL[i] = ListaL[position];
                        ListaL[position] = auxLabel;

                        made = true;  //indica si ya se realizo el cambio para que la funcion no vuelva a entrar al if
                    }
                }
                /* si no se encontró concordancia con ninguna de las otras fichas, 
                   devuelve la seleccionada a su posicion inicial*/
                if (made == false)
                {
                    selected.Location = aux1;
                    made = true;
                }
            }
            selected = null;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //actualiza el valor de n y vuelve a dibujar
        private void CmbTamanyos_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (CmbTamanyos.SelectedIndex)
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
            drawControlls();                    //Dibujar los controles adicionales para el rompecabezas
        }


        //Función para verificar la pariedad de la solución

        public void HallarInversiones(List<Label> Lista, ref int invert, ref int rowNum)
        {
            int columnCount = 0;
            int auxPrevio, auxSiguiente;
            // Conteno de inversiones en las fichas
            for (int i = 0; i < n * n; i++)
            {
                if (Lista[i].Name != "0")
                {
                    for (int j = i + 1; j < n * n; j++)
                    {
                        auxPrevio = int.Parse(Lista[i].Name);           // Auxiliar de la etiqueta de la posición previa convertido de Label a Int
                        if (Lista[j].Name != "0")
                        {
                            auxSiguiente = int.Parse(Lista[j].Name);    // Auxiliar de la etiqueta de la posición siguiente convertido de Label a Int
                            if (auxPrevio > auxSiguiente)
                            {
                                invert++;
                            }
                        }
                    }
                }
            }

            for (int i = (n * n) - 1; i >= 0; i--)
            {
                columnCount++;
                if (columnCount == n + 1)
                {
                    columnCount = 0;
                    rowNum++;
                }
                if (SelectNumbers[i].Name == "0")
                {
                    break;
                }
            }
        }

        public bool paridadSolucion()
        {
            int invertInicial = 0, invertFinal = 0, rowNumIni = 1, rowNumFin = 1;

            HallarInversiones(SelectNumbers, ref invertInicial, ref rowNumIni);
            HallarInversiones(ListaSolucion, ref invertFinal, ref rowNumFin);

            IsSolvable = false;
            // CONDICIONES

            //Si n es impar 
            if (n % 2 == 1)
            {
                if ((invertInicial % 2 == 0 && invertFinal % 2 == 0) || (invertInicial % 2 == 1 && invertFinal % 2 == 1))
                {
                    IsSolvable = true;
                }

            }
            else
            {
                if ((invertInicial % 2 == 0 && rowNumIni % 2 == 1) || (invertInicial % 2 == 1 && rowNumIni % 2 == 0))
                {
                    if ((invertFinal % 2 == 0 && rowNumFin % 2 == 1) || (invertFinal % 2 == 1 && rowNumFin % 2 == 0))
                    {
                        IsSolvable = true;
                    }
                }
                else
                {
                    if ((invertInicial % 2 == 0 && rowNumIni % 2 == 0) || (invertInicial % 2 == 1 && rowNumIni % 2 == 1))
                    {
                        if ((invertFinal % 2 == 0 && rowNumFin % 2 == 0) || (invertFinal % 2 == 1 && rowNumFin % 2 == 1))
                        {
                            IsSolvable = true;
                        }
                    }
                }
            }
            /*

            // CONDICIONES
           

            // Si N es impar y el número de inversiones es par en el estado de entrada.
            if (n % 2 == 1 && invert % 2 == 0)
            {
                verificarParidad = true;                    // SOLUCIONABLE
            }
            else

            // Si N es par, el blanco está en una fila par y el numero de inversiones es impar
            if (n % 2 == 0 && rowNum % 2 == 0 && invert % 2 == 1)
            {
                verificarParidad = true;                    // SOLUCIONABLE
            }
            else

            // Si N es par, el blanco está en una fila impar y el número de inversiones es par. 
            if (n % 2 == 0 && rowNum % 2 == 1 && invert % 2 == 0)
            {
                verificarParidad = true;                    // SOLUCIONABLE
            }

            // Cuando no se cumple ninguna de las condiciones anteriores 
            else
            {
                verificarParidad = false;                   // NO SOLUCIONABLE
            }*/

            return IsSolvable;
        }

        //Para que sea visible el mensaje de verificación de paridado
        private void botonResolver_Click(object sender, EventArgs e)
        {
            paridadSolucion();

            if (IsSolvable == false)
            {
                MessageBox.Show("No es posible resolver este rompecabeza. Intente acomodar las fichas nuevamente");
            }
            else
            {
                this.labelMovimientos.Visible = true;
                this.textoMovimientos.Visible = true;
                this.labelTiempo.Visible = true;
                this.textoTiempo.Visible = true;
                rBtnEstInicial.Checked = true;
                iniciarLogica(SelectNumbers);

                /*ThreadStart start = new ThreadStart(Animacion);
                Thread myThread = new Thread(start);
                myThread.Start();*/
                Animacion();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void rBtnEstInicial_CheckedChanged(object sender, EventArgs e)
        {
            if (rBtnEstInicial.Checked == true)
            {
                DrawFromSelected(SelectNumbers);
            }
            else
            {
                DrawFromSelected(ListaSolucion);
            }
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        public void DrawFromSelected(List<Label> Lista)
        {
            int county = 0, posX = 40, posY = 40;

            for (int i = 0; i < n * n; i++)
            {
                Lista[i].Location = new System.Drawing.Point(posX, posY);
                Lista[i].Visible = true;
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
        public async void Animacion()
        {
            Label auxLabel;
            int auxIndex, auxNum;
            List<int> auxNumbers = new List<int>();
            Point auxLoc;

            for (int i = 0; i < n * n; i++)
            {
                auxNumbers.Add(Convert.ToInt32(SelectNumbers[i].Name));
            }

            for (int i = 0; i < RutaSolucion.Count; i++)
            {
                for (int j = 0; j < n * n; j++)
                {
                    auxIndex = auxNumbers.IndexOf(RutaSolucion[i].Ldatos[j]);
                    auxNum = auxNumbers[j];
                    auxLabel = SelectNumbers[j];
                    //auxLoc = SelectNumbers[j].Location;

                    auxNumbers[j] = auxNumbers[auxIndex];
                    auxNumbers[auxIndex] = auxNum;

                    SelectNumbers[j] = SelectNumbers[auxIndex];
                    SelectNumbers[auxIndex] = auxLabel;

                }
                DrawFromSelected(SelectNumbers);
                await Task.Delay(1500);
            }

        }
    }
}