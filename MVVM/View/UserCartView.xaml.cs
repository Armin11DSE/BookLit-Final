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
using BookLit.Front;

namespace BookLit.MVVM.View
{
    public partial class UserCartView : UserControl
    {
        List<Book> books = new();
        double price;
        public UserCartView()
        {
            InitializeComponent();
            price = 0;
            foreach (Book book in UserWindow.user.Cart)
            {
                price += book.Price;
                if (book.Discount != null)
                {
                    price -= (double)book.Discount;
                }
            }
            CostText.Text = price.ToString();

            books = UserWindow.user.Cart;

            for (int i = 0; i < books.Count; i++)
            {
                Border border = new();
                border.BorderThickness = new(1);
                border.Margin = new Thickness(0, 0, 5, 0);
                border.BorderBrush = Brushes.Yellow;
                border.Width = 150;
                border.Height = 250;

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
                sp.HorizontalAlignment = HorizontalAlignment.Center;
                sp.Margin = new Thickness(0, 5, 0, 0);

                Button removeButton = new();
                removeButton.Click += Remove_Button_Click;
                removeButton.VerticalAlignment = VerticalAlignment.Bottom;
                removeButton.HorizontalAlignment = HorizontalAlignment.Center;
                removeButton.BorderThickness = new(0);
                removeButton.Content = "Remove";
                removeButton.Foreground = Brushes.BlueViolet;
                removeButton.Background = Brushes.Transparent;
                removeButton.Margin = new Thickness(0, 0, 5, 0);

                sp.Children.Add(removeButton);

                bookPanel.Children.Add(sp);
                border.Child = bookPanel;
                CartPanel.Children.Add(border);
            }
        }

        public void PayDirectly_Button_Click(object o, EventArgs e)
        {
            if (price != 0)
            {
                new PaymentWindow(UserWindow.user.Cart, price).Show();
            }
        }

        public void PayByWallet_Button_Click(object o, EventArgs e)
        {
            if (price == 0)
            {
                return;
            }

            if (UserWindow.user.Wallet < price)
            {
                MessageBox.Show("Insufficiant amount of money!");
                return;
            }

            UserWindow.user.TakeMoney(price);

            for (int i = UserWindow.user.Cart.Count - 1; i >= 0; i--)
            {
                UserWindow.user.AddToBooks(UserWindow.user.Cart[i]);
                UserWindow.user.RemoveFromCart(UserWindow.user.Cart[i]);
                UserWindow.user.Cart.RemoveAt(i);
            }

            MessageBox.Show("Purchase has been completed.");
            price = 0;
            CostText.Text = "0";
        }

        public void Remove_Button_Click(object o, EventArgs e)
        {
            StackPanel stackPanel = (StackPanel)((StackPanel)((Button)o).Parent).Parent;
            Book book = books.Where(s => s.title == ((TextBlock)stackPanel.Children[1]).Text && s.writer == ((TextBlock)stackPanel.Children[3]).Text).First();
            UserWindow.user.RemoveFromCart(book);
            this.price -= book.Price;
            if (book.Discount != null)
            {
                price += (double)book.Discount;
            }

            ((Border)stackPanel.Parent).Visibility = Visibility.Hidden;
            CostText.Text = price.ToString();
        }
    }
}
