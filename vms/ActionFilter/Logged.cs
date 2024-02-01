using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using URF.Core.Abstractions;
using vms.entity.Enums;
using vms.entity.models;
using vms.Utility;
using vms.utility.StaticData;
using vms.service.Services.SecurityService;

namespace vms.ActionFilter;

public class Logged : IActionFilter
{

    private readonly EnumOperations _enumOperations;
    private readonly EnumObjectType _enumObjectType;
    private IAuditLogService _service { get; }
    public virtual IDictionary<string, object> ActionArguments { get; set; }

    public string previousstate { get; set; }


    private readonly IUnitOfWork _unityOfWork;
    readonly AuditLog audit = new AuditLog();
    public Logged(IAuditLogService service, IUnitOfWork unitOfWork, EnumOperations enumOperations, EnumObjectType enumObjectType)
    {
        _service = service;
        _enumOperations = enumOperations;
        _enumObjectType = enumObjectType;
        _unityOfWork = unitOfWork;

    }
    //public void OnActionExecuting(ActionExecutedContext context)
    //{
    //}
    public void OnActionExecuted(ActionExecutedContext context)
    {
        try
        {
            if (context.Exception == null)
            {
                //var currentObj = JsonConvert.SerializeObject(ActionArguments.Values.FirstOrDefault());
                var currentObj = JObject.Parse(JsonConvert.SerializeObject(ActionArguments.Values.FirstOrDefault()));
                var session = context.HttpContext.Session.GetComplexData<entity.viewModels.VmUserSession>(ControllerStaticData.SESSION);
                var previousdata = JObject.Parse(session.PreviousData);
                var postMethod = (int)((EnumOperations)Enum.Parse(typeof(EnumOperations), _enumOperations.ToString()));
                var primaryKey = _enumObjectType.ToString() + "Id";
                var Id = "";
                var deletedPrimeryKey = 0;
                if (postMethod != 7)
                {
                    //JObject obj = JObject.Parse(changedobj);
                    Id = (string)previousdata[primaryKey];
                }
                else
                {
                    deletedPrimeryKey = Convert.ToInt32(previousdata.ToString());
                }


                AuditLog audit = new AuditLog
                {
                    ObjectTypeId = (int)((EnumObjectType)Enum.Parse(typeof(EnumObjectType), _enumObjectType.ToString())),
                    AuditOperationId = (int)((EnumOperations)Enum.Parse(typeof(EnumOperations), _enumOperations.ToString())),
                    CreatedBy = session.UserId,
                    PrimaryKey = postMethod == 7 ? Convert.ToInt32(deletedPrimeryKey) : Convert.ToInt32(Id),
                    CreatedTime = DateTime.Now,
                    Descriptions = change(session.PreviousData, (string)currentObj), //session.PreviousData, // description size 
                    IsActive = true,
                    Datetime = DateTime.Now,
                    UserId = session.UserId,
                    OrganizationId = session.OrganizationId
                };
                _service.Insert(audit);
                _unityOfWork.SaveChangesAsync();

            }


        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }


    }



    public void OnActionExecuting(ActionExecutingContext context)
    {
        previousstate = JsonConvert.SerializeObject(context.ActionArguments.Values.FirstOrDefault());
        ActionArguments = context.ActionArguments;

    }

    private string change(string sourceJsonString, string targetJsonString)
    {
        //string sourceJsonString = "{'name':'John Doe','age':'25','hitcount':34}";
        //string targetJsonString = "{'name':'John Doe','age':'26','hitcount':30}";

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
                    item.Add(sourceProperty.Key + ":" + sourceProperty.Value + "->" + sourceProperty.Key + targetProp.Value);
                    Console.WriteLine(string.Format("{0} property value is changed", sourceProperty.Key));
                }

            }
        }
        return string.Join(",", item);
    }

    //public bool CompareJson(string expected, string actual)
    //{
    //    var expectedDoc = JsonConvert.DeserializeXmlNode(expected, "root");
    //    var actualDoc = JsonConvert.DeserializeXmlNode(actual, "root");
    //    var diff = new XmlDiff(XmlDiffOptions.IgnoreWhitespace |
    //                           XmlDiffOptions.IgnoreChildOrder);
    //    using (var ms = new MemoryStream())
    //    using (var writer = new XmlTextWriter(ms, Encoding.UTF8))
    //    {
    //        var result = diff.Compare(expectedDoc, actualDoc, writer);
    //        if (!result)
    //        {
    //            ms.Seek(0, SeekOrigin.Begin);
    //            Console.WriteLine(new StreamReader(ms).ReadToEnd());
    //        }
    //        return result;
    //    }
    //}
}