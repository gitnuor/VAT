using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class DamageService : ServiceBase<Damage>, IDamageService
{
    private readonly IMapper _mapper;
    private readonly IDamageRepository _repository;
	private readonly IDataProtectionProvider _protectionProvider;
	private readonly PurposeStringConstants _purposeStringConstants;
	private IDataProtector _dataProtector;

	public DamageService(IDamageRepository repository, IMapper mapper, IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants) : base(repository)
    {
        _repository = repository;
        _mapper = mapper;
		_protectionProvider = p_protectionProvider;
		_purposeStringConstants = p_purposeStringConstants;
		_dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
	}

    public async Task<IEnumerable<Damage>> GetDamage(int orgIdEnc)
    {
        var damages = await _repository.GetDamage(orgIdEnc);
		damages.ToList().ForEach(delegate (Damage damage)
		{
			damage.EncryptedId = _dataProtector.Protect(damage.DamageId.ToString());
		});
        return damages;
	}

    public async Task<Damage> GetDamage(string idEnc)
    {
        return await _repository.GetDamage(idEnc);
    }


    public async Task<Damage> GetDamagewithDetails(int orgId, int damageId)
    {
        return await _repository.GetDamagewithDetails(orgId, damageId);
		
	}

    public void InsertPurchaseDamage(VmPurchaseDamagePost vmPurchaseDamagePost, VmUserSession userSession)
    {
        try
        {
            var damageInsertData = _mapper.Map<VmPurchaseDamagePost, Damage>(vmPurchaseDamagePost);
            damageInsertData.CreatedBy = userSession.UserId;
            damageInsertData.CreatedTime = DateTime.Now;
            damageInsertData.OrganizationId = userSession.OrganizationId;
            damageInsertData.DamageTypeId = (int)EnumDamageType.AccidentDamage;
            damageInsertData.IsActive = true;
            damageInsertData.PreparedOn = DateTime.Now;
            _repository.CustomInsert(damageInsertData);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

    public void InsertSalesDamage(VmSalesDamagePost vmSalesDamagePost, VmUserSession userSession)
    {
        try
        {
            var damageInsertData = _mapper.Map<VmSalesDamagePost, Damage>(vmSalesDamagePost);
            damageInsertData.CreatedBy = userSession.UserId;
            damageInsertData.CreatedTime = DateTime.Now;
            damageInsertData.OrganizationId = userSession.OrganizationId;
            damageInsertData.DamageTypeId = (int)EnumDamageType.AccidentDamage;
            damageInsertData.IsActive = true;
            damageInsertData.PreparedOn = DateTime.Now;
            _repository.CustomInsert(damageInsertData);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}


    public void InsertDamage(VmCombineDamagePost vmSalesDamagePost, VmUserSession userSession)
    {
        try
        {
            var damageInsertData = _mapper.Map<VmCombineDamagePost, Damage>(vmSalesDamagePost);
            damageInsertData.CreatedBy = userSession.UserId;
            damageInsertData.CreatedTime = DateTime.Now;
            damageInsertData.OrganizationId = userSession.OrganizationId;
            damageInsertData.IsActive = true;
            damageInsertData.PreparedOn = DateTime.Now;
            _repository.CustomInsert(damageInsertData);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}