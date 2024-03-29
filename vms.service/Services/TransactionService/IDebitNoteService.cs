﻿using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.service.Services.TransactionService;

public interface IDebitNoteService : IServiceBase<DebitNote>
{
    Task<IEnumerable<ViewDebitNote>> GetDebitNoteData(string orgIdEnc);
    Task<IEnumerable<ViewDebitNote>> GetDebitNoteDataByOrgAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch);
}