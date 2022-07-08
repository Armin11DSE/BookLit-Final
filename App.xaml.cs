using System;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Data.SqlClient;
using BookLit.Core;

namespace BookLit
{
    public partial class App : Application
    {
        public static readonly string dataBaseAddress = @$"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={Environment.CurrentDirectory[..Environment.CurrentDirectory.IndexOf("bin")]}DataBase\SQL\BookLit.mdf;Integrated Security=True;Connect Timeout=30";

        private static double _Income;
        public static double Income
        {
            get => _Income;
            set
            {
                _Income = value;
                _VipCostPerMonth = value;
                SqlConnection con = new(App.dataBaseAddress);
                string command = "Update App SET income = '" + value + "' Where id = '" + 1 + "'";
                con.Open();
                SqlCommand cmd = new(command, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        private static double _VipCostPerMonth;
        public static double VipCostPerMonth
        {
            get => _VipCostPerMonth;
            set
            {
                _VipCostPerMonth = value;
                SqlConnection con = new(App.dataBaseAddress);
                string command = "Update App SET vipPrice = '" + value + "' Where id = '" + 1 + "'";
                con.Open();
                SqlCommand cmd = new(command, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}
