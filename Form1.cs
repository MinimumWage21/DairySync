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
using BCrypt.Net;


















namespace DairySync
{





    public partial class Form1 : Form
    {


        private void RegisterUser(string username, string password)
        {
            string hashedPassword = PasswordHashing.HashPassword(password);

            // Aquí deberías agregar código para insertar `username` y `hashedPassword` en la base de datos.
        }






        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {

            // Se establece usuario y contraseña para el login, si son correctas se crea el form 3 y se esconde el primero.


            if (textBox1.Text != "" && textBox2.Text != "")
            {
                string usuario = textBox1.Text;
                string password = textBox2.Text;
                string hashedPassword = PasswordHashing.HashPassword(password);










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
            catch (MySqlException )
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









    public static class PasswordHashing
    {
        // Método para hashear una contraseña
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Método para verificar una contraseña contra un hash
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }










    }







}

