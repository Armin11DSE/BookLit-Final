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
    public partial class PurchasesWindow : Window
    {
        public PurchasesWindow(string email)
        {
            InitializeComponent();

            SqlConnection con = new(App.dataBaseAddress);
            string command = "Select * from Purchases Where userEmail = '" + email + "'";
            SqlDataAdapter adapter = new(command, con);
            DataTable data = new();
            adapter.Fill(data);

            for (int i = 0; i < data.Rows.Count; i++)
            {
                TextBlock purchase = new();
                purchase.Height = 30;
                purchase.VerticalAlignment = VerticalAlignment.Center;
                purchase.HorizontalAlignment = HorizontalAlignment.Center;
                purchase.Width = 350;
                purchase.FontSize = 15;
                purchase.Text = $"{i + 1}- {data.Rows[i][2].ToString()}\tby\t{data.Rows[i][3].ToString()}";
                purchases.Children.Add(purchase);
            }
        }

        public void Exit_Button_Click(object o, EventArgs e)
        {
            this.Close();
        }
    }
}
