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
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace BookLit.MVVM.View
{
    public partial class AdminBookAditionView : UserControl
    {
        public AdminBookAditionView() => InitializeComponent();

        public void AddBook_Button_Click(object o, EventArgs e)
        {
            if (BookTitleBox.Text == "" || WriterNameBox.Text == "" || ReleaseYearBox.Text == "" || PriceBox.Text == "" || FileNameBox.Text == "" || CoverNameBox.Text == "" || SummaryBox.Text == "")
            {
                MessageBox.Show("You must fill all the fields!");
                return;
            }

            if (!int.TryParse(ReleaseYearBox.Text, out int year))
            {
                MessageBox.Show("You must enter a number as the release year!");
                ReleaseYearBox.Text = "";
                return;
            }

            if (int.Parse(ReleaseYearBox.Text) > new PersianCalendar().GetYear(DateTime.Now))
            {
                MessageBox.Show("Invalid release year!");
                ReleaseYearBox.Text = "";
                return;
            }

            if (!double.TryParse(PriceBox.Text, out double price))
            {
                MessageBox.Show("You must enter a decimal as the price!");
                PriceBox.Text = "";
                return;
            }

            if (!FileNameBox.Text.IsPdf())
            {
                MessageBox.Show($"{FileNameBox.Text} doesn't exist in the Books\\File");
                FileNameBox.Text = "";
                return;
            }

            if (!CoverNameBox.Text.IsImage())
            {
                MessageBox.Show($"{CoverNameBox.Text} doesn't exist in the Books\\Cover");
                CoverNameBox.Text = "";
                return;
            }

            if (AudioNameBox.Text != "" && !AudioNameBox.Text.IsAudio())
            {
                MessageBox.Show($"{AudioNameBox.Text} doesn't exist in the Books\\Audio");
                AudioNameBox.Text = "";
                return;
            }

            SqlConnection con = new(App.dataBaseAddress);
            con.Open();

            string command = "Select * from Books Where title = '" + BookTitleBox.Text.Trim() + "' and writer = '" + WriterNameBox.Text.Trim() + "'";
            SqlDataAdapter adapter = new(command, con);
            DataTable data = new();
            adapter.Fill(data);

            if (data.Rows.Count != 0)
            {
                MessageBox.Show("The same book already exists!");
                BookTitleBox.Text = "";
                WriterNameBox.Text = "";
                FileNameBox.Text = "";
                CoverNameBox.Text = "";
                AudioNameBox.Text = "";
                ReleaseYearBox.Text = "";
                PriceBox.Text = "";
                SummaryBox.Text = "";
                con.Close();
                return;
            }

            command = "Insert into Books values('" +BookTitleBox.Text.Trim()+ "','" +WriterNameBox.Text.Trim()+ "','" +int.Parse(ReleaseYearBox.Text.Trim())+ "','" +double.Parse(PriceBox.Text.Trim())+ "','" +SummaryBox.Text.Trim()+ "','" +FileNameBox.Text.Trim()+ "','" +CoverNameBox.Text.Trim()+ "','" +AudioNameBox.Text.Trim()+ "','" +0+ "','" +0+ "','" +0+ "','" +null+ "','" +null+ "')";
            SqlCommand cmd = new(command, con);
            cmd.ExecuteNonQuery();
            con.Close();
            BookTitleBox.Text = "";
            WriterNameBox.Text = "";
            PriceBox.Text = "";
            FileNameBox.Text = "";
            CoverNameBox.Text = "";
            AudioNameBox.Text = "";
            SummaryBox.Text = "";
            ReleaseYearBox.Text = "";
            MessageBox.Show("Book was successfully added.");
        }
    }
}
