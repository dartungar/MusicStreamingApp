using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using Repository.Models;
using Repository.DTO;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;


namespace Service
{
    public class UserRepository
    {
        // user & subscription methods
        public static UserDto GetUser(Guid id)
        {
            using ApplicationContext db = new ApplicationContext();
            return UserToDto(db.Users.Find(id));
        }

        public static UserDto GetUser(string login)
        {
            using ApplicationContext db = new ApplicationContext();
            return UserToDto(db.Users.AsNoTracking().Where(u => u.Login == login).FirstOrDefault());
        }

        public static UserDto CheckUserPassword(string login, string password)
        {
            using ApplicationContext db = new ApplicationContext();
            return UserToDto(db.Users.AsNoTracking().Where(
                u => u.Login == login && 
                u.PasswordHash == string.Join("", password.ToCharArray().Reverse<char>()))
                .FirstOrDefault());
        }

        public static List<UserDto> SearchUsers(string query)
        {
            using ApplicationContext db = new ApplicationContext();
            return db.Users.Where(u => u.Login.Contains(query) || u.Name.Contains(query)|| u.Email.Contains(query)).Select(u => UserToDto(u)).ToList();
        }

        public static void UpdateUser(Guid id, string name, string login, string email)
        {
            using ApplicationContext db = new ApplicationContext();
            User foundUser = db.Users.Find(id);
            if (foundUser != null)
            {
                foundUser.Name = name;
                foundUser.Login = login;
                foundUser.Email = email;
            }
            db.SaveChanges();
        }

        public static void UpdateUser(UserDto user)
        {
            using ApplicationContext db = new ApplicationContext();
            User foundUser = db.Users.Find(user.Id);
            if (foundUser != null)
            {
                foundUser.Name = user.Name;
                foundUser.Login = user.Login;
                foundUser.Email = user.Email;
                foundUser.AddressId = user.AddressId;
            }
            db.SaveChanges();
        }

        // TODO: separate methods for updating user password / email

        // TODO: proper password hashing
        public static UserDto AddOrGetUser(string name, string login, string password, string email, string country, string city, string house)
        {
            using ApplicationContext db = new ApplicationContext();
            User user;
            User existingUser = db.Users.AsNoTracking().Where(u => u.Login == login).FirstOrDefault();
            if (existingUser == null)
            {
                user = new User
                {
                    Login = login,
                    PasswordHash = string.Join("", password.ToCharArray().Reverse<char>()), // тут будет нормальное хэширование
                    Name = name,
                    AddressId = AddressRepository.AddOrGetAddress(country, city, house).Id,
                    Email = email
                };
                db.Users.Add(user);
                db.SaveChanges();
            }
            else user = existingUser;
            return UserToDto(user);
        }

        public static UserDto AddOrGetUser(string name, string login, string password, string email, string country, string region, string city, string street, string house)
        {
            using ApplicationContext db = new ApplicationContext();
            User user;
            User existingUser = db.Users.AsNoTracking().Where(u => u.Login == login).FirstOrDefault();
            if (existingUser == null)
            {
                user = new User
                {
                    Login = login,
                    PasswordHash = string.Join("", password.ToCharArray().Reverse<char>()), // тут будет нормальное хэширование
                    Name = name,
                    AddressId = AddressRepository.AddOrGetAddress(country, region, city, street, house).Id,
                    Email = email
                };
                db.Users.Add(user);
                db.SaveChanges();
            }
            else user = existingUser;
            return UserToDto(user);
        }


        public static void RemoveUser(Guid id)
        {
            using ApplicationContext db = new ApplicationContext();
            User user = db.Users.Find(id);
            if (user != null) db.Users.Remove(user);
            db.SaveChanges();
        }

        private static UserDto UserToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Login = user.Login,
                Email = user.Email,
                AddressId = user.AddressId
            };
        }
    }
}
