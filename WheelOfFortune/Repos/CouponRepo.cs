﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using WheelOfFortune.Models;
using WheelOfFortune.Models.Domain;
using WheelOfFortune.Models.ViewModels;
using WheelOfFortune.Repos.Interfaces;

namespace WheelOfFortune.Repos
{
    public class CouponRepo : ICouponRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly string _validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private readonly int _codeLength = 10;

        public CouponRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Coupon> GetAll()
        {
            return _context.Coupons.Include(x => x.User).Include(c => c.Value).ToList();
        }

        public IEnumerable<Coupon> GetByUserId(string userId)
        {
            return _context.Coupons.Where(x => x.User.Id == userId).Include(x => x.User).Include(c => c.Value).ToList();
        }

        public Coupon UpdateExpirationDate(Coupon coupon, DateTime date)
        {
            var c = _context.Coupons.SingleOrDefault(code => code.Id == coupon.Id);
            if (c == null) return null;

            c.DateExpired = date;

            _context.Entry(c).State = EntityState.Modified;
            _context.SaveChanges();
            return c;
        }

        public Coupon CreateCoupon(int value, string userId)
        {
            var user = _context.Users.First(x => x.Id == userId);

            if (user == null)
                throw new Exception("User does not exist");

            var cv = _context.CouponValues.FirstOrDefault(x => x.Value == value);

            if (cv == null)
                throw new Exception("Value does not exist");

            var coupon = new Coupon
            {
                Code = GetCode(),
                Value = cv,
                DateCreated = DateTime.Now,
                DateExpired = DateTime.Now.AddDays(10),
                DateExchanged = DateTime.Now,
                User = user,
                Active = true,
            };

            _context.Coupons.Add(coupon);
            _context.SaveChanges();
            return coupon;
        }

        private string GetCode()
        {
            var sb = new StringBuilder();
            using (var provider = new RNGCryptoServiceProvider())
            {
                while (sb.Length < _codeLength)
                {
                    var oneByte = new byte[1];
                    provider.GetBytes(oneByte);
                    var character = (char) oneByte[0];
                    if (_validChars.Contains(character))
                    {
                        sb.Append(character);
                    }
                }

                return sb.ToString();
            }
        }

        public decimal? Exchange(string code)
        {
            var coupon = _context.Coupons.Where(x => x.Code == code).Include(x => x.User).Include(c => c.Value)
                .FirstOrDefault();


            var now = DateTime.Now;
            if (coupon == null || !coupon.Active || coupon.DateExpired < now)
                return null;


            coupon.Active = false;
            coupon.DateExchanged = now;
            _context.Entry(coupon).State = EntityState.Modified;
            _context.SaveChanges();

            return coupon.Value.Value;
        }

        public bool Delete(int id)
        {
            var coupon = _context.Coupons.FirstOrDefault(x => x.Id == id);

            if (coupon == null)
            {
                return false;
            }

            _context.Entry(coupon).State = EntityState.Deleted;
            _context.SaveChanges();

            return true;
        }


    }
}