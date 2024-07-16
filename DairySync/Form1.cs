using System;
using MySql.Data.MySqlClient;
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
    public static class Configuracion
    {
        public static string CadenaConexion { get; } = "server=localhost;port=3306;database=dairysync;uid=root;password=2693a79a42;";
    }


    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }
        public string connectionString = Configuracion.CadenaConexion;

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            // Se establece usuario y contraseña para el login, si son correctas se crea el form 2 y se esconde el primero.
            if (textBox1.Text == "user" && textBox2.Text == "equipo4") {
                Form f2 = new Form2();
                f2.Show();
                this.Hide(); }


        }
    }

        
    }

