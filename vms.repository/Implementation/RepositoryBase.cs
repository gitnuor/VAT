using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using TrackableEntities.Common.Core;
using URF.Core.EF.Trackable;
using vms.repository.Repository;

// Example: extending IRepository<TEntity> and/or ITrackableRepository<TEntity>, scope: application-wide across all IRepositoryX<TEntity>
namespace vms.repository.Implementation;
public class RepositoryBase<TEntity> : TrackableRepository<TEntity>, IRepositoryBase<TEntity> where TEntity : class, ITrackable
{
    private readonly DbContext _context;

    public RepositoryBase(DbContext context) : base(context)
    {
        _context = context;
    }
    // Example: adding synchronous Find, scope: application-wide
    public TEntity Find(object[] keyValues, CancellationToken cancellationToken)
    {
        return Context.Find<TEntity>(keyValues) as TEntity;
    }

    public async Task<List<T>> CreateQuery<T>(string sql, CancellationToken cancellationToken)
    {
        var items = new List<T>();
        await using (var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
        {
            var command = connection.CreateCommand();
            command.CommandText = sql;
            await connection.OpenAsync(cancellationToken);
            System.Reflection.PropertyInfo[] properties = typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            await using (var reader = await command.ExecuteReaderAsync(cancellationToken))
            {
                while (await reader.ReadAsync(cancellationToken))
                {
                    var item = Activator.CreateInstance<T>();
                    foreach (System.Reflection.PropertyInfo property in properties)
                    {
                        try
                        {
                            if (property.PropertyType.Name == "Nullable`1")
                                property.SetValue(item, Convert.ChangeType(reader[property.Name], Nullable.GetUnderlyingType(property.PropertyType)), null);
                            else
                                property.SetValue(item, Convert.ChangeType(reader[property.Name], property.PropertyType), null);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }

                    }
                    items.Add(item);
                }
            }
        }

        return await Task.FromResult(items);
    }

    public void CustomInsert(TEntity item)
    {
        _context.Add(item);
    }

    public async Task CustomInsertAsync(TEntity item)
    {
        await _context.AddAsync(item);
    }

    public void BulkInsert(IEnumerable<TEntity> items)
    {
        _context.AddRange(items);
    }

    public async Task BulkInsertAsync(IEnumerable<TEntity> items)
    {
        await _context.BulkInsertAsync((IList<TEntity>)items);
    }

    public void BulkDelete(IEnumerable<TEntity> items)
    {
        _context.RemoveRange(items);
    }
}