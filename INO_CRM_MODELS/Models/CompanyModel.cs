using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace INO_CRM_API.Models
{
    public class CompanyModel
    {
        [Key]
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string NIP { get; set; }
        public int BranchId { get; set; }
        public virtual BranchModel Branch { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int UserId { get; set; }
        public virtual UserModel User { get; set; }
        public bool IsDeleted { get; set; }
    }
}
