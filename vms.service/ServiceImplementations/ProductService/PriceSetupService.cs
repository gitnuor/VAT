using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.ProductService;

namespace vms.service.ServiceImplementations.ProductService;

public class PriceSetupService : ServiceBase<PriceSetup>, IPriceSetupService
{
    private readonly IPriceSetupRepository _repository;
    private readonly IMapper _mapper;

    public PriceSetupService(IPriceSetupRepository repository, IMapper mapper) : base(repository)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task InsertProductPriceAsync(VmInputOutputCoEfficientPost vmInputOutputCoEfficientPost,
        VmUserSession userSession)
    {
        try
        {
            var priceSetup = _mapper.Map<VmInputOutputCoEfficientPost, PriceSetup>(vmInputOutputCoEfficientPost);
            var ocs = _mapper.Map<IEnumerable<InputOutputCoEfficientProductCost>, IEnumerable<PriceSetupProductCost>>(
                vmInputOutputCoEfficientPost.ProductCostList);
            var priceSetupProductCosts =
                _mapper.Map<IEnumerable<InputOutputCoEfficientOverheadCost>, IEnumerable<PriceSetupProductCost>>(
                    vmInputOutputCoEfficientPost.OverheadCostList);

            foreach (var item in ocs)
            {
                item.IsRawMaterial = true;
                priceSetup.PriceSetupProductCosts.Add(item);
            }

            // foreach (var item in priceSetupProductCosts)
            // {
            //     priceSetup.PriceSetupProductCosts.Add(item);
            // }


            priceSetup.CreatedTime = DateTime.Now;
            priceSetup.EffectiveFrom = DateTime.Now;
            priceSetup.IsActive = true;
            priceSetup.CreatedBy = userSession.UserId;
            priceSetup.OrganizationId = userSession.OrganizationId;
            priceSetup.ProductId = vmInputOutputCoEfficientPost.HiddenProductId;
            priceSetup.EffectiveTo = null;
            await _repository.CustomInsertAsync(priceSetup);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IEnumerable<PriceSetup>> GetPriceSetupByProductId(int productId)
    {
        return await _repository.Query().Where(x => x.ProductId == productId).Include(x => x.PriceSetupProductCosts)
            .SelectAsync();
    }

    public async Task<IEnumerable<ViewInputOutputCoEfficient>> GetInputOutputCoEfficientReportData(string organizationId)
    {
        return await _repository.GetInputOutputCoEfficientReportData(organizationId);
    }
}