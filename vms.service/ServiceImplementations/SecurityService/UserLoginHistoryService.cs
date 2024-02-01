using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.SecurityService;

namespace vms.service.ServiceImplementations.SecurityService;

public class UserLoginHistoryService : ServiceBase<UserLoginHistory>, IUserLoginHistoryService
{
    private readonly IUserLoginHistoryRepository _repository;

    public UserLoginHistoryService(IUserLoginHistoryRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<UserLoginHistory>> GetByOrganizationAndUser(int organizationId, int userId,
        DateTime fromDate, DateTime toDate)
    {
        List<UserLoginHistory> loginHistory;

        if (userId != 0)
        {
            loginHistory = (await Query().Where(u =>
                    u.UserId == userId
                    && u.OrganizationId == organizationId
                    && u.LoginTime >= fromDate
                    && u.LoginTime < toDate.AddDays(1))
                .Include(u => u.User)
                .SelectAsync()).ToList();
        }
        else
        {
            loginHistory = (await Query().Where(u =>
                    u.OrganizationId == organizationId
                    && u.LoginTime >= fromDate
                    && u.LoginTime < toDate.AddDays(1))
                .Include(u => u.User)
                .SelectAsync()).ToList();
        }

        return loginHistory;
    }
}