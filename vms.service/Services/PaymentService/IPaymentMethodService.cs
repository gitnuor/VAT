using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;

namespace vms.service.Services.PaymentService;

public interface IPaymentMethodService : IServiceBase<PaymentMethod>
{
    Task<IEnumerable<PaymentMethod>> GetPaymentMethods();
    Task<PaymentMethod> GetPaymentMethod(string idEnc);
    Task<SelectList> GetPaymentMethodsSelectList();
}