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
            //Se crea la variable del sp y la que obtendra el valor de la caja de texto
            int id_producto = int.Parse(textBox1.Text);
            string spname = "elimProductos";



            //Se inicia la conexion solo si no esta abierta
            MySqlConnection conexion = conexionBD.ObtenerConexion();
            
            conexionBD.AbrirConexion();





         //Se crea la variable cmd que contiene la instancia del comando a la vez que se crea el comando con sus parametros correspondientes.
         MySqlCommand cmd = new MySqlCommand(spname, conexion);

         //Especificamos que es un tipo de comando stored procedure
                    
         cmd.CommandType = CommandType.StoredProcedure;
         cmd.Parameters.Add(new MySqlParameter("@id_producto1", MySqlDbType.Int32)).Value = id_producto;


         //Se ejecuta el comando de consulta que no devolvera nada y mostrara mensaje solo si afecto filas.
         int filasafectadas = cmd.ExecuteNonQuery();
            if (filasafectadas > 0)
               {
                MessageBox.Show("El producto se ha eliminado exitosamente.");

                insertarProductos();
            }
            conexionBD.CerrarConexion();
                    
                    
                    
                    
                
                
                   
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
    }
}
