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
using System.Globalization;

namespace BookLit.Front
{
    public partial class BookWindow : Window
    {
        Book? book;
        AudioBook? audioBook;
        
        public BookWindow(string title, string writer)
        {
            InitializeComponent();

            SqlConnection con = new(App.dataBaseAddress);
            string command = "Select * from Books Where title = '" + title + "' and writer = '" + writer + "'";
            SqlDataAdapter adapter = new(command, con);
            DataTable data = new();
            adapter.Fill(data);

            var row = data.Rows[0];
            if (row[8] != null)
            {
                audioBook = new(row[1].ToString(), row[2].ToString(), (int)row[3], (double)row[4], row[5].ToString(), row[6].ToString(), row[7].ToString(), row[8].ToString(), (double)row[9], (int)row[10], (bool)row[11], (double?)row[12], (string?)row[13]);

            }
            else
            {
                book = new(row[1].ToString(), row[2].ToString(), (int)row[3], (double)row[4], row[5].ToString(), row[6].ToString(), row[7].ToString(), (double)row[9], (int)row[10], (bool)row[11], (double?)row[12], (string?)row[13]);
            }

            if ((book != null && book.IsVip)
                || (audioBook != null && audioBook.IsVip))
            {
                VipButton.IsChecked = true;
            }
            else
            {
                VipButton.IsChecked = false;
            }
        }

        public void Update_Button_Click(object o, EventArgs e)
        {
            if (PriceBox.Text != "" && !double.TryParse(PriceBox.Text, out double result))
            {
                MessageBox.Show("Invalid new price!");
                PriceBox.Text = "";
                return;
            }

            if (DiscountBox.Text != "" && !double.TryParse(DiscountBox.Text, out double amount))
            {
                MessageBox.Show("Invalid discount!");
                DiscountBox.Text = "";
                return;
            }

            if (DiscountBox.Text != "" && double.TryParse(DiscountBox.Text, out double discount))
            {
                if (book != null)
                {
                    if (discount > book.Price)
                    {
                        MessageBox.Show("Discount can't be more than the book's price!");
                        DiscountBox.Text = "";
                        return;
                    }
                    else if (book.DiscountExpirationDate != null)
                    {
                        MessageBox.Show("There is already a discount on this book!");
                        DiscountBox.Text = "";
                        discountExpirationBox.Text = "";
                        return;
                    }
                }
                else if (audioBook != null)
                {
                    if (audioBook.Price < discount)
                    {
                        MessageBox.Show("Discount can't be more than the book's price!");
                        DiscountBox.Text = "";
                        return;
                    }
                    else if (audioBook.DiscountExpirationDate != null)
                    {
                        MessageBox.Show("There is already a discount on this book!");
                        DiscountBox.Text = "";
                        discountExpirationBox.Text = "";
                        return;
                    }
                }
            }

            if (discountExpirationBox.Text != "")
            {
                PersianCalendar persianCalender = new();
                try
                {
                    if (int.Parse(discountExpirationBox.Text[..4]) < persianCalender.GetYear(DateTime.Now) 
                        || (int.Parse(discountExpirationBox.Text[..4]) == persianCalender.GetYear(DateTime.Now) 
                        && int.Parse(discountExpirationBox.Text[5..7]) < persianCalender.GetYear(DateTime.Now)) 
                        || (int.Parse(discountExpirationBox.Text[..4]) == persianCalender.GetYear(DateTime.Now)
                        && int.Parse(discountExpirationBox.Text[5..7]) == persianCalender.GetMonth(DateTime.Now) 
                        && int.Parse(discountExpirationBox.Text[^2..]) < persianCalender.GetDayOfMonth(DateTime.Now)))
                    {
                        MessageBox.Show("Invalid expiration date!");
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show("Invalid expiration date!");
                    discountExpirationBox.Text = "";
                    return;
                }
            }

            if (book != null)
            {
                book.Update(PriceBox.Text, DiscountBox.Text, discountExpirationBox.Text, (bool)VipButton.IsChecked);
            }
            else if (audioBook != null)
            {
                audioBook.Update(PriceBox.Text, DiscountBox.Text, discountExpirationBox.Text, (bool)VipButton.IsChecked);
            }

            PriceBox.Text = "";
            DiscountBox.Text = "";
            discountExpirationBox.Text = "";
            MessageBox.Show("Book's info was successfully changed.");
        }

        public void Delete_Button_Click(object o, EventArgs e)
        {
            SqlConnection con = new(App.dataBaseAddress);
            con.Open();
            string command;
            if (book != null)
            {
                command = "Delete from Books Where title = '" + book.title + "' and writer = '" + book.writer + "'";
            }
            else
            {
                command = "Delete from Books Where title = '" + audioBook.title + "' and writer = '" + audioBook.writer + "'";
            }

            SqlCommand cmd = new(command, con);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Book was successfully deleted.");
            this.Close();
        }

        public void Vip_Button_Click(object o, EventArgs e)
        {

        }

        public void Return_Button_Click(object o, EventArgs e) => this.Close();
    }
}
