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

namespace BookLit.MVVM.View
{
    public partial class AdminSetVipView : UserControl
    {
        public AdminSetVipView()
        {
            InitializeComponent();
            CostText.Text = App.VipCostPerMonth.ToString();
        }

        public void ChangeVip_Button_Click(object o, EventArgs e)
        {
            if (double.TryParse(VipCostBox.Text, out double newCost))
            {
                App.VipCostPerMonth = newCost;
                CostText.Text = App.VipCostPerMonth.ToString();
                VipCostBox.Text = "";
            }
            else
            {
                MessageBox.Show("Invlaid cost!");
            }
        }
    }
}
