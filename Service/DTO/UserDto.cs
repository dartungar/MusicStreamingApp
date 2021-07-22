using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Service.DTO
{
   public class UserDto
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Login { get; set; }
        [MinLength(6, ErrorMessage = "Password must be minimum 6 characters long")]
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public string Name { get; set; }
        [EmailAddress(ErrorMessage = "Invalid e-mail address")]
        public string Email { get; set; }
        public Guid? AddressId { get; set; }
        public AddressDto Address { get; set; }
    }
}
