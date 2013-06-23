using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MySql.Data;
using MySql.Data.MySqlClient;


namespace SerialComm
{
    public partial class Form1 : Form
    {
        string RxString;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            serialPort1.PortName = "COM3";
            serialPort1.BaudRate = 9600;

            serialPort1.Open();
            if (serialPort1.IsOpen)
            {
                btnStart.Enabled = false;
                btnStop.Enabled = true;
                btnStart.Enabled = false;

                chkFeed.Enabled = true; //피드 체크 타이머 가동
            }

        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            RxString = serialPort1.ReadExisting();
            this.Invoke(new EventHandler(DisplayText));
        }

        private void DisplayText(object sender, EventArgs e)
        {
            textBox1.AppendText(RxString);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                textBox1.ReadOnly = true;

                chkFeed.Enabled = false; //피드 체크 타이머 중지
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!serialPort1.IsOpen) return;
            char[] buff = new char[1];
            buff[0] = e.KeyChar;
            if (buffDB == "1")
                buff[0] = '1';

            // Send the one character buffer.
            serialPort1.Write(buff, 0, 1);

            e.Handled = true;
        }

        string buffDB = "";
        private void dbOpen_Click(object sender, EventArgs e)
        {
            string connStr =
             "server=14.63.164.98;user=farm;database=farm;port=3306;password=farm#55;";
            MySqlConnection conn = new MySqlConnection(connStr);
        try
        {
            Console.WriteLine("Connecting to MySQL...");
            conn.Open();
            // Perform database operations
            string sql = "SELECT * FROM feedwater";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Console.WriteLine(rdr[0] + " -- " + rdr[1]);
            }
            
            buffDB = rdr[1].ToString();
            rdr.Close();

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        conn.Close();
        Console.WriteLine("Done.");
        }

        public char KeyChar { get; set; }

        private void getDB()
        {
            string connStr =
           "server=14.63.164.98;user=farm;database=farm;port=3306;password=farm#55;";
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                // Perform database operations
                string sql = "SELECT * FROM feedwater";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine(rdr[0] + " -- " + rdr[1]);
                }

                buffDB = rdr[1].ToString();
                rdr.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");
        }

        public void setSerial()
        {
            if (!serialPort1.IsOpen) return;
            char[] buff = new char[1];
            if (buffDB == "1")
                buff[0] = '1';
            else
            {
                buff[0] = '0';
            }
            // Send the one character buffer.
            serialPort1.Write(buff, 0, 1);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //데이타베이스 셋팅
            getDB();
            //시리얼 데이타 보내기
            setSerial();
        }
    }
}
 