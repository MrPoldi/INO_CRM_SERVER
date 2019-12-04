using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace INO_CRM_API.Models
{
    public class ContactPersonModel
    {
        [Key]
        public int ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public int CompanyId { get; set; }
        public virtual CompanyModel Company { get; set; }
        public int UserId { get; set; }
        public virtual UserModel User { get; set; }
        public bool IsDeleted { get; set; }
    }
}
