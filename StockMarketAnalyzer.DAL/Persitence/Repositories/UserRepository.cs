﻿using StockMarketAnalyzer.BO;
using StockMarketAnalyzer.DAL.Core.IRepositories;
using StockMarketAnalyzer.DAL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketAnalyzer.DAL.Persitence.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(StockMarketDbContext context)
            : base(context)
        {
        }
        public StockMarketDbContext StockMarketDbContext
        {
            get { return Context as StockMarketDbContext; }
        }

        public void UpdateProfile(UserProfile profile)
        {
            var users = StockMarketDbContext.Users.FirstOrDefault(u => u.UserId == profile.UserId);
            users.UserProfile.FirstName = profile.FirstName;
            users.UserProfile.LastName = profile.LastName;
            users.UserProfile.PhoneNumber = profile.PhoneNumber;
            users.UserProfile.State = profile.State;
            users.UserProfile.City = profile.City;
            users.UserProfile.Country = profile.Country;
            users.UserProfile.Zip = profile.Zip;
            users.UserProfile.Address = profile.Address;
            users.UserProfile.EmailAddress = profile.EmailAddress;
            users.UserProfile.Gender = profile.Gender;
            users.UserProfile.ProfileUrl = profile.ProfileUrl;

            StockMarketDbContext.Entry<User>(users).State = System.Data.Entity.EntityState.Modified;

            StockMarketDbContext.SaveChanges();
        }


        public bool Authenticate(User user)
        {
            user = StockMarketDbContext.Users.FirstOrDefault(u => u.EmailAddress.Equals(user.EmailAddress) && u.Password.Equals(user.Password));

            return user == null ? false : true;
        }

        public bool Register(Register user)
        {

            var newUserProfile = new UserProfile()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                EmailAddress = user.EmailAddress,
                ProfileUrl = user.ProfilePic
            };

            var newUser = new User()
            {
                EmailAddress = user.EmailAddress,
                Password = user.Password,
                UserProfile = newUserProfile,
                UserType = user.UserType
            };

            try
            {
                StockMarketDbContext.Users.Add(newUser);
                StockMarketDbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
                //throw;
            }

        }


        public int GetUserId(string p)
        {
            return StockMarketDbContext.Users.Where(x => x.EmailAddress.Equals(p)).Select(x => x.UserId).FirstOrDefault();
        }


        public UserType GetUserRole(string p)
        {
            return StockMarketDbContext.Users.Where(x => x.EmailAddress.Equals(p)).FirstOrDefault().UserType;
        }
    }
}
