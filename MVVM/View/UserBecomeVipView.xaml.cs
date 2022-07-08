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
using System.Globalization;

namespace BookLit.MVVM.View
{
    public partial class UserBecomeVipView : UserControl
    {
        public UserBecomeVipView()
        {
            InitializeComponent();
            CostText.Text = App.VipCostPerMonth.ToString();
        }

        public void PayDirectly_Button_Click(object o, EventArgs e)
        {
            if (uint.TryParse(MonthsNumBox.Text, out uint amount))
            {
                if (!UserWindow.user.IsVip)
                {
                    new PaymentWindow(amount * App.VipCostPerMonth, true, amount).Show();
                }
                else
                {
                    MessageBox.Show("You have already purchased VIP membership!");
                }
            }
            else
            {
                MessageBox.Show("Invalid number of months!");
            }
        }

        public void PayByWallet_Button_Click(object o, EventArgs e)
        {
            if (uint.TryParse(MonthsNumBox.Text, out uint amount))
            {
                if (!UserWindow.user.IsVip)
                {
                    double cost = amount * App.VipCostPerMonth;
                    if (cost < UserWindow.user.Wallet)
                    {
                        UserWindow.user.TakeMoney(cost);
                        UserWindow.user.IsVip = true;

                        PersianCalendar persianCalendar = new();
                        int months = (int)(persianCalendar.GetMonth(DateTime.Now) + amount);
                        int day = persianCalendar.GetDayOfMonth(DateTime.Now);
                        int month = months % 12;

                        string vipExpirationDate = $"{persianCalendar.GetYear(DateTime.Now) + months / 12}/";
                        if (month < 10)
                        {
                            vipExpirationDate += $"0{month}/";
                        }
                        else
                        {
                            vipExpirationDate += $"{month}/";
                        }

                        if (day < 10)
                        {
                            vipExpirationDate += $"0{day}";
                        }
                        else
                        {
                            vipExpirationDate += $"{day}";
                        }

                        UserWindow.user.VipExpirationDate = vipExpirationDate;
                        MessageBox.Show("Congratulations! You are now a part of the VIP membership.");
                    }
                    else
                    {
                        MessageBox.Show("Insufficiant amount!");
                    }
                }
                else
                {
                    MessageBox.Show("You have already purchased VIP membership!");
                }
            }
            else
            {
                MessageBox.Show("Invalid number of months!");
            }
        }
    }
}
