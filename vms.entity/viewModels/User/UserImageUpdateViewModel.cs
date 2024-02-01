using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vms.entity.viewModels.User
{
    public class UserImageUpdateViewModel
    {
        public int UserId { get; set; }
        public string EncryptedId { get; set; }
        public string UserImageUrl { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [Display(Name = "Image")]
        [NotMapped]
        public IFormFile UserImage { get; set; }

        //[Required(ErrorMessage = "{0} is required!")]
        //[Display(Name = "Signature")]
        //[NotMapped]
        //public IFormFile UserSignature { get; set; }
    }
}
