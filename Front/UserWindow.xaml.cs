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
using BookLit.MVVM.ViewModel;

namespace BookLit.Front
{
    public partial class UserWindow : Window
    {
        public static User user { get; set; }

        public UserWindow(User _user)
        {
            InitializeComponent();
            user = _user;
        }

        public void Exit_Button_Click(object o, EventArgs e)
        {
            new StartingWindow().Show();
            this.Close();
        }
    }
}
