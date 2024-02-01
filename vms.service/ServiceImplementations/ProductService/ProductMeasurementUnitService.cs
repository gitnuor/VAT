using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;
using vms.entity.StoredProcedureModel;
using vms.entity.viewModels.ProductMeasurementUnit;
using vms.repository.Repository.tbl;
using vms.service.Services.ProductService;

namespace vms.service.ServiceImplementations.ProductService
{
    public class ProductMeasurementUnitService : ServiceBase<ProductMeasurementUnit>, IProductMeasurementUnitService
    {
        private readonly IProductMeasurementUnitRepository _repository;
        private readonly IMapper _iMapper;

        public ProductMeasurementUnitService(IProductMeasurementUnitRepository repository, IMapper iMapper) :
            base(repository)
        {
            _iMapper = iMapper;
            _repository = repository;
        }

        public async Task<IEnumerable<ProductMeasurementUnitIndexViewModel>> GetProductMeasurementUnits(string orgIdEnc)
        {
            var listData = await _repository.GetProductMeasurementUnits(orgIdEnc);
            return _iMapper.Map<IEnumerable<ProductMeasurementUnitIndexViewModel>>(listData);
        }

        public async Task<IEnumerable<SpGetMeasurementUnitByProductModel>> SpGetMeasurementUnitByProduct(int productId)
        {
            return await _repository.SpGetMeasurementUnitByProduct(productId);
        }

        public async Task<ProductMeasurementUnitIndexViewModel> GetProductMeasurementUnit(string idEnc)
        {
            var data = await _repository.GetProductMeasurementUnit(idEnc);
            return _iMapper.Map<ProductMeasurementUnitIndexViewModel>(data);
        }

        public async Task<SelectList> GetProductMeasurementUnitSelectList(string pOrgId)
        {
            return new(await _repository.GetProductMeasurementUnits(pOrgId),
                nameof(MeasurementUnit.MeasurementUnitId),
                nameof(MeasurementUnit.Name));
        }

        public async Task<SelectList> SpGetMeasurementUnitByProductSelectList(int productId)
        {
            return new(await _repository.SpGetMeasurementUnitByProduct(productId),
                nameof(SpGetMeasurementUnitByProductModel.MeasurementUnitId),
                nameof(SpGetMeasurementUnitByProductModel.MeasurementUnitName));
        }
    }
}