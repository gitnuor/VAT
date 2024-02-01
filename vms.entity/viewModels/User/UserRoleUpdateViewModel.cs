using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vms.entity.viewModels.User
{
    public class UserRoleUpdateViewModel
    {
        public int UserId { get; set; }
        public string EncryptedId { get; set; }

        [Display(Name = "Role")]
        [Required]
        public int RoleId { get; set; }
        public IEnumerable<CustomSelectListItem> Roles;
    }
}
