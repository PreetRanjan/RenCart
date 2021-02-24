using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RenCart.API.Dtos
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool IsLocked { get; set; }
        public IList<string> Roles { get; set; }
        public string Token { get; set; }
    }
}
