using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vms.entity.viewModels.CustomerViewModel
{
    public class CustomerSubscriptionCategoryCreateViewModel
    {
        public int CustomerSubscriptionCategoryId { get; set; }
        public string EncryptedId { get; set; }
        public int OrganizationId { get; set; }
        public bool IsActive { get; set; }

        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        [Required]
        public string CategoryName { get; set; }
        public string Remarks { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}
