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
    public partial class UserProfileView : UserControl
    {
        public UserProfileView()
        {
            InitializeComponent();
        }

        public void Ok_Button_Click(object o, EventArgs e)
        {
            if (UserWindow.user.IsPasswordCorrect(ConfirmPasswordBox.Password))
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

        public void ChangeProfile_Button_Click(object o, EventArgs e)
        {
            if (FirstnameBox.Text != "" && !FirstnameBox.Text.IsName())
            {
                MessageBox.Show("Invalid new firstname!");
                FirstnameBox.Text = "";
                return;
            }

            if (LastnameBox.Text != "" && !LastnameBox.Text.IsName())
            {
                MessageBox.Show("Invalid new lastname!");
                LastnameBox.Text = "";
                return;
            }

            if (PhoneNumberBox.Text != "" && !PhoneNumberBox.Text.IsPhoneNumber())
            {
                MessageBox.Show("Invalid new phone number!");
                PhoneNumberBox.Text = "";
                return;
            }

            if (PasswordBox.Password != "" && !PasswordBox.Password.IsPassword())
            {
                MessageBox.Show("Invalid new password!");
                PasswordBox.Password = "";
                return;
            }


            UserWindow.user.UpdateProfile(FirstnameBox.Text.Trim(), LastnameBox.Text.Trim(), PhoneNumberBox.Text.Trim(), PasswordBox.Password.Trim());
            FirstnameBox.Text = "";
            LastnameBox.Text = "";
            PhoneNumberBox.Text = "";
            PasswordBox.Password = "";
            MessageBox.Show("Profile info was successfully changed.");
        }
    }
}
