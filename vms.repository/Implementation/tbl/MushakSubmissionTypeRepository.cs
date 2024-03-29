﻿using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class MushakSubmissionTypeRepository : RepositoryBase<MushakSubmissionType>, IMushakSubmissionTypeRepository
{
    private readonly DbContext _context;

    public MushakSubmissionTypeRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}