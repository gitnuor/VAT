using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class ContentRepository : RepositoryBase<Content>, IContentRepository
{
	private readonly DbContext _context;

	public ContentRepository(DbContext context) : base(context)
	{
		_context = context;
	}

	public async Task<IEnumerable<ViewUploadedContent>> GetContentByObject(int objectId, int objectPrimaryKey)
	{
		return await _context.Set<ViewUploadedContent>()
			.Where(x => x.ObjectId == objectId
			            && x.ObjectPrimaryKey == objectPrimaryKey)
			.AsNoTracking()
			.ToListAsync();
	}
}