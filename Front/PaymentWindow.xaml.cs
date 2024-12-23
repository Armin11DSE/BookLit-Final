﻿using System;
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
using System.Globalization;

namespace BookLit.Front
{
    public partial class PaymentWindow : Window
    {
        double? amount;
        bool VipWannaBe;
        uint monthNumForVip;
        List<Book> cart = new();

        public PaymentWindow()
        {
            InitializeComponent();
            amount = null;
        }

        public PaymentWindow(List<Book> cart, double amount)
        {
            InitializeComponent();
            this.cart = cart;
            this.amount = amount;
            AmountBox.Text = amount.ToString();
            AmountBox.IsReadOnly = true;
        }

        public PaymentWindow(double amount, bool wantToBecomeVip = false, uint monthNumForVip = 0)
        {
            InitializeComponent();
            this.amount = amount;
            AmountBox.Text = amount.ToString();
            AmountBox.IsReadOnly = true;
            VipWannaBe = wantToBecomeVip;
            this.monthNumForVip = monthNumForVip;
        }

        public void Return_Button_Click(object o, EventArgs e) => this.Close();

        public void Transter_Button_Click(object o, EventArgs e)
        {
            if (CardNumberBox.Text.IsCardNumber())
            {
                if (CvvBox.Text.IsCVV())
                {
                    if ($"{YearBox.Text}/{MonthBox.Text}".IsCardExpirationDate())
                    {
                        if (double.TryParse(AmountBox.Text, out double amount))
                        {
                            if (this.amount == null && !VipWannaBe)
                            {
                                UserWindow.user.AddMoney(amount);
                            }
                            else if (this.amount != null && VipWannaBe)
                            {
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
                            }
                            else if (cart.Count != 0)
                            {
                                for (int i = 0; i < cart.Count; i++)
                                {
                                    UserWindow.user.AddToBooks(cart[i]);
                                    UserWindow.user.RemoveFromCart(cart[i]);
                                }
                            }

                            MessageBox.Show("Money transfer was successfull.");
                            App.Income += double.Parse(AmountBox.Text);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Invalid amount!");
                            AmountBox.Text = "";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid expiration date!");
                        YearBox.Text = "";
                        MonthBox.Text = "";
                    }
                }
                else
                {
                    MessageBox.Show("Invalid CVV!");
                    CvvBox.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Invalid card number!");
                CardNumberBox.Text = "";
            }
        }
    }
}
