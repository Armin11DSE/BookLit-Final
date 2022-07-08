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

namespace BookLit.MVVM.View
{
    public partial class UserBookmarksView : UserControl
    {
        List<Book> books = new();

        public UserBookmarksView()
        {
            InitializeComponent();

            books = UserWindow.user.Bookmarks;

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
                text.Text = "  rating: " + books[i].Rating + "      by      " + books[i].RatingsNum;
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

                Button removeButton = new();
                removeButton.Click += Remove_Button_Click;
                removeButton.VerticalAlignment = VerticalAlignment.Bottom;
                removeButton.BorderThickness = new(0);
                removeButton.Content = "Remove";
                removeButton.Foreground = Brushes.BlueViolet;
                removeButton.Background = Brushes.Transparent;
                removeButton.Margin = new Thickness(0, 0, 5, 0);

                Button rateButton = new();
                rateButton.Click += Sum_Button_Click;
                rateButton.VerticalAlignment = VerticalAlignment.Bottom;
                rateButton.BorderThickness = new(0);
                rateButton.Content = "Summary";
                rateButton.Foreground = Brushes.BlueViolet;
                rateButton.Background = Brushes.Transparent;
                rateButton.Margin = new Thickness(0, 0, 5, 0);

                Button readButton = new();
                readButton.Click += Add_Button_Click;
                readButton.BorderThickness = new(0);
                readButton.Content = "Add";
                readButton.Foreground = Brushes.BlueViolet;
                readButton.VerticalAlignment = VerticalAlignment.Bottom;
                readButton.Background = Brushes.Transparent;

                sp.Children.Add(removeButton);
                sp.Children.Add(rateButton);
                sp.Children.Add(readButton);

                bookPanel.Children.Add(sp);
                border.Child = bookPanel;
                BooksPanel.Children.Add(border);
            }
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

            ((Border)stackPanel.Parent).Visibility = Visibility.Hidden;
            UserWindow.user.AddToCart(book);
            MessageBox.Show("Book was successfully added to your cart.");
        }

        public void Sum_Button_Click(object o, EventArgs e)
        {
            StackPanel stackPanel = (StackPanel)((StackPanel)((Button)o).Parent).Parent;
            Book book = books.Where(s => s.title == ((TextBlock)stackPanel.Children[1]).Text && s.writer == ((TextBlock)stackPanel.Children[3]).Text).First();
            MessageBox.Show(book.Summary);
        }

        public void Remove_Button_Click(object o, EventArgs e)
        {
            StackPanel stackPanel = (StackPanel)((StackPanel)((Button)o).Parent).Parent;
            Book book = books.Where(s => s.title == ((TextBlock)stackPanel.Children[1]).Text && s.writer == ((TextBlock)stackPanel.Children[3]).Text).First();
            for (int i = 0; i < UserWindow.user.Bookmarks.Count; i++)
            {
                if (UserWindow.user.Bookmarks[i].title == book.title && UserWindow.user.Bookmarks[i].writer == book.writer)
                {
                    MessageBox.Show("Book has already been removed from bookmarks!");
                    return;
                }
            }

            ((Border)stackPanel.Parent).Visibility = Visibility.Hidden;
            UserWindow.user.RemoveFromBookmarks(book);
            MessageBox.Show("Book was removed from bookmarks.");
        }
    }
}
