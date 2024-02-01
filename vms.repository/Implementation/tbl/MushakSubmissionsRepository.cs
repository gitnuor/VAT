using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class MushakSubmissionsRepository : RepositoryBase<MushakSubmission>, IMushakSubmissionsRepository
{
    private readonly DbContext _context;

    public MushakSubmissionsRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}