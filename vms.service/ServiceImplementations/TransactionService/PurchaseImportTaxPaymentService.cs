﻿using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class PurchaseImportTaxPaymentService : ServiceBase<PurchaseImportTaxPayment>, IPurchaseImportTaxPaymentService
{
    private readonly IPurchaseImportTaxPaymentRepository _repository;
    public PurchaseImportTaxPaymentService(IPurchaseImportTaxPaymentRepository repository) : base(repository)
    {
        _repository = repository;
    }
}