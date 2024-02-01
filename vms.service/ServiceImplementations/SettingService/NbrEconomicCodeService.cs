using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;

namespace vms.service.ServiceImplementations.SettingService;

public class NbrEconomicCodeService : ServiceBase<NbrEconomicCode>, INbrEconomicCodeService
{
    private readonly INbrEconomicCodeRepository _repository;

    public NbrEconomicCodeService(INbrEconomicCodeRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<SelectList> GetNbrEconomicCodeSelectList()
    {
        return new(await _repository.GetNbrEconomicCode(),
            nameof(NbrEconomicCode.NbrEconomicCodeId),
            nameof(NbrEconomicCode.EconomicTitle));
    }
}