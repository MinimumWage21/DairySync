using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;





    












namespace DairySync
{

    



    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            // Se establece usuario y contraseña para el login, si son correctas se crea el form 2 y se esconde el primero.
            if (textBox1.Text == "" && textBox2.Text == "")
            {
                Form3 f3 = new Form3();
                f3.Show();
                this.Hide();
                
            }


        }

        private void label3_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
        }
    }
    public sealed class ConexionBD
    {
        private static readonly ConexionBD instancia = new ConexionBD();
        private MySqlConnection conexion;
        private string connectionString = "Server=localhost;Port=3306;Database=dairysync;Uid=root;Pwd=2693a79a42;";

        //Propiedad publica para instanciar la creacion de la conexion
        public static ConexionBD Instancia
        {
            get
            {
                return instancia;
            }
        }

        //Constructor privado para evitar instanciacion externa
        private ConexionBD()
        {
            conexion = new MySqlConnection(connectionString);

        }


        //Metodo para abrir conexion
        public void AbrirConexion()
        {
            if (conexion.State != ConnectionState.Open)
            {
                conexion.Open();
            }
        }
        //Obtener la conexion
        public MySqlConnection ObtenerConexion()
        {
            AbrirConexion();
            return conexion;
        }

        //Metodo para cerrar la conexion 
        public void CerrarConexion()
        {
            if ((conexion.State == System.Data.ConnectionState.Open))
            {
                conexion.Close();
            }
        }




    }
    
        
    




}

