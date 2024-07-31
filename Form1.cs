using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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


            if (textBox1.Text != "" && textBox2.Text != "")
            {
                string usuario = textBox1.Text;
                string password = textBox2.Text;
                ConexionBD.Instancia.ActualizarConexion(usuario, password);

                // Verificar si la conexión fue exitosa
                if (ConexionBD.Instancia.AbrirConexion())
                {
                    MessageBox.Show("Conexión exitosa", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Form3 f3 = new Form3();
                    f3.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Error al conectar a la base de datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingrese su usuario y contraseña", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }




        }


    }

       
    
    public sealed class ConexionBD
    {
        private static readonly ConexionBD instancia = new ConexionBD();
        private MySqlConnection conexion;
    private string connectionString;


    public void ActualizarConexion(string usuario, string password)
    {
        connectionString = $"Server=localhost;Port=3306;Database=dairysync;Uid={usuario};Pwd={password};";
        conexion = new MySqlConnection(connectionString);
    }




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
        public bool AbrirConexion()
        {
            try
            {
                if (conexion.State != ConnectionState.Open)
                {
                    conexion.Open();
                }
                return true;
            }
            catch (MySqlException ex)
            {
                // Manejo de excepciones de conexión aquí si es necesario
                return false;
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

