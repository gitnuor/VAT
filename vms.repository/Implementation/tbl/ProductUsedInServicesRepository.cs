using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.entity.viewModels.ProductUsedInService;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl
{
    public class ProductUsedInServicesRepository : RepositoryBase<ProductUsedInService>, IProductUsedInServicesRepository
    
    {
        private readonly DbContext _context;
        private readonly IDataProtector _dataProtector;

        public ProductUsedInServicesRepository(DbContext context, IDataProtectionProvider protectionProvider, PurposeStringConstants purposeStringConstants) : base(context)
        {
            _context = context;
            _dataProtector = protectionProvider.CreateProtector(purposeStringConstants.UserIdQueryString);
        }

        public async Task<IEnumerable<ProductUsedInService>> GetProductUsedInServiceList(string pOrgId)
        {
            var orgId = int.Parse(_dataProtector.Unprotect(pOrgId));
            var byProductDataList = await Query()
                .Where(o => o.OrganizationId == orgId)
                .Include(o => o.Organization)
                .Include(o => o.OrgBranch)
                .Include(o => o.Product)
                .Include(o => o.MeasurementUnit)
                .Include(o => o.Customer)
                .SelectAsync();
            var list = byProductDataList.ToList();
            list.ForEach(delegate (ProductUsedInService productUsedInService)
            {
                productUsedInService.EncryptedId =
                    _dataProtector.Protect(productUsedInService.ProductUsedInServiceId.ToString());
            });
            return list;
        }

        public async Task<int> InsertProductUsedInServiceData(VmProductUsedInServicePostModel vmProductUsedInServicePostModel)
        {
            try
            {
                string sql = $"EXEC [dbo].[SpInsertProductUsedInService]  " +
                             $"@OrganizationId," +
                             $"@OrgBranchId," +
                             $"@ProductId," +
                             $"@CustomerId," +
                             $"@Quantity," +
                             $"@MeasurementUnitId," +
                             //$"@ConversionRatio," +
                             $"@IsActive," +
                             $"@ReferenceKey," +
                             $"@CreatedBy," +
                             $"@CreatedTime," +
                             $"@ModifiedBy," +
                             $"@ModifiedTime," +
                             $"@ApiTransactionId," +
                             $"@ExcelDataUploadId"
                             ;

                var parameter = new DynamicParameters();

                parameter.Add("@OrganizationId", vmProductUsedInServicePostModel.OrganizationId);
                parameter.Add("@OrgBranchId", vmProductUsedInServicePostModel.OrgBranchId);
                parameter.Add("@ProductId", vmProductUsedInServicePostModel.ProductId);
                parameter.Add("@CustomerId", vmProductUsedInServicePostModel.CustomerId);
                parameter.Add("@Quantity", vmProductUsedInServicePostModel.Quantity);
                parameter.Add("@MeasurementUnitId", vmProductUsedInServicePostModel.MeasurementUnitId);
                //parameter.Add("@ConversionRatio", vmProductUsedInServicePostModel.ConversionRatio);
                parameter.Add("@IsActive", vmProductUsedInServicePostModel.IsActive);
                parameter.Add("@ReferenceKey", vmProductUsedInServicePostModel.ReferenceKey);
                parameter.Add("@CreatedBy", vmProductUsedInServicePostModel.CreatedBy);
                parameter.Add("@CreatedTime", vmProductUsedInServicePostModel.CreatedTime);
                parameter.Add("@ModifiedBy", vmProductUsedInServicePostModel.ModifiedBy);
                parameter.Add("@ModifiedTime", vmProductUsedInServicePostModel.ModifiedTime);
                parameter.Add("@ApiTransactionId", vmProductUsedInServicePostModel.ApiTransactionId);
                parameter.Add("@ExcelDataUploadId", vmProductUsedInServicePostModel.ExcelDataUploadId);
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
