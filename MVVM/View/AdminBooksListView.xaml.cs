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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BookLit.Core;
using System.Data.SqlClient;
using System.Data;
using BookLit.Front;

namespace BookLit.MVVM.View
{
    public partial class AdminBooksListView : UserControl
    {
        public AdminBooksListView()
        {
            InitializeComponent();

            SqlConnection con = new(App.dataBaseAddress);
            string command = "Select * from Books";
            SqlDataAdapter adapter = new(command, con);
            DataTable data = new();
            adapter.Fill(data);

            string sellsCommand;
            SqlDataAdapter sellsAdapter;
            DataTable sellsData;

            for (int i = 0; i < data.Rows.Count; i++)
            {
                Button button = new();
                button.Foreground = Brushes.BlueViolet;
                button.Width = 650;
                button.Height = 30;
                button.HorizontalContentAlignment = HorizontalAlignment.Left;
                button.VerticalContentAlignment = VerticalAlignment.Center;
                button.FontSize = 15;
                button.BorderThickness = new(0);
                button.Background = Brushes.Transparent;
                button.Content = (i + 1).ToString() + "- " + data.Rows[i][1].ToString() + "    by    " + data.Rows[i][2].ToString() + "\t\trating: " + Math.Round((double)data.Rows[i][9], 1) + " by " + (int)data.Rows[i][10] + "\tsells: ";
                button.Click += bookClick;

                sellsCommand = "Select * from Purchases Where bookTitle = '" + data.Rows[i][1] + "' and bookWriter = '" + data.Rows[i][2] + "'";
                sellsAdapter = new(sellsCommand, con);
                sellsData = new();
                sellsAdapter.Fill(sellsData);
                button.Content += sellsData.Rows.Count.ToString();

                BooksListPanel.Children.Add(button);
            }
        }

        public void bookClick(object o, EventArgs e)
        {
            string title, writer;

            Button button = (Button)o;
            int index = button.Content.ToString().IndexOf("    by    ");
            title = button.Content.ToString()[(button.Content.ToString().IndexOf('-') + 2)..index];
            writer = button.Content.ToString()[(index + 10)..];
            writer = writer[..writer.IndexOf('\t')];
            new BookWindow(title, writer).Show();
        }
    }
}
