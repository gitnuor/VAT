using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vms.entity.viewModels.User
{
    public class UserBranchCreateViewModel
    {
    
        public int OrgBranchId { get; set; }
        public string BranchName { get; set; }
		public bool IsSelected { get; set; }
	}
}
