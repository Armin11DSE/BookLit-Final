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
using BookLit.Core;
using BookLit.Front;

namespace BookLit.MVVM.View
{
    public partial class UserHomeView : UserControl
    {
        List<Book> books = new();

        public UserHomeView()
        {
            InitializeComponent();
            SqlConnection con = new(App.dataBaseAddress);
            string command = "Select * from Books";
            SqlDataAdapter adapter = new(command, con);
            DataTable data = new();
            adapter.Fill(data);

            int[] rands = new int[3];
            int counter = -1;

            if (data.Rows.Count > 3)
            {
                while (counter < 2)
                {
                    int ran = new Random().Next(data.Rows.Count);

                    bool reset = false;
                    for (int i = counter; i >= 0; i++)
                    {
                        if (rands[i] == ran)
                        {
                            reset = true;
                            break;
                        }
                    }

                    if (reset)
                    {
                        continue;
                    }

                    rands[++counter] = ran;
                }
            }
            else
            {
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    if (!IsDuplicate(data.Rows[i][1].ToString(), data.Rows[i][2].ToString()))
                    {
                        rands[++counter] = i;
                    }
                }
            }

            for (int i = 0; i <= counter && counter >= 0; i++)
            {
                if (data.Rows[rands[i]][8].ToString() != "")
                {
                    books.Add(new AudioBook(data.Rows[rands[i]][1].ToString(), data.Rows[rands[i]][2].ToString(), (int)data.Rows[rands[i]][3], (double)data.Rows[rands[i]][4], data.Rows[rands[i]][5].ToString(), data.Rows[rands[i]][6].ToString(), data.Rows[rands[i]][7].ToString(), data.Rows[rands[i]][8].ToString(), (double)data.Rows[rands[i]][9], (int)data.Rows[rands[i]][10], (bool)data.Rows[rands[i]][11], (double)data.Rows[rands[i]][12], data.Rows[rands[i]][13].ToString()));
                }
                else
                {
                    books.Add(new Book(data.Rows[rands[i]][1].ToString(), data.Rows[rands[i]][2].ToString(), (int)data.Rows[rands[i]][3], (double)data.Rows[rands[i]][4], data.Rows[rands[i]][5].ToString(), data.Rows[rands[i]][6].ToString(), data.Rows[rands[i]][7].ToString(), (double)data.Rows[rands[i]][9], (int)data.Rows[rands[i]][10], (bool)data.Rows[rands[i]][11], (double)data.Rows[rands[i]][12], data.Rows[rands[i]][13].ToString()));
                }

                Border border = new();
                border.BorderThickness = new(1);
                border.Margin = new Thickness(0, 0, 5, 0);
                border.BorderBrush = Brushes.Yellow;
                border.Width = 150;
                border.Height = 250;

                StackPanel bookPanel = new();

                Image cover = new();
                cover.Stretch = Stretch.Uniform;
                string path = Environment.CurrentDirectory[..Environment.CurrentDirectory.IndexOf("bin")];
                Uri resourceUri = new Uri($"{path}Books/Cover/{data.Rows[rands[i]][7].ToString()}");
                cover.Source = new BitmapImage(resourceUri);
                cover.Height = 100;
                cover.Width = 150;
                bookPanel.Children.Add(cover);

                TextBlock text = new();
                text.Text = data.Rows[rands[i]][1].ToString();
                text.HorizontalAlignment = HorizontalAlignment.Center;
                text.VerticalAlignment = VerticalAlignment.Top;
                text.Background = Brushes.Transparent;
                text.Foreground = Brushes.White;
                text.Margin = new Thickness(0, 2, 0, 0);
                bookPanel.Children.Add(text);

                text = new();
                text.HorizontalAlignment = HorizontalAlignment.Center;
                text.VerticalAlignment = VerticalAlignment.Top;
                text.Background = Brushes.Transparent;
                text.Foreground = Brushes.White;
                text.Margin = new Thickness(0, 2, 0, 0);
                text.Text = "by";
                bookPanel.Children.Add(text);

                text = new();
                text.HorizontalAlignment = HorizontalAlignment.Center;
                text.VerticalAlignment = VerticalAlignment.Top;
                text.Background = Brushes.Transparent;
                text.Foreground = Brushes.White;
                text.Margin = new Thickness(0, 2, 0, 0);
                text.Text = data.Rows[rands[i]][2].ToString();
                bookPanel.Children.Add(text);

                text = new();
                text.HorizontalAlignment = HorizontalAlignment.Left;
                text.VerticalAlignment = VerticalAlignment.Top;
                text.Background = Brushes.Transparent;
                text.Foreground = Brushes.White;
                text.Margin = new Thickness(0, 5, 0, 0);
                text.Text = "  rating: " + data.Rows[rands[i]][9].ToString() + "      by      " + data.Rows[rands[i]][10].ToString();
                bookPanel.Children.Add(text);

                text = new();
                text.HorizontalAlignment = HorizontalAlignment.Left;
                text.VerticalAlignment = VerticalAlignment.Top;
                text.Background = Brushes.Transparent;
                text.Foreground = Brushes.White;
                text.Margin = new Thickness(0, 2, 0, 0);
                text.Text = "Release year: " + data.Rows[rands[i]][3].ToString();
                bookPanel.Children.Add(text);

                text = new();
                text.HorizontalAlignment = HorizontalAlignment.Left;
                text.VerticalAlignment = VerticalAlignment.Top;
                text.Background = Brushes.Transparent;
                text.Foreground = Brushes.White;
                text.Margin = new Thickness(0, 2, 0, 0);
                text.Text = "Price: " + data.Rows[rands[i]][4].ToString() + " - " + ((double)data.Rows[rands[i]][12]).ToString();
                bookPanel.Children.Add(text);

                if ((bool)data.Rows[rands[i]][11])
                {
                    text = new();
                    text.HorizontalAlignment = HorizontalAlignment.Center;
                    text.VerticalAlignment = VerticalAlignment.Top;
                    text.Background = Brushes.Transparent;
                    text.Foreground = Brushes.DarkRed;
                    text.Margin = new Thickness(0, 2, 0, 0);
                    text.Text = "Vip";
                    bookPanel.Children.Add(text);
                }

                StackPanel sp = new();
                sp.Orientation = Orientation.Horizontal;
                sp.Margin = new Thickness(0, 5, 0, 0);

                Button sumButton = new();
                sumButton.Click += Sum_Button_Click;
                sumButton.BorderThickness = new(0);
                sumButton.Content = "Summary";
                sumButton.Foreground = Brushes.BlueViolet;
                sumButton.Background = Brushes.Transparent;
                sumButton.VerticalAlignment = VerticalAlignment.Bottom;
                sumButton.Margin = new Thickness(0, 0, 5, 0);

                Button markButton = new();
                markButton.Click += Bookmark_Button_Click;
                markButton.VerticalAlignment = VerticalAlignment.Bottom;
                markButton.BorderThickness = new(0);
                markButton.Content = "Bookmark";
                markButton.Foreground = Brushes.BlueViolet;
                markButton.Background = Brushes.Transparent;
                markButton.Margin = new Thickness(0, 0, 5, 0);

                Button addButton = new();
                addButton.Click += Add_Button_Click;
                addButton.BorderThickness = new(0);
                addButton.Content = "Add";
                addButton.Foreground = Brushes.BlueViolet;
                addButton.VerticalAlignment = VerticalAlignment.Bottom;
                addButton.Background = Brushes.Transparent;

                sp.Children.Add(sumButton);
                sp.Children.Add(markButton);
                sp.Children.Add(addButton);
                bookPanel.Children.Add(sp);

                border.Child = bookPanel;
                HomePanel.Children.Add(border);
            }
        }

        private bool IsDuplicate(string title, string writer)
        {
            foreach (Book book in UserWindow.user.Books)
            {
                if (book.title == title && book.writer == writer)
                {
                    return true;
                }
            }

            return false;
        }

        public void Add_Button_Click(object o, EventArgs e)
        {
            StackPanel stackPanel = (StackPanel)((StackPanel)((Button)o).Parent).Parent;
            Book book = books.Where(s => s.title == ((TextBlock)stackPanel.Children[1]).Text && s.writer == ((TextBlock)stackPanel.Children[3]).Text).First();
            if (book.IsVip && !UserWindow.user.IsVip)
            {
                MessageBox.Show("You must have vip membership to access this book!");
                return;
            }

            foreach (Book _book in UserWindow.user.Cart)
            {
                if (_book.title == book.title && _book.writer == book.writer)
                {
                    MessageBox.Show("You have already added this book to your cart!");
                    return;
                }
            }

            UserWindow.user.AddToCart(book);
            MessageBox.Show("Book was successfully added to your cart.");
        }

        public void Sum_Button_Click(object o, EventArgs e)
        {
            StackPanel stackPanel = (StackPanel)((StackPanel)((Button)o).Parent).Parent;
            Book book = books.Where(s => s.title == ((TextBlock)stackPanel.Children[1]).Text && s.writer == ((TextBlock)stackPanel.Children[3]).Text).First();
            MessageBox.Show(book.Summary);
        }

        public void Bookmark_Button_Click(object o, EventArgs e)
        {
            StackPanel stackPanel = (StackPanel)((StackPanel)((Button)o).Parent).Parent;
            Book book = books.Where(s => s.title == ((TextBlock)stackPanel.Children[1]).Text && s.writer == ((TextBlock)stackPanel.Children[3]).Text).First();
            foreach (Book _book in UserWindow.user.Bookmarks)
            {
                if (_book.title == book.title && _book.writer == book.writer)
                {
                    MessageBox.Show("Book has already been bookmarked!");
                    return;
                }
            }
            UserWindow.user.AddToBookmarks(book);
            MessageBox.Show("Book was successfully bookmarked.");
        }
    }
}
