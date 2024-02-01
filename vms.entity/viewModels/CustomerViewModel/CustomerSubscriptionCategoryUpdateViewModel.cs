using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vms.entity.viewModels.CustomerViewModel
{
    public class CustomerSubscriptionCategoryUpdateViewModel
    {
        public int CustomerSubscriptionCategoryId { get; set; }
        public string EncryptedId { get; set; }
        public int OrganizationId { get; set; }

        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        [Required]
        public string CategoryName { get; set; }
        public string Remarks { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
