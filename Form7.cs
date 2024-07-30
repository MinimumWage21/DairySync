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

                // Verificación de stock total antes de realizar las inserciones
                Dictionary<int, int> productosAComprar = new Dictionary<int, int>();
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    // Verificar si la fila es una fila nueva o si no tiene valores válidos
                    if (row.IsNewRow || row.Cells["descripcion"].Value == null || row.Cells["cantidad"].Value == null || row.Cells["precio"].Value == null)
                    {
                        continue;
                    }

                    string descripcion = row.Cells["descripcion"].Value.ToString();
                    int cantidad = Convert.ToInt32(row.Cells["cantidad"].Value);

                    int idProducto = ObtenerIdProducto(descripcion);

                    // Verificar si ya hay un registro para este producto en el diccionario
                    if (productosAComprar.ContainsKey(idProducto))
                    {
                        productosAComprar[idProducto] += cantidad;
                    }
                    else
                    {
                        productosAComprar[idProducto] = cantidad;
                    }
                }

                // Verificar el stock disponible para cada producto
                foreach (var producto in productosAComprar)
                {
                    int idProducto = producto.Key;
                    int cantidadAComprar = producto.Value;

                    // Obtener el stock actual del producto
                    string queryStock = "SELECT stock FROM productos WHERE id_producto = @idProducto";
                    MySqlCommand cmdStock = new MySqlCommand(queryStock, conexion);
                    cmdStock.Parameters.AddWithValue("@idProducto", idProducto);
                    MySqlDataReader reader = cmdStock.ExecuteReader();

                    int stockActual = 0;
                    if (reader.Read())
                    {
                        stockActual = reader.GetInt32("stock");
                    }
                    reader.Close();

                    // Verificar si la cantidad a vender excede el stock actual
                    if (cantidadAComprar > stockActual)
                    {
                        MessageBox.Show("No se puede vender más de lo que hay en stock.");
                        transaccion.Rollback();
                        return;
                    }
                }

                // Obtener el último id_venta y calcular el nuevo id_venta
                string queryLastIdVenta = "SELECT IFNULL(MAX(id_venta), 0) FROM ventas";
                MySqlCommand cmdLastIdVenta = new MySqlCommand(queryLastIdVenta, conexion, transaccion);
                int nuevoIdVenta = Convert.ToInt32(cmdLastIdVenta.ExecuteScalar()) + 1;

                // Procesar la venta y actualizar el stock
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    // Verificar si la fila es una fila nueva o si no tiene valores válidos
                    if (row.IsNewRow || row.Cells["descripcion"].Value == null || row.Cells["cantidad"].Value == null || row.Cells["precio"].Value == null)
                    {
                        continue;
                    }

                    string descripcion = row.Cells["descripcion"].Value.ToString();
                    int cantidad = Convert.ToInt32(row.Cells["cantidad"].Value);
                    decimal precio = Convert.ToDecimal(row.Cells["precio"].Value);

                    int idProducto = ObtenerIdProducto(descripcion);
                    decimal ingresoVenta = cantidad * precio;

                    // Insertar venta en la tabla ventas con el id_venta obtenido
                    string queryDetalleVenta = "INSERT INTO ventas (id_venta, id_producto, ingreso_venta, cant_vendida, fecha) VALUES (@idVenta, @idProducto, @ingresoVenta, @cantidad, @fecha)";
                    MySqlCommand cmdDetalleVenta = new MySqlCommand(queryDetalleVenta, conexion, transaccion);
                    cmdDetalleVenta.Parameters.AddWithValue("@idVenta", nuevoIdVenta);
                    cmdDetalleVenta.Parameters.AddWithValue("@idProducto", idProducto);
                    cmdDetalleVenta.Parameters.AddWithValue("@ingresoVenta", ingresoVenta);
                    cmdDetalleVenta.Parameters.AddWithValue("@cantidad", cantidad);
                    cmdDetalleVenta.Parameters.AddWithValue("@fecha", DateTime.Now); // Asignar la fecha actual
                    cmdDetalleVenta.ExecuteNonQuery();

                    // Actualizar stock en la tabla productos
                    string queryUpdateStock = "UPDATE productos SET stock = stock - @cantidad WHERE id_producto = @idProducto";
                    MySqlCommand cmdUpdateStock = new MySqlCommand(queryUpdateStock, conexion, transaccion);
                    cmdUpdateStock.Parameters.AddWithValue("@cantidad", cantidad);
                    cmdUpdateStock.Parameters.AddWithValue("@idProducto", idProducto);
                    cmdUpdateStock.ExecuteNonQuery();

                    // Registrar el total cobrado
                    totalCobrado += ingresoVenta;
                }

                // Confirmar la transacción
                transaccion.Commit();

                MessageBox.Show("Venta realizada y stock actualizado correctamente.");

                // Mostrar alerta si hay productos con stock bajo
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    // Verificar si la fila es una fila nueva o si no tiene valores válidos
                    if (row.IsNewRow || row.Cells["descripcion"].Value == null || row.Cells["cantidad"].Value == null)
                    {
                        continue;
                    }

                    string descripcion = row.Cells["descripcion"].Value.ToString();
                    int cantidad = Convert.ToInt32(row.Cells["cantidad"].Value);

                    int idProducto = ObtenerIdProducto(descripcion);

                    // Obtener el stock actual y el stock mínimo del producto
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

                    // Verificar si el stock después de la venta está por debajo del stock mínimo
                    if (stockActual < stockMinimo)
                    {
                        productosBajoStock.Add(descripcion);
                    }
                }

                // Mostrar alerta si hay productos con stock bajo
                if (productosBajoStock.Count > 0)
                {
                    string mensaje = "Atención: Los siguientes productos tienen stock por debajo del mínimo:\n" + string.Join("\n", productosBajoStock);
                    MessageBox.Show(mensaje, "Alerta de Stock Bajo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // Limpiar DataGridView2 después de la venta
                dataGridView2.Rows.Clear();

                // Recargar los productos en el DataGrid después de la venta
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
            // Verificar si se ha seleccionado exactamente una fila
            if (dataGridView1.SelectedRows.Count == 1)
            {
                // Se almacena la fila seleccionada en la variable selectedRow
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Se obtienen los valores necesarios de la fila seleccionada
                string nombreProducto = selectedRow.Cells["descripcion"].Value.ToString();
                int cantidad = 1;
                decimal precio = Convert.ToDecimal(selectedRow.Cells["precio"].Value);

                // Añadir fila al dataGridView2
                dataGridView2.Rows.Add(nombreProducto, cantidad, precio);

                // Calcular el total si es necesario
                CalcularTotal();
            }
            else if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageBox.Show("Selecciona solo una fila.");
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

        private void button4_Click_1(object sender, EventArgs e)
        {
            // Limpiar DataGridView2
            dataGridView2.Rows.Clear();

            // Reiniciar el total a 0
            int totalCobrado = 0;
            label2.Text = $"Total: {totalCobrado:C}"; // Suponiendo que tienes una etiqueta para mostrar el total

        }
    } 
            

}
    

