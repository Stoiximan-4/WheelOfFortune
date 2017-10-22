﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using WheelOfFortune.Models;
using WheelOfFortune.Models.Domain;
using WheelOfFortune.Repos.Interfaces;

namespace WheelOfFortune.Repos
{
    public class BalanceRepo : IBalanceRepo
    {
        private readonly ApplicationDbContext context;

        public BalanceRepo(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Balance GetByUserId(string userId)
        {
            return context.Balances.Where(x => x.User.Id == userId).First();
        }

        public Balance UpdateBalance(decimal balance)
        {
            try
            {
                var userId = HttpContext.Current.User.Identity.GetUserId();
                var blc = GetByUserId(userId);

                blc.BalanceValue = blc.BalanceValue + balance;

                context.Entry(blc).State = EntityState.Modified;
                context.SaveChanges();
                return blc;
            }
            catch (NullReferenceException e)
            {
                return null;
            }
        }

        public Balance CreateBalance(string userId)
        {
         
            var user = context.Users.FirstOrDefault(x => x.Id == userId);

            if (user == null)
               throw new InvalidOperationException();

            var balance = new Balance
            {
                User = user,
                BalanceValue = 100
            };

            context.Balances.Add(balance);
            context.SaveChanges();
            return balance;
        }
    }
}