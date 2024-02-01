using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vms.entity.viewModels.User
{
    public class UserPasswordUpdateViewModel
    {
        public int UserId { get; set; }
        public string EncryptedId { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
    }
}
