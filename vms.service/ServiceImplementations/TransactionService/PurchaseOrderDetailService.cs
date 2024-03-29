﻿using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class PurchaseOrderDetailService : ServiceBase<PurchaseDetail>, IPurchaseOrderDetailService
{
    private readonly IPurchaseOrderDetailsRepository _repository;
    public PurchaseOrderDetailService(IPurchaseOrderDetailsRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PurchaseDetail>> GetPurchaseDetails(string idEnc)
    {
        return await _repository.GetPurchaseDetails(idEnc);

    }
}