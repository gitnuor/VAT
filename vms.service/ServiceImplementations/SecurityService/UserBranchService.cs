using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.SecurityService;

namespace vms.service.ServiceImplementations.SecurityService
{
    public class UserBranchService : ServiceBase<UserBranch>, IUserBranchService
    {
        private readonly IUserBranchRepository _repository;
        public UserBranchService(IUserBranchRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
