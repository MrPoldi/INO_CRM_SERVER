using System.ComponentModel.DataAnnotations;

namespace INO_CRM_API.Models
{
    public class BranchModel
    {
        [Key]
        public int BranchId { get; set; }
        public string Name { get; set; }
    }
}