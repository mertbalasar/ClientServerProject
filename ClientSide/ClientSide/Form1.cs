using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientSide
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ConsoleWrite(string text)
        {
            MethodInvoker inv = delegate
            {
                richTextBox1.Text += text;
            };
            Invoke(inv);
        }

        async private void SendMessage(string message)
        {
            decimal portNumber = numericUpDown1.Value;
            bool succ = true;
            try
            {
                TcpClient client = new TcpClient("localhost", Convert.ToInt32(portNumber));
                NetworkStream stream = client.GetStream();
                byte[] sendData = Encoding.ASCII.GetBytes(message);
                await stream.WriteAsync(sendData, 0, sendData.Length);
                stream.Flush();
                stream.Close();
                client.Close();
            }
            catch
            {
                succ = false;
            }
            if (succ)
            {
                ConsoleWrite(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " SUCCESS > " + message + "\n");
            }
            else
            {
                ConsoleWrite(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " FAILED > " + message + "\n");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SendMessage("A1 Sent");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SendMessage("A2 Sent");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SendMessage("B1 Sent");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SendMessage("B2 Sent");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }
    }
}
