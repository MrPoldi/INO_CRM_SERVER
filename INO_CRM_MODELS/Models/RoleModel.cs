using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace INO_CRM_API.Models
{
    public class RoleModel
    {
        [Key]
        public int RoleId { get; set; }
        public string Name { get; set; }
        //[JsonIgnore]
        //public virtual ICollection<UserModel> Users { get; set; }
    }
}