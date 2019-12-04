using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace INO_CRM_API.Models
{
    public class TradeNoteModel
    {
        [Key]
        public int NoteId { get; set; }
        public string Content { get; set; }
        public int CompanyId { get; set; }
        public virtual CompanyModel Company { get; set; }
        public int UserId { get; set; }
        public virtual UserModel User { get; set; }
        public bool IsDeleted { get; set; }
    }
}
