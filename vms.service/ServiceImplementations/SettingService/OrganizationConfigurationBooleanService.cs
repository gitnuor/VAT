using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;

namespace vms.service.ServiceImplementations.SettingService
{
    public class OrganizationConfigurationBooleanService : ServiceBase<ViewOrganizationConfigurationBoolean>, IOrganizationConfigurationBooleanService
    {
        private readonly IOrganizationConfigurationBooleanRepository _repository;

        public OrganizationConfigurationBooleanService(IOrganizationConfigurationBooleanRepository repository, IMapper mapper) : base(repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ViewOrganizationConfigurationBoolean>> GetOrganizationConfigurationBoolean(string orgIdEnc)
        {
            return await _repository.GetOrganizationConfigurationBoolean(orgIdEnc);
        }

    }
}
