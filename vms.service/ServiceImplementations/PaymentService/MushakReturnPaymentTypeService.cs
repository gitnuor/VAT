using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.PaymentService;

namespace vms.service.ServiceImplementations.PaymentService;

public class MushakReturnPaymentTypeService : ServiceBase<MushakReturnPaymentType>, IMushakReturnPaymentTypeService
{
    private readonly IMushakReturnPaymentTypeRepository _repository;

    public MushakReturnPaymentTypeService(IMushakReturnPaymentTypeRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<SelectList> GetSelectList()
    {
        return new(await _repository.GetPaymentType(), nameof(MushakReturnPaymentType.MushakReturnPaymentTypeId),
            nameof(MushakReturnPaymentType.TypeName));
    }

    public async Task<IEnumerable<MushakReturnPaymentType>> MushakReturnPaymentTypeWithCode()
    {
        return await _repository.MushakReturnPaymentTypeWithCode();
    }
}