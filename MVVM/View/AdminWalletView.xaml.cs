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
using BookLit.Front;
using BookLit.Core;

namespace BookLit.MVVM.View
{
    public partial class AdminWalletView : UserControl
    {
        public AdminWalletView()
        {
            InitializeComponent();
            IncomeText.Text = App.Income.ToString();
        }

        public void Ok_Button_Click(object o, EventArgs e)
        {
            if (AdminWindow.admin.IsPasswordCorrect(ConfirmPasswordBox.Password))
            {
                ConfirmPasswordGrid.Visibility = Visibility.Hidden;
                MainGrid.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Wrong password!");
                ConfirmPasswordBox.Password = "";
            }
        }

        public void TakeMoney_Button_Click(object o, EventArgs e)
        {
            if (double.TryParse(AmountBox.Text, out double amount) || amount > App.Income)
            {
                if (CardNumberBox.Text.IsCardNumber())
                {
                    App.Income -= amount;
                    IncomeText.Text = App.Income.ToString();
                    AmountBox.Text = "";
                    CardNumberBox.Text = "";
                }
                else
                {
                    MessageBox.Show("Invalid card number!");
                    CardNumberBox.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Invlaid amount!");
                AmountBox.Text = "";
            }
        }
    }
}
