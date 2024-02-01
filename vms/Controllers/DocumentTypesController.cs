using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using vms.entity.models;
using vms.Utility;
using vms.entity.viewModels;
using vms.utility.StaticData;
using Newtonsoft.Json;
using vms.entity.Enums;
using Microsoft.AspNetCore.DataProtection;
using vms.service.Services.UploadService;

namespace vms.Controllers;

public class DocumentTypesController : ControllerBase
{

    private readonly IDocumentTypeService _service;
    public DocumentTypesController(ControllerBaseParamModel controllerBaseParamModel, IDocumentTypeService service) : base(controllerBaseParamModel)
    {
        _service = service;
    }


        
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_DOCUMENT_TYPE_CAN_VIEW)]
    public async Task<IActionResult> Index(int? page, string search = null)
    {
        var documentTypes = await _service.GetDocumentTypes(UserSession.OrganizationId);

        documentTypes.ToList().ForEach(delegate (DocumentType doc)
        {
            doc.EncryptedId = IvatDataProtector.Protect(doc.DocumentTypeId.ToString());
                
        });
        return View(documentTypes);
    }


    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_DOCUMENT_TYPE_CAN_VIEW)]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var documentType = await _service.Query().Include(d => d.Organization)
            .SingleOrDefaultAsync(m => m.DocumentTypeId == id, CancellationToken.None);
        if (documentType == null)
        {
            return NotFound();
        }

        return View(documentType);
    }

        
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_DOCUMENT_TYPE_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_DOCUMENT_TYPE_CAN_ADD)]
    public IActionResult Create()
    {
           
        return View();
    }

    [HttpPost]
         
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_DOCUMENT_TYPE_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_DOCUMENT_TYPE_CAN_ADD)]
    public async Task<IActionResult> Create(DocumentType documentType)
    {
        if (ModelState.IsValid)
        {
            documentType.CreatedBy = UserSession.UserId;
            documentType.CreatedTime = DateTime.Now;
            documentType.IsActive = true;
            documentType.OrganizationId = UserSession.OrganizationId;

            _service.Insert(documentType);
            await UnitOfWork.SaveChangesAsync();
            var jObj = JsonConvert.SerializeObject(documentType, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });


            AuditLog au = new AuditLog
            {
                ObjectTypeId = (int)EnumObjectType.DocumentType,
                PrimaryKey = documentType.DocumentTypeId,
                AuditOperationId = (int)EnumOperations.Add,
                UserId = UserSession.UserId,
                Datetime = DateTime.Now,
                Descriptions = jObj.ToString(),
                IsActive = true,
                CreatedBy = UserSession.UserId,
                CreatedTime = DateTime.Now,
                OrganizationId = UserSession.OrganizationId
            };
            await AuditLogCreate(au);
            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
            return RedirectToAction(nameof(Index));
        }
        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
        return View(documentType);
    }

        
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_DOCUMENT_TYPE_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_DOCUMENT_TYPE_CAN_EDIT)]
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var documentType = await _service.GetDocumentType(id);
                
        if (documentType == null)
        {
            return NotFound();
        }
        documentType.EncryptedId = id;
        return View(documentType);
    }

    [HttpPost]
         
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_DOCUMENT_TYPE_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_DOCUMENT_TYPE_CAN_EDIT)]
    public async Task<IActionResult> Edit(string id, DocumentType documentType)
    {
        int documentTypeId = int.Parse(IvatDataProtector.Unprotect(id));

        if (documentTypeId != documentType.DocumentTypeId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var documentData = await _service.Query().Include(d => d.Organization)
                    .SingleOrDefaultAsync(m => m.DocumentTypeId == documentTypeId, CancellationToken.None);
                var prevObj = JsonConvert.SerializeObject(documentData, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                documentData.Name = documentType.Name;
                _service.Update(documentData);
                await UnitOfWork.SaveChangesAsync();
                var jObj = JsonConvert.SerializeObject(documentData, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });


                AuditLog au = new AuditLog
                {
                    ObjectTypeId = (int)EnumObjectType.DocumentType,
                    PrimaryKey = documentType.DocumentTypeId,
                    AuditOperationId = (int)EnumOperations.Edit,
                    UserId = UserSession.UserId,
                    Datetime = DateTime.Now,
                    Descriptions = GetChangeInformation(prevObj.ToString(), jObj.ToString()),
                    IsActive = true,
                    CreatedBy = UserSession.UserId,
                    CreatedTime = DateTime.Now,
                    OrganizationId = UserSession.OrganizationId
                };
                await AuditLogCreate(au);
                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.UPDATE_CLASSNAME
                    ;
            }

			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return RedirectToAction(nameof(Index));
        }
          
        return View(documentType);
    }

        
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_DOCUMENT_TYPE_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_DOCUMENT_TYPE_CAN_DELETE)]
    public async Task<IActionResult> ChangeDocumentTypeStatus(string id)
    {
        var damage = await _service.Query().SingleOrDefaultAsync(p => p.DocumentTypeId == int.Parse(IvatDataProtector.Unprotect(id)), CancellationToken.None);

        if (damage.IsActive == true)
        {
            damage.IsActive = false;
            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.DELETE_CLASSNAME;

        }
        else
        {
            damage.IsActive = true;
            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ACTIVE_CLASSNAME;

        }
        _service.Update(damage);
        await UnitOfWork.SaveChangesAsync();
        AuditLog au = new AuditLog
        {
            ObjectTypeId = (int)EnumObjectType.DocumentType,
            PrimaryKey = damage.DocumentTypeId,
            AuditOperationId = (int)EnumOperations.Delete,
            UserId = UserSession.UserId,
            Datetime = DateTime.Now,
            Descriptions = "IsActive:0",
            IsActive = true,
            CreatedBy = UserSession.UserId,
            CreatedTime = DateTime.Now,
            OrganizationId = UserSession.OrganizationId
        };
        await AuditLogCreate(au);
        return RedirectToAction(nameof(Index));
    }

       
}