using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DairySync
{
    public partial class Form6 : Form
    {
        private ConexionBD conexionBD = ConexionBD.Instancia;


        private void insertarProductos()
        {
            string query = "SELECT id_producto, descripcion, stock, precio FROM productos";
            MySqlConnection conexion = ConexionBD.Instancia.ObtenerConexion();
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, conexion);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;

        }





        public Form6()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Verificar que la caja de texto contiene un número entero
            if (!int.TryParse(textBox1.Text, out int id_producto))
            {
                MessageBox.Show("Por favor, introduce un número entero válido para el ID del producto.");
                return;
            }

            // Conectar a la base de datos
            MySqlConnection conexion = conexionBD.ObtenerConexion();
            conexionBD.AbrirConexion();

            try
            {
                // Verificar si el producto existe en la base de datos
                string queryCheckExistencia = "SELECT COUNT(*) FROM productos WHERE id_producto = @idProducto";
                MySqlCommand cmdCheckExistencia = new MySqlCommand(queryCheckExistencia, conexion);
                cmdCheckExistencia.Parameters.AddWithValue("@idProducto", id_producto);
                int count = Convert.ToInt32(cmdCheckExistencia.ExecuteScalar());

                if (count == 0)
                {
                    MessageBox.Show("El producto con el ID proporcionado no existe.");
                    return;
                }

                // Eliminar el producto de la base de datos usando el procedimiento almacenado
                MySqlCommand cmdEliminar = new MySqlCommand("elimProductos", conexion);
                cmdEliminar.CommandType = CommandType.StoredProcedure;
                cmdEliminar.Parameters.AddWithValue("id_producto1", id_producto);
                int filasAfectadas = cmdEliminar.ExecuteNonQuery();

                if (filasAfectadas > 0)
                {
                    MessageBox.Show("Producto eliminado exitosamente.");
                    insertarProductos();
                }
                else
                {
                    MessageBox.Show("No se pudo eliminar el producto. Verifique el ID e intente nuevamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el producto: " + ex.Message);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }







        }

        private void Form6_Load(object sender, EventArgs e)
        {
            insertarProductos();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
            f4.Show();
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
