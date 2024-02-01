using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using vms.entity.models;
using vms.entity.viewModels.ByProductReceive;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService
{
    public class ByProductReceiveService : ServiceBase<ByProductReceive>, IByProductReceiveService
    {
        private readonly IByProductReceiveRepository _repository;
        private readonly IMapper _iMapper;

        public ByProductReceiveService(IByProductReceiveRepository repository, IMapper iMapper) : base(repository)
        {
            _repository = repository;
            _iMapper = iMapper;
        }

        public async Task<IEnumerable<VmByProductReceive>> GetByProductReceiveList(string pOrgId)
        {
            var listData = await _repository.GetByProductReceiveList(pOrgId);
            return _iMapper.Map<IEnumerable<VmByProductReceive>>(listData);

        }

        public async Task<int> InsertByProductReceiveData(VmByProductReceivePostModel vmByProductReceivePostModel)
        {
            var saleId = await _repository.InsertByProductReceiveData(vmByProductReceivePostModel);
            return saleId;
        }
    }
}
