using System;
using System.IO;
using System.Windows.Forms;
using Tulpep.NotificationWindow;
using System.Collections.Generic;

namespace RhinoApplication
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            if(!File.Exists("this.js"))
                File.Create("this.js");
            else
            {
                textBox1.Text = File.ReadAllText("this.js")+"";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            File.WriteAllText("this.js", textBox1.Text);
            Form1.function.print("Saved");
            textBox1.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = File.ReadAllText("this.js")+"";
            Form1.function.print("Loaded");
            textBox1.Focus();
        }



        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
            {
                File.WriteAllText("this.js", textBox1.Text);
                print("Saved");
                textBox1.Focus();
            }
            if ((e.KeyCode == Keys.L || e.KeyCode == Keys.O) && e.Control)
            {
                textBox1.Text = File.ReadAllText("this.js") + "";
                print("Loaded");
                textBox1.Focus();
            }
           

           

            if (e.KeyCode == Keys.F3)
            {
                this.Close();
            }
        }

        public static void print(string str)
        {
            PopupNotifier popup = new PopupNotifier();
            //popup.Image
            popup.TitleText = "Print Window";
            popup.TitleFont = new System.Drawing.Font("Arial", 20);
            popup.TitlePadding = new Padding(85, 10, 0, 0);
            popup.ContentPadding = new Padding(15, 10, 0, 15);
            popup.Size = new System.Drawing.Size(350, 300);
            popup.ContentText = str + "\r\n";
            popup.ContentFont = new System.Drawing.Font("Arial", 13);
            popup.HeaderColor = System.Drawing.Color.White;
            popup.TitleColor = System.Drawing.Color.Black;
            popup.ContentHoverColor = System.Drawing.Color.Black;
            popup.ContentColor = System.Drawing.Color.Gray;
            popup.ButtonHoverColor = System.Drawing.Color.White;
            popup.ShowGrip = false;
            popup.HeaderHeight = 1;
            popup.Popup();
        }
        

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox1.Focus();
        }
    }
}
