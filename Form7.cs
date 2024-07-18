using MySql.Data.MySqlClient;
using System;
using System.Collections;
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
    public partial class Form7 : Form
    {
        ConexionBD conexionBD = ConexionBD.Instancia;



        private void insertarProductos()
        {
            string query = "SELECT id_producto, descripcion, stock, precio FROM productos";
            using (MySqlConnection conexion = ConexionBD.Instancia.ObtenerConexion())
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conexion);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
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
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
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
                int cantidad = 1; // Aquí podrías tener una variable que represente la cantidad ordenada, pero en tu caso siempre estás añadiendo 1.
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
            Form4 f4 = new Form4();
            f4.Show(); this.Hide();
        }
    } 
            

}
    

