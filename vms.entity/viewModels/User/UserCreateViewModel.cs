using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using vms.entity.models;

namespace vms.entity.viewModels.User
{
	public class UserCreateViewModel
	{
		public int UserId { get; set; }
		[Required]
		[MaxLength(200)]
		[Display(Name = "Name")]
		public string FullName { get; set; }

		public string UserName { get; set; }

		public string EmployeeId { get; set; }

		public string EmployeePin { get; set; }

		public string Designation { get; set; }

		[Required]
		[MaxLength(100)]
		[EmailAddress]
		[Display(Name = "Username/EmailAddress")]
		public string EmailAddress { get; set; }

		[Required]
		[MaxLength(50)]
		public string Mobile { get; set; }

		[Required]
		[MaxLength(50)]
		[Display(Name = "NID")]
		public string NidNo { get; set; }

		[MaxLength(50)]
		[Display(Name = "TIN")]
		public string TinNo { get; set; }

		[Required]
		[MaxLength(200)]
		public string Address { get; set; }

		public string UserImageUrl { get; set; }

		public string UserSignUrl { get; set; }

		public string Password { get; set; }

		public int UserTypeId { get; set; }

		public int RoleId { get; set; }

		public int? OrganizationId { get; set; }

		[DisplayName("Is Require Branch?")]
		public bool IsRequireBranch { get; set; }

		public bool IsActive { get; set; }

		public string InActivationReason { get; set; }

		public DateTime? InActivationTime { get; set; }

		public int? InActivatedBy { get; set; }

		public bool? IsLocked { get; set; }

		public DateTime? LastLockTime { get; set; }

		public string LastLockReason { get; set; }

		public int? LastUnlockedBy { get; set; }

		public DateTime? LastUnlockTime { get; set; }

		public DateTime? LastLoginTime { get; set; }

		public int? AccessFailedCount { get; set; }

		public bool IsDefaultPassword { get; set; }

		public bool IsCompanyRepresentative { get; set; }

		public string ReferenceKey { get; set; }

		public int? CreatedBy { get; set; }

		public DateTime? CreatedTime { get; set; }

		public DateTime? ExpiryDate { get; set; }

		public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

		public virtual Organization Organization { get; set; }

		public virtual Role Role { get; set; }

		public List<UserBranch> UserBranches { get; set; } = new List<UserBranch>();

		public List<UserLoginHistory> UserLoginHistories { get; set; } = new List<UserLoginHistory>();

		public virtual UserType UserType { get; set; }

		public List<UserBranchCreateViewModel> UserBranchList { get; set; } = new();
      

        public IEnumerable<CustomSelectListItem> Roles;
		public IEnumerable<CustomSelectListItem> UserTypes;

		[Required(ErrorMessage = "{0} is required!")]
		[Display(Name = "Image")]
		[NotMapped]
		public IFormFile UserImage { get; set; }

        [Display(Name = "Signature")]
        [NotMapped]
        public IFormFile UserSignature { get; set; }
    }
}
