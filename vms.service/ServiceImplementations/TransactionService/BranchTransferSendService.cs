using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using vms.entity.models;
using vms.entity.viewModels.BranchTransferSend;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class BranchTransferSendService : ServiceBase<BranchTransferSend>, IBranchTransferSendService
{
    private readonly IBranchTransferSendRepository _repository;
    private readonly IMapper _mapper;
    public BranchTransferSendService(IBranchTransferSendRepository repository, IMapper mapper) : base(repository)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BranchTransferSend>> GetBranchTransferSendsByOrganization(string orgIdEnc)
    {
        return await _repository.GetBranchTransferSendsByOrganization(orgIdEnc);
    }

    public Task<int> InsertBranchTransferSend(BranchTransferSendCreatePostViewModel model)
    {
        var trans = _mapper.Map<BranchTransferSendCreatePostViewModel, BranchTransferSendParamViewModel>(model);
        trans.IsComplete = true;
        trans.CreatedTime = DateTime.Now;
        return _repository.InsertBranchTransferSend(trans);
    }

    public Task<SpGetBranchTransferChallanModel> GetBranchTransferChallan(string transferIdEnc, int userId)
    {
        return _repository.GetBranchTransferChallan(transferIdEnc, userId);
    }
}