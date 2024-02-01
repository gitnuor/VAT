using Microsoft.Extensions.Hosting;
using URF.Core.Abstractions;
using vms.entity.models;
using vms.service.Services.ProductService;

namespace vms.scheduled.service
{
    public class TimedHostedService : BackgroundService
	{
		private readonly IBrandService _brandService;
		private readonly IUnitOfWork _unitOfWork;

		public TimedHostedService(IUnitOfWork unitOfWork, IBrandService brandService)
		{
			_unitOfWork = unitOfWork;
			_brandService = brandService;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			using PeriodicTimer timer = new(TimeSpan.FromMinutes(5));

			try
			{
				var brand = new Brand
				{
					OrganizationId = 7,
					Name = "Test Brand",
					NameInBangla = "Test Brand"
				};
				_brandService.Insert(brand);
				await _unitOfWork.SaveChangesAsync(stoppingToken);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
	}
}