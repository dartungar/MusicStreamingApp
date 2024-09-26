using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Service;
using Service.DTO;
using Domain.Models;


namespace WebApp
{
    public class LoginDto
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class AuthService
    {

        private readonly UserService _userService;
        public Func<string, Task> _authenticateFunc;


        public AuthService(UserService userService)
        {
            _userService = userService;
        }

        public async Task Login(LoginDto loginDto)
        {
            UserDto userDto = _userService.Get(u => u.Login == loginDto.Login).FirstOrDefault();
            if (userDto == null)
                throw new Exception("Пользователь не найден");
            if (!VerifyPassword(loginDto.Password, userDto.PasswordHash))
                throw new Exception("Неверный пароль");
            await _authenticateFunc(loginDto.Login);
        }

        public async Task Register(UserDto userDto)
        {
            User user = _userService.FromDto(userDto);
            user.PasswordHash = HashPassword(userDto.Password);
            _userService.Add(user);
            await _authenticateFunc(userDto.Login);

        }


        // https://www.automationmission.com/2020/09/17/hashing-and-salting-passwords-in-c/
        private string HashPassword(string password, byte[] salt = null)
        {
            if (salt == null)
            {
                salt = new byte[64 / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }
            }


            string hashedPassword = Convert.ToBase64String(new Rfc2898DeriveBytes(password, salt, 1000, HashAlgorithmName.SHA256).GetBytes(24));
            string saltString = Convert.ToBase64String(salt);
            return hashedPassword + saltString;
        }

        private bool VerifyPassword(string password, string hash)
        {
            string saltString = hash.Substring(hash.Length - 12);
            byte[] salt = Convert.FromBase64String(saltString);
            string newHashedPassword = HashPassword(password, salt);
            return newHashedPassword == hash;
        }
    }
}
