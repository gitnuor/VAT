using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using URF.Core.Abstractions;
using vms.entity.models;
using vms.entity.viewModels;
using vms.service.Services.SecurityService;
using vms.utility.StaticData;
using vms.Utility;

namespace vms.Controllers;

[SessionExpireFilter]
public class ControllerBase1 : Controller
{
    protected IWebHostEnvironment Environment;
    protected IUnitOfWork UnitOfWork;
    protected static IConfiguration Configuration;
    protected VmUserSession UserSession;
    protected readonly IDataProtectionProvider _protectionProvider;
    protected IDataProtector _dataProtector;
    protected IAuditLogService _log;
    protected ControllerBase1(ControllerBaseParamModel controllerBaseParamModel)
    {
        _log = controllerBaseParamModel.VatAuditLogService;
        Environment = controllerBaseParamModel.HostingEnvironment;
        UnitOfWork = controllerBaseParamModel.UnitOfWork;
        _dataProtector = controllerBaseParamModel.ProtectionProvider.CreateProtector(controllerBaseParamModel.VatPurposeStringConstants.UserIdQueryString);
        UserSession = controllerBaseParamModel.HttpContextAccessor.HttpContext.Session.GetComplexData<VmUserSession>(ControllerStaticData.SESSION);
        if (Configuration == null)
        {
               
            var builder = new ConfigurationBuilder()
                .SetBasePath(Environment.ContentRootPath)
                .AddJsonFile(ControllerStaticData.APPLICATION_JSON, true, true);
            Configuration = builder.Build();
        }
    }
    public async System.Threading.Tasks.Task<bool> auditLogCreate(AuditLog auditLog)
    {
        try
        {
            _log.Insert(auditLog);
            await UnitOfWork.SaveChangesAsync();
            return true;
        }

		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

    protected string change(string sourceJsonString, string targetJsonString)
    {
        var sourceJObject = JsonConvert.DeserializeObject<JObject>(sourceJsonString);
        var targetJObject = JsonConvert.DeserializeObject<JObject>(targetJsonString);
        var item = new List<string>();

        if (!JToken.DeepEquals(sourceJObject, targetJObject))
        {
            foreach (KeyValuePair<string, JToken> sourceProperty in sourceJObject)
            {
                JProperty targetProp = targetJObject.Property(sourceProperty.Key);

                if (!JToken.DeepEquals(sourceProperty.Value, targetProp.Value))
                {
                    if (sourceProperty.Key != "TrackingState")
                    {
                        item.Add(sourceProperty.Key + ":" + sourceProperty.Value + "->" + sourceProperty.Key + ":" + targetProp.Value);
                        Console.WriteLine(string.Format("{0} property value is changed", sourceProperty.Key));
                    }

                }
            }
        }
        return string.Join(",", item);
    }

}