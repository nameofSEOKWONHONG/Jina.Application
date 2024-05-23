using eXtensionSharp;
using Jina.Base.Service.Abstract;
using Jina.Domain.Entity.Common;
using Jina.Session.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Service.Infra.Services;

public interface ISequenceService : IScopeService
{
    Task<int> GetNextNumber(string tableName);
}

public class SequenceService : ISequenceService
{
    private readonly ISessionContext _ctx;
    public SequenceService(ISessionContext ctx)
    {
        _ctx = _ctx;
    }

    public async Task<int> GetNextNumber(string tableName)
    {
        if (tableName.xIsEmpty()) throw new Exception("Table name is empty");

        var db = this._ctx.DbContext.xAs<AppDbContext>();
        if (db.Database.CurrentTransaction.xIsEmpty()) throw new Exception("Transaction is empty");
        
        var exist = await db.Sequences.FirstOrDefaultAsync(m => m.TableName == tableName);
        if (exist.xIsEmpty())
        {
            exist = new Sequence()
            {
                TenantId = this._ctx.TenantId,
                TableName = tableName,
                NextValue = 1
            };
            db.Sequences.Add(exist);
        }
        else
        {
            exist.NextValue += 1;
            db.Sequences.Update(exist);
        }

        await db.SaveChangesAsync();
        return exist.NextValue;
    }
}