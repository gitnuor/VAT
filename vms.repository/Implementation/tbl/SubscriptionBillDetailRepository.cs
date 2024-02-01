using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class SubscriptionBillDetailRepository
	(DbContext context) : RepositoryBase<SubscriptionBillDetail>(context), ISubscriptionBillDetailRepository;