using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class CustomerSubscriptionDetailRepository
    (DbContext context) : RepositoryBase<CustomerSubscriptionDetail>(context), ICustomerSubscriptionDetailRepository;