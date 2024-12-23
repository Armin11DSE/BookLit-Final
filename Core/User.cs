using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows;

namespace BookLit.Core
{
    public class User
    {
        public readonly string Email;
        private string Password;

        public string Firstname { get; private set; }
        public string Lastname { get; private set; }
        public string PhoneNumber { get; private set; }
        public double Wallet { get; private set; }
        private bool _IsVip;
        public bool IsVip
        {
            get => _IsVip;
            set
            {
                _IsVip = value;
                SqlConnection con = new(App.dataBaseAddress);
                con.Open();
                string command = "Update Users SET isVip = '" + this._IsVip + "' Where email = '" + this.Email + "'";
                SqlCommand cmd = new(command, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        private string? _VipExpirationDate; 
        public string? VipExpirationDate
        {
            get => _VipExpirationDate;
            set
            {
                _VipExpirationDate = value;
                SqlConnection con = new(App.dataBaseAddress);
                con.Open();
                string command = "Update Users SET vipExpirationDate = '" + this._VipExpirationDate + "' Where email = '" + this.Email + "'";
                SqlCommand cmd = new(command, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public List<Book> Books;
        public List<Book> Cart;
        public List<Book> Bookmarks;

        public User(string email, string firstname, string lastname, string phoneNumber, string password, List<Book> Books, List<Book> Bookmarks, List<Book> Cart,double wallet = 0,  bool isVip = false, string? vipExpirationDate = null)
        {
            this.Firstname = firstname;
            this.Lastname = lastname;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.Password = password;
            this.Wallet = wallet;
            this.Cart = Cart;
            this.Books = Books;
            this.Bookmarks = Bookmarks;
            this._IsVip = isVip;
            this._VipExpirationDate = vipExpirationDate;
            if (VipExpirationDate != null && VipExpirationDate != "")
            {
                PersianCalendar persianCalender = new();
                if (int.Parse(VipExpirationDate[..4]) >= persianCalender.GetYear(DateTime.Now) && int.Parse(VipExpirationDate[5..7]) >= persianCalender.GetMonth(DateTime.Now) && int.Parse(VipExpirationDate[^2..]) > persianCalender.GetDayOfMonth(DateTime.Now))
                {
                    this.IsVip = false;
                    this.VipExpirationDate = null;
                }
            }
        }

        public void UpdateProfile(string firstname, string lastname, string phoneNumber, string password)
        {
            if (firstname != "")
            {
                this.Firstname = firstname;
            }

            if (lastname != "")
            {
                this.Lastname = lastname;
            }

            if (phoneNumber != "")
            {
                this.PhoneNumber = phoneNumber;
            }

            if (password != "")
            {
                this.Password = password;
            }

            SqlConnection con = new(App.dataBaseAddress);
            con.Open();
            string command = "Update Users SET firstname = '" + this.Firstname + "', lastname = '" + this.Lastname + "', phoneNumber = '" + this.PhoneNumber + "', password = '" + this.Password + "' Where email = '" + this.Email + "'";
            SqlCommand cmd = new(command, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void AddMoney(double amount)
        {
            this.Wallet += amount;
            SqlConnection con = new(App.dataBaseAddress);
            con.Open();
            string command = "Update Users SET Wallet = '" + this.Wallet + "' Where email = '" + this.Email + "'";
            SqlCommand cmd = new(command, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void TakeMoney(double amount)
        {
            this.Wallet -= amount;
            SqlConnection con = new(App.dataBaseAddress);
            con.Open();
            string command = "Update Users SET Wallet = '" + this.Wallet + "' Where email = '" + this.Email + "'";
            SqlCommand cmd = new(command, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public bool IsPasswordCorrect(string password) => this.Password == password;

        public void AddToCart(Book book)
        {
            Cart.Add(book);
            SqlConnection con = new(App.dataBaseAddress);
            con.Open();
            string command = "Insert into Cart values('" + Email + "','" + book.title + "','" + book.writer + "')";
            SqlCommand cmd = new(command, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void AddToBooks(Book book)
        {
            Books.Add(book);
            SqlConnection con = new(App.dataBaseAddress);
            con.Open();
            string command = "Insert into Purchases values('" + Email + "','" + book.title + "','" + book.writer + "')";
            SqlCommand cmd = new(command, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void AddToBookmarks(Book book)
        {
            Bookmarks.Add(book);
            SqlConnection con = new(App.dataBaseAddress);
            con.Open();
            string command = "Insert into Bookmarks values('" + Email + "','" + book.title + "','" + book.writer + "')";
            SqlCommand cmd = new(command, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void RemoveFromCart(Book book)
        {
            for (int i = 0; i < Cart.Count; i++)
            {
                if (Cart[i].title == book.title && Cart[i].writer == book.writer)
                {
                    Cart.RemoveAt(i);
                    break;
                }
            }

            SqlConnection con = new(App.dataBaseAddress);
            con.Open();
            string command = "Delete from Cart Where userEmail = '" + Email + "' and bookTitle = '" + book.title + "' and bookWriter = '" + book.writer + "'";
            SqlCommand cmd = new(command, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void RemoveFromBookmarks(Book book)
        {
            for (int i = 0; i < Bookmarks.Count; i++)
            {
                if (Bookmarks[i].title == book.title && Bookmarks[i].writer == book.writer)
                {
                    Bookmarks.RemoveAt(i);
                    break;
                }
            }

            SqlConnection con = new(App.dataBaseAddress);
            con.Open();
            string command = "Delete from Bookmarks Where userEmail = '" + Email + "' and bookTitle = '" + book.title + "' and bookWriter = '" + book.writer + "'";
            SqlCommand cmd = new(command, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}