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
    public partial class Form9 : Form
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




        ConexionBD conexionBD = ConexionBD.Instancia;





        public Form9()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox2.Text, out int cantidad) && cantidad > 0)
            {
                if (!string.IsNullOrEmpty(textBox1.Text) && int.TryParse(textBox1.Text, out int idProducto))
                {
                    try
                    {
                        MySqlConnection conexion = conexionBD.ObtenerConexion();
                        
                            conexionBD.AbrirConexion();

                            // Obtener el stock actual y el stock minimo del producto
                            string queryStock = "SELECT stock, stock_min FROM productos WHERE id_producto = @idProducto";
                            MySqlCommand cmdStock = new MySqlCommand(queryStock, conexion);
                            cmdStock.Parameters.AddWithValue("@idProducto", idProducto);

                            MySqlDataReader reader = cmdStock.ExecuteReader();

                            int stockActual = 0;
                            int stockMinimo = 0;
                            if (reader.Read())
                            {
                                stockActual = reader.GetInt32("stock");
                                stockMinimo = reader.GetInt32("stock_min");
                            }
                            reader.Close();

                            // Verificar si el stock despues de la operacion quedara por debajo del stock minimo
                            if (stockActual - cantidad < stockMinimo)
                            {
                                MessageBox.Show("No se puede quitar la cantidad solicitada. El stock quedaría por debajo del mínimo permitido.");
                                return;
                            }

                            // Actualizar stock en la tabla productos
                            string queryUpdateStock = "UPDATE productos SET stock = stock - @cantidad WHERE id_producto = @idProducto";
                            MySqlCommand cmdUpdateStock = new MySqlCommand(queryUpdateStock, conexion);
                            cmdUpdateStock.Parameters.AddWithValue("@cantidad", cantidad);
                            cmdUpdateStock.Parameters.AddWithValue("@idProducto", idProducto);

                            int filasAfectadas = cmdUpdateStock.ExecuteNonQuery();

                            if (filasAfectadas > 0)
                            {
                                MessageBox.Show("Cantidad eliminada del stock correctamente.");
                            }
                            else
                            {
                                MessageBox.Show("No se encontró el producto con el ID proporcionado.");
                            }

                            //recargar los productos en el DataGridView1 después de la actualización
                            insertarProductos();
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al actualizar el stock: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, ingrese un ID de producto válido.");
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingrese una cantidad válida y mayor que cero.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox2.Text, out int cantidad) && cantidad > 0)
            {
                if (!string.IsNullOrEmpty(textBox1.Text) && int.TryParse(textBox1.Text, out int idProducto))
                {
                    try
                    {
                            MySqlConnection conexion = conexionBD.ObtenerConexion();
                        
                            string queryUpdateStock = "UPDATE productos SET stock = stock + @cantidad WHERE id_producto = @idProducto";
                            MySqlCommand cmdUpdateStock = new MySqlCommand(queryUpdateStock, conexion);
                            cmdUpdateStock.Parameters.AddWithValue("@cantidad", cantidad);
                            cmdUpdateStock.Parameters.AddWithValue("@idProducto", idProducto);

                            
                            int filasAfectadas = cmdUpdateStock.ExecuteNonQuery();
                            conexion.Close();

                            if (filasAfectadas > 0)
                            {
                                MessageBox.Show("Cantidad añadida al stock correctamente.");
                            }
                            else
                            {
                                MessageBox.Show("No se encontró el producto con el ID proporcionado.");
                            }

                            //recargar los productos en el DataGrid despues de la actualizacion
                            insertarProductos();
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al actualizar el stock: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, ingrese un ID de producto válido.");
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingrese una cantidad válida y mayor que cero.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
            f4.Show(); this.Close();
        }

        private void Form9_Load(object sender, EventArgs e)
        {
            insertarProductos();
        }
    }
}
