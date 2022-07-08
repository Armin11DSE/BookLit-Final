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

namespace BookLit.Front
{
    public partial class RatingWindow : Window
    {
        Book book;
        public RatingWindow(Book book)
        {
            InitializeComponent();
            this.book = book;
        }

        public void One(object o, EventArgs e)
        {
            book.AddRating(1);
            this.Close();
        }

        public void Two(object o, EventArgs e)
        {
            book.AddRating(2);
            this.Close();
        }

        public void Three(object o, EventArgs e)
        {
            book.AddRating(3);
            this.Close();
        }

        public void Four(object o, EventArgs e)
        {
            book.AddRating(4);
            this.Close();
        }

        public void Five(object o, EventArgs e)
        {
            book.AddRating(5);
            this.Close();
        }
    }
}
