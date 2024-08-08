using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DairySync
{
    public partial class Form3 : Form
    {
        private ConexionBD conexionBD = ConexionBD.Instancia;

        private string ObtenerUsuarioConectado()
        {
            string usuario = string.Empty;
            MySqlConnection conexion = conexionBD.ObtenerConexion();
            conexionBD.AbrirConexion();

            try
            {
                string query = "SELECT USER()"; 
                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    object resultado = cmd.ExecuteScalar();
                    if (resultado != null)
                    {
                        usuario = resultado.ToString();
                        
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener el usuario conectado: " + ex.Message);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexionBD.CerrarConexion();
                }
            }

            return usuario;
        }






















        public Form3()
        {
            InitializeComponent();
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {


        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                // Obtener el usuario conectado
                string usuarioConectado = ObtenerUsuarioConectado();

                // Verificar si el usuario conectado no es 'vendedor'
                if (!usuarioConectado.StartsWith("vendedor", StringComparison.OrdinalIgnoreCase))
                {
                    // Mostrar el formulario 4
                    Form4 f4 = new Form4();
                    f4.Show();
                    this.Hide(); // Ocultar el formulario actual
                }
                else
                {
                    MessageBox.Show("Acceso restringido. No tienes permisos para acceder a esta opción.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al verificar el usuario: " + ex.Message);
            }




        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form7 f7 = new Form7();
            f7.Show(); this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show(); this.Close();
        }
    }
}
