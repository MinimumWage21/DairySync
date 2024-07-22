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
            int stock;
            int stock_min;
            decimal precio;
            int id_producto;

            try
            {
                // Validaciones de entrada
                if (string.IsNullOrWhiteSpace(nombre))
                {
                    MessageBox.Show("El nombre del producto no puede estar vacío.");
                    return;
                }

                if (!int.TryParse(textBox2.Text, out stock) || stock < 0)
                {
                    MessageBox.Show("El stock debe ser un número entero positivo.");
                    return;
                }

                if (!int.TryParse(textBox3.Text, out stock_min) || stock_min < 0)
                {
                    MessageBox.Show("El stock mínimo debe ser un número entero positivo.");
                    return;
                }

                if (!decimal.TryParse(textBox4.Text, out precio) || precio < 0)
                {
                    MessageBox.Show("El precio debe ser un número positivo.");
                    return;
                }

                if (!int.TryParse(textBox5.Text, out id_producto) || id_producto < 0)
                {
                    MessageBox.Show("El ID del producto debe ser un número entero positivo.");
                    return;
                }

                // Advertencia si el stock inicial es menor que el stock mínimo
                if (stock < stock_min)
                {
                    MessageBox.Show("Advertencia: el valor del stock inicial es menor al valor mínimo.");
                }

                // Consulta para insertar el producto
                string query = "INSERT INTO productos (id_producto, descripcion, stock, stock_min, precio) VALUES (@id_producto, @descripcion, @stock, @stock_min, @precio)";

                // Obtener la conexión
                MySqlConnection conexion = ConexionBD.Instancia.ObtenerConexion();
                
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        // Agregar parametros
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
                        }
                        else
                        {
                            MessageBox.Show("No se pudo insertar el registro.");
                        }
                    }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar el registro: " + ex.Message);
            }
           




        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
            f4.Show();
            this.Close();
            conexionBD.CerrarConexion();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
            
        }
    } }

