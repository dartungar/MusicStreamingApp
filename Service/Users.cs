using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using Repository.Models;
using Repository.DTO;
using System.Security.Cryptography;

namespace Service
{
    public class Users
    {

        // user & subscription methods
        public static UserDto GetUser(Guid id)
        {
            using ApplicationContext db = new ApplicationContext();
            return UserToDto(db.Users.Find(id));
        }

        public static List<UserDto> GetUsers(string query)
        {
            using ApplicationContext db = new ApplicationContext();
            return db.Users.Where(u => u.Login == query || u.Name == query || u.Email == query).Select(u => UserToDto(u)).ToList();
        }

        public static void UpdateUser(Guid id, string name, string login, string email)
        {
            using ApplicationContext db = new ApplicationContext();
            // нужно ли повторно искать пользователя? по идее, новый контекст => да
            // возможно, стоит использовать паттерн Unit Of Work? когда изучу его
            User foundUser = db.Users.Find(id);
            if (foundUser != null)
            {
                foundUser.Name = name;
                foundUser.Login = login;
                foundUser.Email = email;
            }
            db.SaveChanges();
        }

        // TODO: separate methods for updating user password / email

        // TODO: proper password hashing
        public static UserDto AddOrGetUser(string name, string login, string password, string email, string country, string city, string house)
        {
            using ApplicationContext db = new ApplicationContext();
            User newUser = new User
            {
                Id = Guid.NewGuid(),
                Login = login,
                PasswordHash = string.Join("", password.ToCharArray().Reverse<char>()), // тут будет нормальное хэширование
                Name = name,
                AddressId = Addresses.AddOrGetAddress(country, city, house).Id,
                Email = email
            };
            db.Users.Add(newUser);
            db.SaveChanges();
            return UserToDto(newUser);

        }

        public static UserDto AddUser(string name, string login, string password, string email, string country, string region, string city, string street, string house)
        {
            using ApplicationContext db = new ApplicationContext();
            User newUser = new User
            {
                Id = Guid.NewGuid(),
                Login = login,
                PasswordHash = string.Join("", password.ToCharArray().Reverse<char>()), // тут будет нормальное хэширование
                Name = name,
                AddressId = Addresses.AddOrGetAddress(country, region, city, street, house).Id,
                Email = email
            };
            db.Users.Add(newUser);
            db.SaveChanges();
            return UserToDto(newUser);

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
