using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DairySync
{
    public partial class Form7 : Form
    {
        ConexionBD conexionBD = ConexionBD.Instancia;












        private int ObtenerIdProducto(string descripcion)
        {
            MySqlConnection conexion = conexionBD.ObtenerConexion();

            string query = "SELECT id_producto FROM productos WHERE descripcion = @descripcion";
            MySqlCommand cmd = new MySqlCommand(query, conexion);
            cmd.Parameters.AddWithValue("@descripcion", descripcion);


            object result = cmd.ExecuteScalar();
            if (result != null)
            {
                return Convert.ToInt32(result);
            }
            else
            {
                throw new Exception("Producto no encontrado.");
            }

        }














        private void insertarProductos()
        {
            string query = "SELECT id_producto, descripcion, stock, precio FROM productos";
            MySqlConnection conexion = ConexionBD.Instancia.ObtenerConexion();

            MySqlDataAdapter adapter = new MySqlDataAdapter(query, conexion);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;

        }





        private void CalcularTotal()
        {
            decimal total = 0;
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                decimal precio = Convert.ToDecimal(row.Cells["precio"].Value);
                int cantidad = Convert.ToInt32(row.Cells["cantidad"].Value);
                total += precio * cantidad;
            }
            label2.Text = "Total: " + total.ToString("C");




        }




        public Form7()
        {
            InitializeComponent();
            dataGridView2.Columns.Add("descripcion", "Descripción");
            dataGridView2.Columns.Add("cantidad", "Cantidad");
            dataGridView2.Columns.Add("precio", "Precio");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Verificar si hay filas en el datagrid
            if (dataGridView2.Rows.Count == 0 || dataGridView2.Rows.Cast<DataGridViewRow>().All(row => row.IsNewRow || row.Cells["descripcion"].Value == null))
            {
                MessageBox.Show("No hay filas seleccionadas para procesar la venta.");
                return;
            }

            MySqlTransaction transaccion = null;
            decimal totalCobrado = 0;
            MySqlConnection conexion = conexionBD.ObtenerConexion();

            try
            {
                transaccion = conexion.BeginTransaction();

                // Lista para almacenar los productos con stock bajo
                List<string> productosBajoStock = new List<string>();

                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    // Verificar si la fila es una fila nueva o si no tiene valores validos
                    if (row.IsNewRow || row.Cells["descripcion"].Value == null || row.Cells["cantidad"].Value == null || row.Cells["precio"].Value == null)
                    {
                        continue;
                    }

                    string descripcion = row.Cells["descripcion"].Value.ToString();
                    int cantidad = Convert.ToInt32(row.Cells["cantidad"].Value);
                    decimal precio = Convert.ToDecimal(row.Cells["precio"].Value);

                    int idProducto = ObtenerIdProducto(descripcion);
                    decimal ingresoVenta = cantidad * precio;

                    // Insertar venta en la tabla ventas con la fecha actual
                    string queryVenta = "INSERT INTO ventas (id_producto, ingreso_venta, cant_vendida, fecha) VALUES (@idProducto, @ingresoVenta, @cantidad, @fecha)";
                    MySqlCommand cmdVenta = new MySqlCommand(queryVenta, conexion, transaccion);
                    cmdVenta.Parameters.AddWithValue("@idProducto", idProducto);
                    cmdVenta.Parameters.AddWithValue("@ingresoVenta", ingresoVenta);
                    cmdVenta.Parameters.AddWithValue("@cantidad", cantidad);
                    cmdVenta.Parameters.AddWithValue("@fecha", DateTime.Now); // Asignar la fecha actual
                    cmdVenta.ExecuteNonQuery();

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

                    // Actualizar stock en la tabla productos
                    string queryUpdateStock = "UPDATE productos SET stock = stock - @cantidad WHERE id_producto = @idProducto";
                    MySqlCommand cmdUpdateStock = new MySqlCommand(queryUpdateStock, conexion, transaccion);
                    cmdUpdateStock.Parameters.AddWithValue("@cantidad", cantidad);
                    cmdUpdateStock.Parameters.AddWithValue("@idProducto", idProducto);
                    cmdUpdateStock.ExecuteNonQuery();

                    // Verificar si el stock después de la venta esta por debajo del stock mínimo
                    if (stockActual - cantidad < stockMinimo)
                    {
                        productosBajoStock.Add(descripcion);
                    }

                    // Registrar el total cobrado
                    totalCobrado += ingresoVenta;
                }

                // Insertar un único registro en la tabla clientes
                if (dataGridView2.Rows.Count > 0)
                {
                    // Obtener el último id_venta generado
                    string queryLastIdVenta = "SELECT LAST_INSERT_ID()";
                    MySqlCommand cmdLastIdVenta = new MySqlCommand(queryLastIdVenta, conexion);
                    int ultimoIdVenta = Convert.ToInt32(cmdLastIdVenta.ExecuteScalar());

                    string queryInsertCliente = "INSERT INTO clientes (id_venta) VALUES (@idVenta)";
                    MySqlCommand cmdInsertCliente = new MySqlCommand(queryInsertCliente, conexion, transaccion);
                    cmdInsertCliente.Parameters.AddWithValue("@idVenta", ultimoIdVenta);
                    cmdInsertCliente.ExecuteNonQuery();
                }

                transaccion.Commit();
                MessageBox.Show("Venta realizada y stock actualizado correctamente.");

                // Mostrar alerta si hay productos con stock bajo
                if (productosBajoStock.Count > 0)
                {
                    string mensaje = "Atención: Los siguientes productos tienen stock por debajo del mínimo:\n" + string.Join("\n", productosBajoStock);
                    MessageBox.Show(mensaje, "Alerta de Stock Bajo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // Limpiar DataGridView2 después de la venta
                dataGridView2.Rows.Clear();

                // recargar los productos en el DataGrid después de la venta
                insertarProductos();
            }
            catch (Exception ex)
            {
                if (transaccion != null)
                {
                    transaccion.Rollback();
                }
                MessageBox.Show("Error al realizar la venta: " + ex.Message);
            }
            finally
            {
                if (conexion.State == System.Data.ConnectionState.Open)
                {
                    conexion.Close();
                }


            }

            }




        private void Form7_Load(object sender, EventArgs e)
        {   //Se llama a la funcion que inserta productos al datagrid una vez este cargado el formulario.
            insertarProductos();







        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                //Se almacena la fila seleccionada del dataGridView de la izquierda en la variable selectedRow.
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                //Se obtienen los valores necesarios de la fila seleccionada
                string nombreProducto = selectedRow.Cells["descripcion"].Value.ToString();
                int cantidad = 1; 
                decimal precio = Convert.ToDecimal(selectedRow.Cells["precio"].Value);

                // Añadir fila al dataGridView2
                dataGridView2.Rows.Add(nombreProducto, cantidad, precio);

                // Calcular el total si es necesario
                CalcularTotal();
            }
            else
            {
                MessageBox.Show("Selecciona la fila entera.");
            }
        
    }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.Show(); this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            
        }
    } 
            

}
    

