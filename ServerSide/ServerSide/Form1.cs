using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerSide
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        TcpListener server;
        TcpClient client;
        Thread thread;

        async private void ListenMessage()
        {
            ConsoleWrite("LOG > Connected localhost:" + numericUpDown1.Value.ToString() + ", Listening...");
            while (true)
            {
                client = await server.AcceptTcpClientAsync();
                byte[] receivedBuffer = new byte[1024];
                NetworkStream stream = client.GetStream();
                stream.Read(receivedBuffer, 0, receivedBuffer.Length);
                string message = Encoding.ASCII.GetString(receivedBuffer);
                ConsoleWrite(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " > " + message);
            }
        }

        private void ConsoleWrite(string text)
        {
            MethodInvoker inv = delegate
            {
                if (string.IsNullOrEmpty(richTextBox1.Text)) richTextBox1.Text += text;
                else richTextBox1.Text += "\n" + text;
            };
            Invoke(inv);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            decimal portNumber = numericUpDown1.Value;
            IPAddress ip = Dns.GetHostEntry("localhost").AddressList[0];
            server = new TcpListener(ip, Convert.ToInt32(portNumber));
            bool succ = true;
            try
            {
                client = default(TcpClient);
                server.Start();
                thread = new Thread(new ThreadStart(ListenMessage));
                thread.Start();
                button1.Enabled = false;
            } catch
            {
                succ = false;
            }
            if (succ)
            {
                label4.ForeColor = Color.Green;
                label4.Text = "Açık";
            } else
            {
                label4.ForeColor = Color.Red;
                label4.Text = "Başarısız";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }
    }
}
