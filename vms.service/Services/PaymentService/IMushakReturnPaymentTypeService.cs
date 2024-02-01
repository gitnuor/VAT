using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;

namespace vms.service.Services.PaymentService;

public interface IMushakReturnPaymentTypeService : IServiceBase<MushakReturnPaymentType>
{
    Task<SelectList> GetSelectList();
    Task<IEnumerable<MushakReturnPaymentType>> MushakReturnPaymentTypeWithCode();
}