using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INO_CRM_API.Models
{
    public class ContactPersonModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public CompanyModel Company { get; set; }
        public UserModel AddedByUser { get; set; }
        public bool IsDeleted { get; set; }
    }
}
