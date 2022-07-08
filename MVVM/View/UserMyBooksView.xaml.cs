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
using BookLit.Core;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;

namespace BookLit.MVVM.View
{
    public partial class UserMyBooksView : UserControl
    {
        List<Book> books = new();

        public UserMyBooksView()
        {
            InitializeComponent();

            books = UserWindow.user.Books;

            for (int i = 0; i < books.Count; i++)
            {
                Border border = new();
                border.BorderThickness = new(1);
                border.Margin = new Thickness(0, 0, 5, 0);
                border.BorderBrush = Brushes.Yellow;
                border.Width = 150;
                border.Height = 250;
                border.HorizontalAlignment = HorizontalAlignment.Left;

                StackPanel bookPanel = new();
                bookPanel.HorizontalAlignment = HorizontalAlignment.Left;

                Image cover = new();
                cover.Stretch = Stretch.Uniform;
                string path = Environment.CurrentDirectory[..Environment.CurrentDirectory.IndexOf("bin")];
                Uri resourceUri = new Uri($"{path}Books/Cover/{books[i].coverName}");
                cover.Source = new BitmapImage(resourceUri);
                cover.Height = 100;
                cover.Width = 150;
                bookPanel.Children.Add(cover);

                TextBlock text = new();
                text.Text = books[i].title;
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
                text.Text = books[i].writer;
                bookPanel.Children.Add(text);

                text = new();
                text.HorizontalAlignment = HorizontalAlignment.Left;
                text.VerticalAlignment = VerticalAlignment.Top;
                text.Background = Brushes.Transparent;
                text.Foreground = Brushes.White;
                text.Margin = new Thickness(0, 5, 0, 0);
                text.Text = "  rating: " + books[i].Rating.ToString() + "      by      " + books[i].RatingsNum.ToString();
                bookPanel.Children.Add(text);

                text = new();
                text.HorizontalAlignment = HorizontalAlignment.Left;
                text.VerticalAlignment = VerticalAlignment.Top;
                text.Background = Brushes.Transparent;
                text.Foreground = Brushes.White;
                text.Margin = new Thickness(0, 2, 0, 0);
                text.Text = "Release year: " + books[i].publishYear.ToString();
                bookPanel.Children.Add(text);

                StackPanel sp = new();
                sp.Orientation = Orientation.Horizontal;
                sp.Margin = new Thickness(0, 5, 0, 0);
                sp.HorizontalAlignment = HorizontalAlignment.Center;

                Button rateButton = new();
                rateButton.Click += Rate;
                rateButton.VerticalAlignment = VerticalAlignment.Bottom;
                rateButton.BorderThickness = new(0);
                rateButton.Content = "Rate";
                rateButton.Foreground = Brushes.BlueViolet;
                rateButton.Background = Brushes.Transparent;
                rateButton.Width = 50;

                Button readButton = new();
                readButton.Click += Read;
                readButton.BorderThickness = new(0);
                readButton.Content = "Read";
                readButton.Foreground = Brushes.BlueViolet;
                readButton.VerticalAlignment = VerticalAlignment.Bottom;
                readButton.Background = Brushes.Transparent;
                readButton.Width = 50;

                sp.Children.Add(rateButton);
                sp.Children.Add(readButton);

                if (books[i] is AudioBook)
                {
                    Button listenButton = new();
                    listenButton.Click += Listen;
                    listenButton.BorderThickness = new(0);
                    listenButton.Content = "Listen";
                    listenButton.Foreground = Brushes.BlueViolet;
                    listenButton.VerticalAlignment = VerticalAlignment.Bottom;
                    listenButton.Background = Brushes.Transparent;
                    sp.Children.Add(listenButton);
                    listenButton.Width = 50;
                }
                
                bookPanel.Children.Add(sp);
                border.Child = bookPanel;
                BooksPanel.Children.Add(border);
            }
        }

        public void Rate(object o, EventArgs e)
        {
            StackPanel stackPanel = (StackPanel)((StackPanel)((Button)o).Parent).Parent;
            Book book = books.Where(s => s.title == ((TextBlock)stackPanel.Children[1]).Text && s.writer == ((TextBlock)stackPanel.Children[3]).Text).First();

            SqlConnection con = new(App.dataBaseAddress);
            string command = "Select * from RatedBooks Where userEmail = '" + UserWindow.user.Email + "' and bookTitle = '" + book.title + "' and bookWriter = '" + book.writer + "'";
            SqlDataAdapter adapter = new(command, con);
            DataTable data = new();
            adapter.Fill(data);
            if (data.Rows.Count != 0)
            {
                MessageBox.Show("You have already rated this book!");
                return;
            }

            new RatingWindow(book).Show();
        }

        public void Read(object o, EventArgs e)
        {
            StackPanel stackPanel = (StackPanel)((StackPanel)((Button)o).Parent).Parent;
            Book book = books.Where(s => s.title == ((TextBlock)stackPanel.Children[1]).Text && s.writer == ((TextBlock)stackPanel.Children[3]).Text).First();
            string path = Environment.CurrentDirectory[..Environment.CurrentDirectory.IndexOf("bin")];
            Process p = new Process();
            p.StartInfo.UseShellExecute = true;
            p.StartInfo.FileName = $@"{path}Books\File\{book.fileName}";

            try
            {
                p.Start();
            }
            catch (Exception)
            {

            }
        }

        public void Listen(object o, EventArgs e)
        {
            StackPanel stackPanel = (StackPanel)((StackPanel)((Button)o).Parent).Parent;
            AudioBook book = books.Where(s => s.title == ((TextBlock)stackPanel.Children[1]).Text && s.writer == ((TextBlock)stackPanel.Children[3]).Text).First() as AudioBook;
            string path = Environment.CurrentDirectory[..Environment.CurrentDirectory.IndexOf("bin")];

            MediaPlayer mediaPlayer = new();
            mediaPlayer.Open(new Uri($"{path}Books/Audio/{book.audioName}"));
            mediaPlayer.Play();
        }
    }
}
