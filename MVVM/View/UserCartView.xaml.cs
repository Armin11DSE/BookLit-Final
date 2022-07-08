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
        double price;
        public UserCartView()
        {
            InitializeComponent();
            price = 0;
            foreach (Book book in UserWindow.user.Cart)
            {
                price += book.Price;
            }
            CostText.Text = price.ToString();
        }

        public void PayDirectly_Button_Click(object o, EventArgs e)
        {
            if (price != 0)
            {
                new PaymentWindow(price).Show();
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
        }
    }
}
