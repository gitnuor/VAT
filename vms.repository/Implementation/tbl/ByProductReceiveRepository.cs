using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.entity.viewModels.ByProductReceive;
using vms.repository.Repository.tbl;
using vms.utility;

namespace vms.repository.Implementation.tbl
{
    public class ByProductReceiveRepository : RepositoryBase<ByProductReceive>, IByProductReceiveRepository
	{
		private readonly DbContext _context;
		private readonly IDataProtector _dataProtector;

		public ByProductReceiveRepository(DbContext context, IDataProtectionProvider protectionProvider, PurposeStringConstants purposeStringConstants) : base(context)
		{
			_context = context;
			_dataProtector = protectionProvider.CreateProtector(purposeStringConstants.UserIdQueryString);
		}

		public async Task<IEnumerable<ByProductReceive>> GetByProductReceiveList(string pOrgId)
		{
			var orgId = int.Parse(_dataProtector.Unprotect(pOrgId));
			var byProductDataList =  await Query()
				.Where(o => o.OrganizationId == orgId)
				.Include(o => o.Organization)
				.Include(o => o.OrgBranch)
				.Include(o => o.Product)
				.Include(o => o.MeasurementUnit)
				.SelectAsync();
			var list = byProductDataList.ToList();
			list.ForEach(delegate (ByProductReceive byProductReceive)
			{
				byProductReceive.EncryptedId =
					_dataProtector.Protect(byProductReceive.ByProductReceiveId.ToString());
			});
			return list;
		}

        public async Task<int> InsertByProductReceiveData(VmByProductReceivePostModel vmByProductReceivePostModel)
        {
            try
            {
                string sql = $"EXEC [dbo].[SpInsertByProductReceive]  " +
                             $"@OrganizationId," +
                             $"@OrgBranchId," +
                             $"@ProductId," +
                             $"@ProductDescription," +
                             $"@Quantity," +
                             $"@UnitPrice," +
                             $"@MeasurementUnitId," +
                             $"@ReceiveDate," +
                             $"@Remarks," +
                             $"@ReferenceKey," +
                             $"@CreatedBy," +
                             $"@CreatedTime" 
                             ;

                var parameter = new DynamicParameters();

                parameter.Add("@OrganizationId", vmByProductReceivePostModel.OrganizationId);
                parameter.Add("@OrgBranchId", vmByProductReceivePostModel.OrgBranchId);
                parameter.Add("@ProductId", vmByProductReceivePostModel.ProductId);
                parameter.Add("@ProductDescription", vmByProductReceivePostModel.ProductDescription);
                parameter.Add("@Quantity", vmByProductReceivePostModel.Quantity);
                parameter.Add("@UnitPrice", vmByProductReceivePostModel.UnitPrice);
                parameter.Add("@MeasurementUnitId", vmByProductReceivePostModel.MeasurementUnitId);
                parameter.Add("@ReceiveDate",
                    StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(vmByProductReceivePostModel.ReceiveDate));
                parameter.Add("@Remarks", vmByProductReceivePostModel.Remarks);
                parameter.Add("@ReferenceKey", vmByProductReceivePostModel.ReferenceKey);
                parameter.Add("@CreatedBy", vmByProductReceivePostModel.CreatedBy);
                parameter.Add("@CreatedTime", vmByProductReceivePostModel.CreatedTime);
                using (var queryMultiple = _context.Database.GetDbConnection()
                           .QueryMultiple(sql, parameter, commandTimeout: 500))
                {
                    return await queryMultiple.ReadFirstAsync<int>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
