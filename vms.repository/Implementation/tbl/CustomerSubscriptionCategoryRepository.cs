﻿using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl
{
	public class CustomerSubscriptionCategoryRepository : RepositoryBase<CustomerSubscriptionCategory>,ICustomerSubscriptionCategoryRepository
	{
		private readonly DbContext _context;
		private readonly IDataProtector _dataProtector;

		public CustomerSubscriptionCategoryRepository(DbContext context, IDataProtectionProvider protectionProvider, PurposeStringConstants purposeStringConstants) : base(context)
		{
			_context = context;
			_dataProtector = protectionProvider.CreateProtector(purposeStringConstants.UserIdQueryString);
		}
		public async Task<IEnumerable<CustomerSubscriptionCategory>> GetCustomerCategory(string pOrgId)
		{
			var orgId = int.Parse(_dataProtector.Unprotect(pOrgId));
			return await Query()
				.Where(o => o.OrganizationId == orgId)
				.SelectAsync();
		}
	}
}
