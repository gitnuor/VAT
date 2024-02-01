using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.entity.viewModels.User
{
    public class UserBranchUpdateViewModel
    {
        public int UserId { get; set; }
        public string EncryptedId { get; set; }
        [DisplayName("Is Require Branch?")]
        public bool IsRequireBranch { get; set; }
        public List<UserBranch> UserBranches { get; set; } = new List<UserBranch>();
        public List<UserBranchCreateViewModel> UserBranchList { get; set; } = new();

    }
}
