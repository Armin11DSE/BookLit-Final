using System;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;

namespace BookLit.Core
{
    internal static class Validations
    {
        public static bool IsName(this string name)
        {
            const string pattern = @"^[a-zA-Z]{3,32}$";
            return Regex.IsMatch(name, pattern);
        }

        public static bool IsEmail(this string email)
        {
            const string pattern = @"^.{1,32}@.{1,32}\.{1,32}";
            return Regex.IsMatch(email, pattern);
        }

        public static bool IsPhoneNumber(this string phoneNumber)
        {
            const string pattern = @"^09\d{9}$";
            return Regex.IsMatch(phoneNumber, pattern);
        }

        public static bool IsPassword(this string password)
        {
            const string size_pattern = @".{8,40}";
            const string word_pattern = @"[a-zA-Z]";
            return Regex.IsMatch(password, size_pattern) && Regex.IsMatch(password, word_pattern);
        }

        public static bool IsCVV(this string cvv)
        {
            const string pattern = @"^\d{3,4}$";
            return Regex.IsMatch(cvv, pattern);
        }

        public static bool IsCardNumber(this string cardNumber)
        {
            const string pattern = @"^\d{10}";
            if (!Regex.IsMatch(cardNumber, pattern))
            {
                return false;
            }

            int sum = 0;

            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                if (i % 2 == 1)
                {
                    int doubledNumber = (cardNumber[i] - '0') * 2;
                    while (doubledNumber > 0)
                    {
                        sum += doubledNumber % 10;
                        doubledNumber /= 10;
                    }
                    continue;
                }
                sum += cardNumber[i] - '0';
            }

            return sum % 10 == 0;
        }

        public static bool IsCardExpirationDate(this string expirationDate)
        {
            const string pattern = @"^\d{2}/\d{2}$";
            if (!Regex.IsMatch(expirationDate, pattern))
            {
                return false;
            }

            PersianCalendar persianCalendar = new();

            if (int.Parse(expirationDate[..2]) > persianCalendar.GetYear(DateTime.Now) % 100)
            {
                return true;
            }

            if (int.Parse(expirationDate[..2]) == persianCalendar.GetYear(DateTime.Now) % 100 && int.Parse(expirationDate[^2..]) >= persianCalendar.GetMonth(DateTime.Now))
            {
                return true;
            }

            return false;
        }

        public static bool IsReleaseyear(this int releaseYear)
            => releaseYear <= (int)new PersianCalendar().GetYear(DateTime.Now);

        public static bool IsPdf(this string fileName)
            => File.Exists(@$"..\..\..\Books\File\{fileName}")
            && fileName[^3..] == "pdf";

        public static bool IsImage(this string fileName)
            => File.Exists(@$"..\..\..\Books\Cover\{fileName}")
            && (fileName[^3..] == "jpg" || fileName[^3..] == "png");

        public static bool IsAudio(this string fileName)
        {
            string extension = fileName[^3..];

            return File.Exists(@$"..\..\..\Books\Audio\{fileName}")
                && (extension == "mp3"
                || extension == "aax"
                || extension == "m4a"
                || extension == "m4b"
                || extension == "aac"
                || extension == "m4p"
                || extension == "ogg"
                || extension == "wma"
                || extension == "wav"
                || extension == "wma"
                || extension == "alac");
        }
    }
}
