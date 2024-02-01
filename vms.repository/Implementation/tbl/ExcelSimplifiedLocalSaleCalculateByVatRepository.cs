using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class ExcelSimplifiedLocalSaleCalculateByVatRepository(DbContext context) :
	RepositoryBase<ExcelSimplifiedLocalSaleCalculateByVat>(context), IExcelSimplifiedLocalSaleCalculateByVatRepository;