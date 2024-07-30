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

             

                default:
                    MessageBox.Show("No se seleccionó nada");
                    return;
            }

            MySqlConnection conexion = ConexionBD.Instancia.ObtenerConexion();


            // Crear el comando SQL con la consulta y la conexion
            MySqlCommand cmd = new MySqlCommand(query, conexion);

            // Abrir la conexión solo si no está abierta
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
    }
                





}
    

