using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INO_CRM_API.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public RoleModel Role { get; set; }
        public bool IsDeleted { get; set; }
    }
}
