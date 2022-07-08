using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Globalization;
using BookLit.Front;

namespace BookLit.Core
{
    public class Book
    {
        public readonly string title;
        public readonly string writer;
        public readonly int publishYear;

        public double Rating { get; protected set; }
        public int RatingsNum { get; protected set; }
        public double Price { get; protected set; }
        public string Summary { get; protected set; }
        public bool IsVip { get; protected set; }
        public double? Discount { get; protected set; }
        public string? DiscountExpirationDate { get; protected set; }

        public readonly string fileName;
        public readonly string coverName;

        public Book(string title, string writer, int publishYear, double price, string summary, string fileName, string coverName, double rating = 0, int ratingsNum = 0, bool isVip = false, double? discount = null, string? discountExpirationDate = null)
        {

            if (!fileName.IsPdf())
            {
                throw new Exception("Invlalid pdf file address!");
            }

            if (!coverName.IsImage())
            {
                throw new Exception("Invalid image file address!");
            }

            this.title = title;
            this.writer = writer;
            this.publishYear = publishYear;
            this.Price = price;
            this.Summary = summary;
            this.fileName = fileName;
            this.coverName = coverName;
            this.Rating = rating;
            this.RatingsNum = ratingsNum;
            this.IsVip = isVip;
            this.Discount = discount;
            this.DiscountExpirationDate = discountExpirationDate;

            if (DiscountExpirationDate != null && DiscountExpirationDate != "")
            {
                PersianCalendar persianCalender = new();
                if (int.Parse(DiscountExpirationDate[..4]) >= persianCalender.GetYear(DateTime.Now) && int.Parse(DiscountExpirationDate[5..7]) >= persianCalender.GetMonth(DateTime.Now) && int.Parse(DiscountExpirationDate[^2..]) > persianCalender.GetDayOfMonth(DateTime.Now))
                {
                    this.Discount = null;
                    this.DiscountExpirationDate = null;
                }
            }
        }

        public void AddRating(int newRating)
        {
            if (newRating > 5 || newRating < 1)
            {
                throw new Exception("Invalid rating!");
            }

            this.Rating *= RatingsNum;
            this.Rating = (this.Rating + newRating) / (++RatingsNum);

            SqlConnection con = new(App.dataBaseAddress);
            con.Open();
            string command = "Update Books SET rating = '" + this.Rating + "', ratingsNum = '" +this.RatingsNum+ "' Where title = '" + this.title + "' and writer = '" + this.writer + "'";
            SqlCommand cmd = new(command, con);
            cmd.ExecuteNonQuery();

            command = "Insert into RatedBooks values('" + UserWindow.user.Email + "','" +this.title+ "','" +this.writer+ "','" + this.Rating + "')";
            cmd = new(command, con);
            cmd.ExecuteNonQuery();

            con.Close();
        }

        public void Update(string price, string discount, string discountExpiration, bool isVip)
        {
            if (price != "")
            {
                this.Price = double.Parse(price);
            }

            if (discount != "")
            {
                this.Discount = double.Parse(discount);
            }

            if (discountExpiration != null)
            {
                this.DiscountExpirationDate = discountExpiration;
            }

            SqlConnection con = new(App.dataBaseAddress);
            con.Open();
            string command = "Update Books SET price = '" + this.Price + "', discount = '" + this.Discount + "', discountExpiration = '" + this.DiscountExpirationDate + "' Where title = '" + this.title + "' and writer = '" + this.writer + "'";
            SqlCommand cmd = new(command, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }

    internal class AudioBook : Book
    {
        public readonly string audioName;

        public AudioBook(string title, string writer, int publishYear, double price, string summary, string fileName, string coverName, string audioName, double rating = 0, int ratingsNum = 0, bool isVip = false, double? discount = null, string? discountExpirationDate = null)
            : base(title, writer, publishYear, price, summary, fileName, coverName, rating, ratingsNum, isVip, discount, discountExpirationDate)
        {
            if (!audioName.IsAudio())
            {
                throw new Exception("Invlaid audio file name!");
            }

            this.audioName = audioName;
        }
    }
}