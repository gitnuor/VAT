using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IExportTypeRepository : IRepositoryBase<ExportType>
{
    Task<IEnumerable<ExportType>> GetExportTypes();
    Task<ExportType> GetExportType(string idEnc);
}