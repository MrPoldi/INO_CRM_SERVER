using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INO_CRM_API.Models
{
    public class TradeNoteModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public CompanyModel Company { get; set; }
        public UserModel AddedByUser { get; set; }
        public bool IsDeleted { get; set; }
    }
}
