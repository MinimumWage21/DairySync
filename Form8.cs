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
    public partial class Form8 : Form
    {
        ConexionBD conexionBD = ConexionBD.Instancia;
       

        public Form8()
        {
            InitializeComponent();
            
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            
        }

       

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {


            string query = "";

            switch (comboBox1.Text)
            {
                
                
                case "productos":
                    query = "SELECT * FROM productos";
                    break;

                case "ventas":
                    query = "SELECT * FROM ventas";
                    break;

                case "ventas(descendente)":
                    query = "SELECT * FROM ventas ORDER BY fecha DESC";
                    break;
             

                default:
                    MessageBox.Show("No se seleccionó nada");
                    return;
            }

            MySqlConnection conexion = ConexionBD.Instancia.ObtenerConexion();


            // Crear el comando SQL con la consulta y la conexion
            MySqlCommand cmd = new MySqlCommand(query, conexion);

            // Abrir la conexión solo si no esta abierta
            conexionBD.AbrirConexion();


            // Crear el adaptador y llenar el DataTable
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;

            conexionBD.CerrarConexion();
                        
                    
               
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
            f4.Show(); this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Verificar que la caja de texto contiene un numero entero válido
            if (!int.TryParse(textBox1.Text, out int id_venta))
            {
                MessageBox.Show("Por favor, introduce un número entero válido para el ID de la venta.");
                return;
            }

            // Abrir la conexion a la base de datos
            MySqlConnection conexion = conexionBD.ObtenerConexion();
            conexionBD.AbrirConexion();

            try
            {
                // Crear la consulta con un parametro
                string query = "SELECT * FROM ventas WHERE id_venta = @idVenta";

                // Crear el comando MySqlCommand
                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    // Añadir el parámetro a la consulta
                    cmd.Parameters.AddWithValue("@idVenta", id_venta);

                    // Ejecutar el comando y llenar el DataTable con los resultados
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Mostrar los resultados en el DataGridView
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar la venta: " + ex.Message);
            }
            finally
            {
                // Cerrar la conexión si esta abierta
                if (conexion.State == ConnectionState.Open)
                {
                    conexionBD.CerrarConexion();
                }
            }
        }
    }
                





}
    

