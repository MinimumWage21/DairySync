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

    public partial class Form5 : Form
    {
        private void insertarProductos()
        {
            string query = "SELECT id_producto, descripcion, stock, precio FROM productos";
            MySqlConnection conexion = ConexionBD.Instancia.ObtenerConexion();
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conexion);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            
        }
        private ConexionBD conexionBD = ConexionBD.Instancia;

        public Form5()
        {
            InitializeComponent();


        }
        private void Form5_Load(object sender, EventArgs e)
        {

            insertarProductos();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            string nombre = textBox1.Text;
            int stock = Convert.ToInt32(textBox2.Text);
            decimal precio = decimal.Parse(textBox4.Text);
            int stock_min = Convert.ToInt32(textBox3.Text);
            int id_producto = Convert.ToInt32(textBox5.Text);
            string query = "INSERT INTO productos (id_producto,descripcion, stock,stock_min, precio) VALUES (@id_producto,@descripcion, @stock,@stock_min, @precio)";


            {
                
                {
                   
                    //obtener la conexion dentro de la variable conexion
                    MySqlConnection conexion = ConexionBD.Instancia.ObtenerConexion();


                    
                  
                        conexionBD.AbrirConexion();
                    

                    // Crear el comando SQL dentro del 'using' y con 'using' para cmd
                    MySqlCommand cmd = new MySqlCommand(query, conexion);
                    {
                        // Agregar parámetros
                        cmd.Parameters.AddWithValue("@descripcion", nombre);
                        cmd.Parameters.AddWithValue("@stock", stock);

                        cmd.Parameters.AddWithValue("@stock_min", stock_min);
                        cmd.Parameters.AddWithValue("@precio", precio);
                        cmd.Parameters.AddWithValue("@id_producto", id_producto);

                        // Ejecutar la consulta
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Registro insertado correctamente.");

                            insertarProductos();

                            conexion.Close();
                        }
                        else
                        {
                            MessageBox.Show("No se pudo insertar el registro.");
                        }
                    }
                }
                }





        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
            f4.Show();
            this.Close();
        }

        
    } }

