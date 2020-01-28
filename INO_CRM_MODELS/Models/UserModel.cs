using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace INO_CRM_API.Models
{
    public class UserModel
    {     
        [Key]
        public int UserId { get; set; }        
        public string FirstName { get; set; }        
        public string LastName { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        //[JsonIgnore]
        public virtual RoleModel Role { get; set; }
        public bool IsDeleted { get; set; }
    }
}
