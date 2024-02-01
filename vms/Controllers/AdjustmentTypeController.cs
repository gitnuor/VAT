using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using vms.entity.viewModels;
using vms.service.Services.SettingService;
using vms.Utility;

namespace vms.Controllers;

public class AdjustmentTypeController : ControllerBase
{
    private readonly IAdjustmentTypeService _adjustmentType;
    public AdjustmentTypeController(IAdjustmentTypeService adjustmentType, ControllerBaseParamModel controllerBaseParamModel) : base(controllerBaseParamModel)
    {
        _adjustmentType = adjustmentType;
    }

    [VmsAuthorize(FeatureList.INTEGRATION_SETUP_ADJUSTMENT_TYPE_CAN_VIEW)]
    public async Task<IActionResult> Index()
    {
        return View(await _adjustmentType.Query().SelectAsync());
    }
}