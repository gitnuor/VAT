using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<IEnumerable<User>> GetUsers(int orgIdEnc);
    Task<User> GetUser(string idEnc);
    Task<User> GetUserByUsername(string userName);
    Task<User> GetUserByUsernameAndEncPass(string userName, string encPassword);
    Task<IEnumerable<User>> GetAllByOrganization(string encOrganizationId);
    Task<IEnumerable<ViewUser>> GetAllUserByOrganization(string encOrganizationId);
    Task<List<ViewUser>> GetAllUserByOrganization(int orgId);
    Task<ViewUser> GetUserByReference(string referenceId, int orgId);
}