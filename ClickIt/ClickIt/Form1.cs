using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace ClickIt
{
    public partial class Form1 : Form      
    {
        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(int vKey);
        [DllImport("user32.dll")]
        static extern byte VkKeyScan(char ch);
        public const int Press = 0x0001; //Key down flag
        public const int Release = 0x0002; //Key up flag   
        string keyBuffer = "";
        private float mouseX;
        private float mouseY;

        public Form1()       
        {
            
            InitializeComponent();
            var seznam = new Dictionary<string, byte>()
            {
                {"None", 0},
                {"MouseLeft", 0x02},
                {"MouseRight", 0x08},
                {"LCtrl", 0xA2},
                {"X", 0x58}
            };
            keysBox1.DataSource = seznam.ToList();
            keysBox1.DisplayMember = "Key";
            keysBox1.ValueMember = "Value";

            keysBox2.DataSource = seznam.ToList();
            keysBox2.DisplayMember = "Key";
            keysBox2.ValueMember = "Value";
            keysBox1.SelectedIndex = 1;

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt16(numericUpDown1.Value);
            timer1.Interval = i;
            timer1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            byte hodnota = Convert.ToByte(keysBox1.SelectedValue);
            byte hodnota2 = Convert.ToByte(keysBox2.SelectedValue);
            if (mouseProtect.Checked == true)
            {
                if(mouseX + 40 < MousePosition.X || mouseX - 40 > MousePosition.X || mouseY + 40 < MousePosition.Y || mouseY - 40 > MousePosition.Y)
                {
                    hodnota = 0;
                    hodnota2 = 0;
                }              
            }

            if (hodnota == 0 && hodnota2 == 0)
            {
                timer1.Stop();
                keyBuffer = "NumPad2";
            }          
            else{ 
                
                keybd_event(hodnota, 0, 0, 0);
                keybd_event(hodnota2, 0, 0, 0);
                keybd_event(hodnota2, 0, Release, 0);
                keybd_event(hodnota, 0, Release, 0);                             
                
                if (hodnota == 0x02 || hodnota2 == 0x02)
                {
                    mouse_event(hodnota, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                    mouse_event(0x04, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                }
                if (hodnota == 0x08 || hodnota2 == 0x08)
                {
                    mouse_event(hodnota, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                    mouse_event(0x10, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                }
            }
        }           
        private void timer2_Tick(object sender, EventArgs e)
        {
                       
            foreach (System.Int32 i in Enum.GetValues(typeof(Keys)))
            {
                if (GetAsyncKeyState(i) == -32767)
                keyBuffer = Enum.GetName(typeof(Keys), i);
            }
            label1.Text = keyBuffer;
            if (keyBuffer == "NumPad1")
            {
                mouseX = MousePosition.X;
                mouseY = MousePosition.Y;
                int i = Convert.ToInt16(numericUpDown1.Value);
                timer1.Interval = i;
                timer1.Start();
                keyBuffer = "RUN";
            }
            else if (keyBuffer == "NumPad2")
            {
                timer1.Stop();
            }
            else { }
        }
    }
}
