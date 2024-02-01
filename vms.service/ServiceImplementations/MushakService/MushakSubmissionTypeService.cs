﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.MushakService;

namespace vms.service.ServiceImplementations.MushakService;

public class MushakSubmissionTypeService : ServiceBase<MushakSubmissionType>, IMushakSubmissionTypeService
{
    private readonly IMushakSubmissionTypeRepository _repository;

    public MushakSubmissionTypeService(IMushakSubmissionTypeRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<MushakSubmissionType>> Get(int organizationId, string searchQuery = null)
    {
        var queryableResult = _repository.Queryable().Where(q => q.MushakSubmissionTypeId == organizationId);

        //if (string.IsNullOrEmpty(searchQuery)) return await queryableResult.ToListAsync(CancellationToken.None);

        //searchQuery = searchQuery.ToLower();
        //queryableResult = queryableResult.Where(q =>
        //    q.Amount.ToString(CultureInfo.InvariantCulture).Contains(searchQuery)
        //    || q.Year.ToString(CultureInfo.InvariantCulture).Contains(searchQuery)
        //    || CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(q.Month).ToLower().Contains(searchQuery)
        //    || q.Description.ToLower().Contains(searchQuery)
        //);

        return await queryableResult.ToListAsync(CancellationToken.None);
    }
}