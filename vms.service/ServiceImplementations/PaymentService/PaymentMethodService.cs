using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.PaymentService;

namespace vms.service.ServiceImplementations.PaymentService;

public class PaymentMethodService : ServiceBase<PaymentMethod>, IPaymentMethodService
{
    private readonly IPaymentMethodRepository _repository;

    public PaymentMethodService(IPaymentMethodRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PaymentMethod>> GetPaymentMethods()
    {
        return await _repository.GetPaymentMethods();
    }

    public async Task<PaymentMethod> GetPaymentMethod(string idEnc)
    {
        return await _repository.GetPaymentMethod(idEnc);
    }

    public async Task<SelectList> GetPaymentMethodsSelectList()
    {
        return new(await _repository.GetPaymentMethods(),
            nameof(PaymentMethod.PaymentMethodId),
            nameof(DocumentType.Name));
    }
}