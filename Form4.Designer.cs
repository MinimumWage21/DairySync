namespace DairySync
{
    partial class Form4
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mySqlConnectionBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.mySqlConnectionBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // mySqlConnectionBindingSource
            // 
            this.mySqlConnectionBindingSource.DataSource = typeof(MySql.Data.MySqlClient.MySqlConnection);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Tan;
            this.button1.Location = new System.Drawing.Point(464, 383);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(340, 215);
            this.button1.TabIndex = 0;
            this.button1.Text = "Introducir Producto";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Tan;
            this.button2.Location = new System.Drawing.Point(464, 651);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(340, 215);
            this.button2.TabIndex = 1;
            this.button2.Text = "Eliminar Producto";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.Tan;
            this.button4.Location = new System.Drawing.Point(971, 383);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(340, 215);
            this.button4.TabIndex = 3;
            this.button4.Text = "Visualizar Base De Datos";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button3.Location = new System.Drawing.Point(79, 858);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(221, 65);
            this.button3.TabIndex = 4;
            this.button3.Text = "Volver";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.Tan;
            this.button5.Location = new System.Drawing.Point(971, 651);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(340, 215);
            this.button5.TabIndex = 5;
            this.button5.Text = "Modificar Stock De Productos";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(236)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form4";
            this.Text = "Form4";
            ((System.ComponentModel.ISupportInitialize)(this.mySqlConnectionBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource mySqlConnectionBindingSource;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button5;
    }
}