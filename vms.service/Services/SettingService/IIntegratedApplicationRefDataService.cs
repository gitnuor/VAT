using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.Enums;
using vms.entity.models;

namespace vms.service.Services.SettingService;

public interface IIntegratedApplicationRefDataService : IServiceBase<IntegratedApplicationRefDatum>
{
    Task<IntegratedApplicationRefDatum> GetIntegratedApplicationRefDatum(string idEnc);
    Task<bool> IsReferenceDataExist(int integratedApplicationId, EnumReferenceDataType datType, string refKey);
	Task InsertReferenceData(EnumReferenceDataType datType, int integratedApplicationId, int dataKy, string refKey);
}