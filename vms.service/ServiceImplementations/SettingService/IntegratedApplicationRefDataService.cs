using System.Collections.Generic;
using System.Threading.Tasks;
using URF.Core.Abstractions;
using vms.entity.Enums;
using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;

namespace vms.service.ServiceImplementations.SettingService;

public class IntegratedApplicationRefDataService : ServiceBase<IntegratedApplicationRefDatum>, IIntegratedApplicationRefDataService
{
    private readonly IIntegratedApplicationRefDataRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

	public IntegratedApplicationRefDataService(IIntegratedApplicationRefDataRepository repository, IUnitOfWork unitOfWork) : base(repository)
	{
		_repository = repository;
		_unitOfWork = unitOfWork;
	}

    public Task<IntegratedApplicationRefDatum> GetIntegratedApplicationRefDatum(string idEnc)
    {
        return _repository.GetIntegratedApplicationRefDatum(idEnc);
    }

    public Task InsertReferenceData(EnumReferenceDataType datType, int integratedApplicationId, int dataKy, string refKey)
    {
	    var refData = new IntegratedApplicationRefDatum
	    {
		    IntegratedApplicationId = integratedApplicationId,
		    IntegratedApplicationRefDataTypeId = (int)datType,
		    DataKey = dataKy,
		    ReferenceKey = refKey
	    };
        _repository.Insert(refData);
        return _unitOfWork.SaveChangesAsync();
    }

    public Task<bool> IsReferenceDataExist(int integratedApplicationId, EnumReferenceDataType datType, string refKey)
    {
        return _repository.Query().AnyAsync(x => x.IntegratedApplicationId == integratedApplicationId && x.IntegratedApplicationRefDataTypeId == (int)datType && x.ReferenceKey == refKey);
    }
}