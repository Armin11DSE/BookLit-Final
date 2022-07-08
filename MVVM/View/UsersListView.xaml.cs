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
using System.Data;
using System.Data.SqlClient;
using BookLit.Front;

namespace BookLit.MVVM.View
{
    public partial class UsersListView : UserControl
    {
        public UsersListView()
        {
            InitializeComponent();

            SqlConnection con = new(App.dataBaseAddress);
            string command = "Select * from Users";
            SqlDataAdapter adapter = new(command, con);
            DataTable data = new();
            adapter.Fill(data);

            for (int i = 0; i < data.Rows.Count; i++)
            {
                Button button = new();
                button.Content = (i + 1).ToString() + "- ";
                button.Content += String.Format("{0,-35} {1,-35} {2,-35}", data.Rows[i][0].ToString(), data.Rows[i][1].ToString(), data.Rows[i][2].ToString());
                if ((bool)data.Rows[i][6])
                {
                    button.Content += "Vip";
                }

                button.Foreground = Brushes.BlueViolet;
                button.Width = 650;
                button.Height = 30;
                button.HorizontalContentAlignment = HorizontalAlignment.Left;
                button.VerticalContentAlignment = VerticalAlignment.Center;
                button.FontSize = 15;
                button.BorderThickness = new(0);
                button.Background = Brushes.Transparent;
                button.Click += userClick;

                AllUsersPanel.Children.Add(button);
            }
        }

        public void userClick(object o, EventArgs e)
        {
            Button button = (Button)o;
            int index = button.Content.ToString().IndexOf('-');
            new PurchasesWindow(button.Content.ToString()[(index + 2)..(index + 36)].Trim()).Show();
        }
    }
}
