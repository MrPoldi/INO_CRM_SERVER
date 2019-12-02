using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INO_CRM_API.Models
{
    public class CompanyModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NIP { get; set; }
        public BranchModel BranchId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public UserModel AddedByUser { get; set; }
        public bool IsDeleted { get; set; }
    }
}
