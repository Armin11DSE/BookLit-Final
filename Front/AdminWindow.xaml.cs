using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BookLit.Core;
using System.Data;
using System.Data.SqlClient;

namespace BookLit.Front
{
    public partial class AdminWindow : Window
    {
        public static Admin admin { get; set; }

        public AdminWindow(Admin _admin)
        {
            InitializeComponent();
            admin = _admin;
        }

        public void Search_Button_Click(object o, EventArgs e)
        {
            SqlConnection con = new(App.dataBaseAddress);
            string command;
            SqlDataAdapter adapter;
            DataTable data = new();

            if (searchBox.Text == "")
            {
                return;
            }

            int index = searchBox.Text.IndexOf(" by ");
            if (index == -1)
            {
                index = searchBox.Text.IndexOf('@');

                if (index == -1)
                {
                    string[] name = searchBox.Text.Split(' ');
                    if (name.Length != 2)
                    {
                        return;
                    }

                    command = "Select * from Users Where firstname = '" + name[0] + "' and lastname = '" + name[1] + "'";
                    adapter = new(command, con);
                    adapter.Fill(data);

                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        new PurchasesWindow(data.Rows[i][0].ToString()).Show();
                    }

                    return;
                }

                command = "Select * from Users Where email = '" + searchBox.Text + "'";
                adapter = new(command, con);
                adapter.Fill(data);

                new PurchasesWindow(data.Rows[0][0].ToString()).Show();
                return;
            }

            string title = searchBox.Text[..(index - 1)];
            string writer = searchBox.Text[(index + 4)..];

            command = "Select * from Books Where title = '" + title + "' and writer = '" + writer + "'";
            adapter = new(command, con);
            adapter.Fill(data);

            if (data.Rows.Count == 0)
            {
                MessageBox.Show("No such book exists!");
                return;
            }

            new BookWindow(title, writer);
        }

        public void Exit_Button_Click(object o, EventArgs e)
        {
            new StartingWindow().Show();
            this.Close();
        }
    }
}
