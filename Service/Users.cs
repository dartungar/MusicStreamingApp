using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using Repository.Models;
using System.Security.Cryptography;

namespace Service
{
    public class Users
    {

        // user & subscription methods
        public List<User> GetUsers()
        {
            using ApplicationContext db = new ApplicationContext();
            return db.Users.ToList();
        }

        public List<User> GetUsers(string query)
        {
            using ApplicationContext db = new ApplicationContext();
            return db.Users.Where(u => u.Login == query || u.Name == query || u.Email == query).ToList();
        }

        public void UpdateUser(User user)
        {
            using ApplicationContext db = new ApplicationContext();
            // нужно ли повторно искать пользователя? по идее, новый контекст => да
            // возможно, стоит использовать паттерн Unit Of Work? когда изучу его
            User foundUser = db.Users.Find(user.Id);
            if (foundUser != null) foundUser = user;
            db.SaveChanges();
        }
        // TODO: separete methods for updating user password / email

        // TODO: proper password hashing
        public User AddUser(string name, string login, string password, string email, string country, string city, int house)
        {
            using ApplicationContext db = new ApplicationContext();
            User newUser = new User
            {
                Id = Guid.NewGuid(),
                Login = login,
                PasswordHash = string.Join("", password.ToCharArray().Reverse<char>()), // тут будет нормальное хэширование
                Name = name,
                AddressId = new Addresses().AddAddress(country, city, house).Id,
                Email = email
            };
            db.Users.Add(newUser);
            db.SaveChanges();
            return newUser;

        }

        public void RemoveUser(User user)
        {
            using ApplicationContext db = new ApplicationContext();
            db.Users.Remove(user);
            db.SaveChanges();
        }

        public void RemoveUser(Guid id)
        {
            using ApplicationContext db = new ApplicationContext();
            User user = db.Users.Find(id);
            if (user != null) db.Users.Remove(user);
            db.SaveChanges();
        }
    }
}
