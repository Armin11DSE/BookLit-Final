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
using BookLit.MVVM.ViewModel;
using BookLit.Core;
using System.Data;
using System.Data.SqlClient;
using BookLit;

namespace BookLit.Front
{
    public partial class StartingWindow : Window
    {
        public StartingWindow()
        {
            InitializeComponent();

            SqlConnection con = new(App.dataBaseAddress);
            string command = "Select * from App where id = '" + 1 + "'";
            SqlDataAdapter adapter = new(command, con);
            DataTable data = new();
            adapter.Fill(data);
            App.Income = (double)data.Rows[0][1];
            App.VipCostPerMonth = (double)data.Rows[0][2];
        }

        public void UserSignUp_Click(object o, EventArgs e)
        {
            StartingGrid.Visibility = Visibility.Hidden;
            UserSignUpGrid.Visibility = Visibility.Visible;
        }

        public void AdminSignIn_Click(object o, EventArgs e)
        {
            StartingGrid.Visibility = Visibility.Hidden;
            AdminSignInGrid.Visibility = Visibility.Visible;
        }

        public void UserSignIn_Click(object o, EventArgs e)
        {
            StartingGrid.Visibility = Visibility.Hidden;
            UserSignInGrid.Visibility = Visibility.Visible;
        }

        public void UserSignUp_Button_Click(object o, EventArgs e)
        {
            if (!UserFirstnameBox.Text.IsName())
            {
                MessageBox.Show("Invalid firstname!");
                UserFirstnameBox.Text = "";
                return;
            }

            if (!UserLastnameBox.Text.IsName())
            {
                MessageBox.Show("Invalid lastname!");
                UserLastnameBox.Text = "";
                return;
            }

            if (!UserPhoneNumberBox.Text.IsPhoneNumber())
            {
                MessageBox.Show("Invalid phone number!");
                UserPhoneNumberBox.Text = "";
                return;
            }

            if (!UserEmailBox_SU.Text.IsEmail())
            {
                MessageBox.Show("Invalid email address!");
                UserEmailBox_SU.Text = "";
                return;
            }

            if (!UserPasswordBox_SU.Password.IsPassword())
            {
                MessageBox.Show("Invalid password!");
                UserPasswordBox_SU.Password = "";
                return;
            }

            SqlConnection con = new(App.dataBaseAddress);
            con.Open();

            string command = "Select * from Users Where email = '" + UserEmailBox_SU.Text.Trim() + "'";
            SqlDataAdapter adapter = new(command, con);
            DataTable data = new();
            adapter.Fill(data);

            if (data.Rows.Count != 0)
            {
                MessageBox.Show("User with the same email address already exists!");
                UserEmailBox_SU.Text = "";
                UserPasswordBox_SU.Password = "";
                con.Close();
                return;
            }

            command = "Insert into Users values('" + UserEmailBox_SU.Text.Trim() + "','" + UserFirstnameBox.Text.Trim() + "','" + UserLastnameBox.Text.Trim() + "','" + UserPhoneNumberBox.Text.Trim() + "','" + UserPasswordBox_SU.Password.Trim() + "','" + 0 + "','" + 0 + "','" + null + "')";
            SqlCommand cmd = new(command, con);
            cmd.BeginExecuteNonQuery();
            new UserWindow(new User(UserEmailBox_SU.Text, UserFirstnameBox.Text, UserLastnameBox.Text, UserPhoneNumberBox.Text, UserPasswordBox_SU.Password, new List<Book>(), new List<Book>(), new List<Book>())).Show();

            UserFirstnameBox.Text = "";
            UserLastnameBox.Text = "";
            UserPhoneNumberBox.Text = "";
            UserEmailBox_SU.Text = "";
            UserPasswordBox_SU.Password = "";
            this.Close();
            con.Close();
        }

        public void AdminSignIn_Button_Click(object o, EventArgs e)
        {
            SqlConnection con = new(App.dataBaseAddress);
            con.Open();
            string command = "Select * From Admins Where email = '" + AdminEmailBox.Text.Trim() + "' and password = '" + AdminPasswordBox.Password.Trim() + "'";
            SqlDataAdapter adapter = new(command, con);
            DataTable data = new();
            adapter.Fill(data);

            try
            {
                new AdminWindow(new Admin(data.Rows[0][0].ToString(), data.Rows[0][1].ToString().Trim())).Show();
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("No such admin exists!\nOr the entered password is wrong!");
            }
            finally
            {
                AdminEmailBox.Text = "";
                AdminPasswordBox.Password = "";
                con.Close();
            }
        }

        public void UserSignIn_Button_Click(object o, EventArgs e)
        {
            SqlConnection con = new(App.dataBaseAddress);
            con.Open();
            string command = "Select * From Users Where email = '" + UserEmailBox_SI.Text.Trim() + "' and password = '" + UserPasswordBox_SI.Password.Trim() + "'";
            SqlDataAdapter adapter = new(command, con);
            DataTable data = new();
            adapter.Fill(data);

            try
            {
                var row = data.Rows[0];
                SqlDataAdapter bookAdapter;
                DataTable bookData;

                command = "Select * From Cart Where userEmail = '" + row[0] + "'";
                SqlDataAdapter cartAdapter = new(command, con);
                DataTable cartData = new();
                cartAdapter.Fill(cartData);

                command = "Select * From Purchases Where userEmail = '" + row[0] + "'";
                SqlDataAdapter purchaseAdapter = new(command, con);
                DataTable purchaseData = new();
                purchaseAdapter.Fill(purchaseData);

                command = "Select * From Bookmarks Where userEmail = '" + row[0] + "'";
                SqlDataAdapter markAdapter = new(command, con);
                DataTable markData = new();
                markAdapter.Fill(markData);

                List<Book> cart = new();
                List<Book> purchases = new();
                List<Book> bookmarks = new();

                for (int i = 0; i < cartData.Rows.Count; i++)
                {
                    command = "Select * From Books Where title = '" + cartData.Rows[i][2].ToString() + "' and writer = '" + cartData.Rows[i][3].ToString() + "'";
                    bookAdapter = new(command, con);
                    bookData = new();
                    bookAdapter.Fill(bookData);

                    var Row = bookData.Rows[0];

                    if (Row[8].ToString() != "")
                    {
                        cart.Add(new AudioBook(Row[1].ToString(), Row[2].ToString(), (int)Row[3], (double)Row[4], Row[5].ToString(), Row[6].ToString(), Row[7].ToString(), Row[8].ToString(), (double)Row[9], (int)Row[10], (bool)Row[11], (double)Row[12], Row[13].ToString()));
                    }
                    else
                    {
                        cart.Add(new Book(Row[1].ToString(), Row[2].ToString(), (int)Row[3], (double)Row[4], Row[5].ToString(), Row[6].ToString(), Row[7].ToString(), (double)Row[9], (int)Row[10], (bool)Row[11], (double)Row[12], Row[13].ToString()));
                    }
                }

                for (int i = 0; i < markData.Rows.Count; i++)
                {
                    command = "Select * From Books Where title = '" + markData.Rows[i][2].ToString() + "' and writer = '" + markData.Rows[i][3].ToString() + "'";
                    bookAdapter = new(command, con);
                    bookData = new();
                    bookAdapter.Fill(bookData);

                    var Row = bookData.Rows[0];

                    if (Row[8].ToString() != "")
                    {
                        bookmarks.Add(new AudioBook(Row[1].ToString(), Row[2].ToString(), (int)Row[3], (double)Row[4], Row[5].ToString(), Row[6].ToString(), Row[7].ToString(), Row[8].ToString(), (double)Row[9], (int)Row[10], (bool)Row[11], (double)Row[12], Row[13].ToString()));
                    }
                    else
                    {
                        bookmarks.Add(new Book(Row[1].ToString(), Row[2].ToString(), (int)Row[3], (double)Row[4], Row[5].ToString(), Row[6].ToString(), Row[7].ToString(), (double)Row[9], (int)Row[10], (bool)Row[11], (double)Row[12], Row[13].ToString()));
                    }
                }

                for (int i = 0; i < purchaseData.Rows.Count; i++)
                {
                    command = "Select * From Books Where title = '" + purchaseData.Rows[i][2].ToString() + "' and writer = '" + purchaseData.Rows[i][3].ToString() + "'";
                    bookData = new();
                    bookAdapter = new(command, con);
                    bookAdapter.Fill(bookData);

                    var Row = bookData.Rows[0];

                    if (Row[8].ToString() != "")
                    {
                        purchases.Add(new AudioBook(Row[1].ToString(), Row[2].ToString(), (int)Row[3], (double)Row[4], Row[5].ToString(), Row[6].ToString(), Row[7].ToString(), Row[8].ToString(), (double)Row[9], (int)Row[10], (bool)Row[11], (double)Row[12], Row[13].ToString()));
                    }
                    else
                    {
                        purchases.Add(new Book(Row[1].ToString(), Row[2].ToString(), (int)Row[3], (double)Row[4], Row[5].ToString(), Row[6].ToString(), Row[7].ToString(), (double)Row[9], (int)Row[10], (bool)Row[11], (double)Row[12], Row[13].ToString()));
                    }
                }

                new UserWindow(new User(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString().Trim(), purchases, bookmarks, cart, (double)row[5], (bool)row[6], row[7].ToString())).Show();
                this.Close();
            }
            catch (Exception x)
            {
                throw x;
                MessageBox.Show("No such user exists!\nOr the entered password is wrong!");
            }
            finally
            {
                UserEmailBox_SI.Text = "";
                UserPasswordBox_SI.Password = "";
                con.Close();
            }
        }

        public void Return_Button_Click(object o, EventArgs e)
        {
            UserSignUpGrid.Visibility = Visibility.Hidden;
            AdminSignInGrid.Visibility = Visibility.Hidden;
            UserSignInGrid.Visibility = Visibility.Hidden;
            AdminEmailBox.Text = "";
            AdminPasswordBox.Password = "";
            UserFirstnameBox.Text = "";
            UserLastnameBox.Text = "";
            UserPhoneNumberBox.Text = "";
            UserEmailBox_SU.Text = "";
            UserPasswordBox_SU.Password = "";
            UserEmailBox_SI.Text = "";
            UserPasswordBox_SI.Password = "";
            StartingGrid.Visibility = Visibility.Visible;
        }

        public void Exit_Button_Click(object o, EventArgs e) => this.Close();
    }
}
