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

namespace BookLit.MVVM.View
{
    public partial class UserWalletView : UserControl
    {
        public UserWalletView()
        {
            InitializeComponent();
            WalletAmountText.Text = UserWindow.user.Wallet.ToString();
        }

        public void ChargeWallet_Button_Click(object o, EventArgs e)
        {
            new PaymentWindow().Show();
        }
    }
}
