using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.viewModels.CustomerViewModel;
using vms.entity.viewModels;
using vms.entity.viewModels.User;
using vms.entity.Dto.User;

namespace vms.service.Services.SecurityService;

public interface IUserService : IServiceBase<User>
{
    Task<IEnumerable<User>> GetUsers(int orgIdEnc);
    Task<IEnumerable<ViewUser>> GetAllByOrganization(string encOrganizationId);
    Task<IEnumerable<User>> GetAllByOrganization(string encOrganizationId, EnumUserStatus userStatus);
    Task<IEnumerable<User>> GetAllActiveByOrganization(string encOrganizationId);
    Task<IEnumerable<User>> GetAllInactiveByOrganization(string encOrganizationId);
    Task<User> GetUser(string idEnc);
    Task<bool> IsUserExists(string userName);
    Task<User> GetUserByUsername(string userName);
    Task<User> GetUserByUsernameAndEncPass(string userName, string encPassword);
    Task AddFailedCount(int id);
    Task AddFailedCountAndLockUser(int id);
    Task ResetFailedCount(int id);
    Task ResetFailedCountAndUpdateLoginTime(int id);
    Task UpdateLoginTime(int id);
    Task InactivateUser(int id, int inactivateByUserId, string reason);
    Task LockUser(int id, string reason);
    Task<User> GetUserByOrgAndId(int id, int orgId);
    Task<IEnumerable<CustomSelectListItem>> GetAllByOrganizationSelectList(string pOrgId);
    Task InsertUserBranches(IEnumerable<UserBranchCreateViewModel> userBranches, int orgId, int userId);
    Task InsertUserDataFromApi(UserPostDto userPostDto, string appKey, string userPassword);
	Task<IEnumerable<UserDto>> GetAllForApi(string encOrganizationId);
}