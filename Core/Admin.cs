using System;
using System.Data;
using System.Data.SqlClient;

namespace BookLit.Core
{
    public class Admin
    {
        public readonly string Email;
        private readonly string Password;

        public Admin(string email, string password)
        {
            this.Email = email;
            this.Password = password;
        }

        public bool IsPasswordCorrect(string enteredPassword) => this.Password == enteredPassword;
    }
}