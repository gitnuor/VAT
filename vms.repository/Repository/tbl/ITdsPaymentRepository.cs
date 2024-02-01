using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.StoredProcedureModel.MushakReturn;
using vms.entity.viewModels.Payment;

namespace vms.repository.Repository.tbl;

public interface ITdsPaymentRepository : IRepositoryBase<TdsPayment>
{
	Task<List<SpGetTdsPaymentChallan>> TdsPaymentChallan(int tdsPaymentId);
	Task<int> InsertTdsPayment(VmTdsPaymentPost tdsPayment);

	IQueryable<ViewTdsPayment> GetTdsPayments(int organizationId);
	IQueryable<ViewTdsPayment> GetTdsPayments();
	Task<ViewTdsPayment> GetTdsPayment(int paymentId);
}