using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.service.Services.SecurityService;

public interface IUserLoginHistoryService : IServiceBase<UserLoginHistory>
{
    Task<IEnumerable<UserLoginHistory>> GetByOrganizationAndUser(int organizationId, int userId, DateTime fromDate,
        DateTime toDate);
}