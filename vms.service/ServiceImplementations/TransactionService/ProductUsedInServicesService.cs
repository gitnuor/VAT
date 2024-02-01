using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels.ByProductReceive;
using vms.entity.viewModels.ProductUsedInService;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService
{
    public class ProductUsedInServicesService : ServiceBase<ProductUsedInService>, IProductUsedInServicesService
    {
        private readonly IProductUsedInServicesRepository _repository;
        private readonly IMapper _iMapper;

        public ProductUsedInServicesService(IProductUsedInServicesRepository repository, IMapper iMapper) : base(repository)
        {
            _repository = repository;
            _iMapper = iMapper;
        }

        public async Task<IEnumerable<VmProductUsedInService>> GetProductUsedInServiceList(string pOrgId)
        {
            var listData = await _repository.GetProductUsedInServiceList(pOrgId);
            return _iMapper.Map<IEnumerable<VmProductUsedInService>>(listData);

        }

        public async Task<int> InsertProductUsedInServiceData(VmProductUsedInServicePostModel vmProductUsedInServicePostModel)
        {
            var saleId = await _repository.InsertProductUsedInServiceData(vmProductUsedInServicePostModel);
            return saleId;
        }
    }
}
