using eXtensionSharp;
using Jina.Domain.Entity.Base;
using Jina.Domain.Example;
using Jina.Domain.Shared;
using Jina.Session.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Jina.Domain.Service.Infra
{
	public static class EntityExtensions
    {
        public static IQueryable<TEntity> vAsNoTrackingQueryable<TEntity>(this DbSet<TEntity> dbSet, ISessionContext ctx)
            where TEntity : TenantEntity
        {
            return dbSet
                .AsNoTracking()
                .Where(m => m.TenantId == ctx.TenantId);
        }
        
        public static async Task<T> vFirstAsync<T>(this IQueryable<T> query, ISessionContext ctx, Expression<Func<T, bool>> predicate = null)
             where T : TenantEntity
        {
            T result = default;

            if (predicate.xIsNotEmpty())
                result = await query.FirstOrDefaultAsync(predicate);
            else
                result = await query.FirstOrDefaultAsync();

            if (ctx.IsDecrypt.xIsTrue())
            {
                if (result.xIsNotEmpty())
                {
                    result.CreatedName = result.CreatedName.vToAESEncrypt();
                    result.LastModifiedName = result.LastModifiedName.vToAESDecrypt();
                }
            }

            return result;
        }

        public static async Task<T2> vFirstAsync<T1, T2>(this IQueryable<T1> query, ISessionContext ctx, Expression<Func<T1, bool>> predicate, Expression<Func<T1, T2>> expression)
             where T1 : TenantEntity
            where T2 : DtoBase
        {
            query = query.AsNoTracking().Where(m => m.TenantId == ctx.TenantId);

            T2 result = default;

            if (predicate.xIsNotEmpty())
                result = await query.Where(predicate).Select(expression).FirstOrDefaultAsync();
            else
                result = await query.Select(expression).FirstOrDefaultAsync();

            if (ctx.IsDecrypt.xIsTrue())
            {
                if (result.xIsNotEmpty())
                {
                    result.CreatedName = result.CreatedName.vToAESDecrypt();
                    result.LastModifiedName = result.LastModifiedName.vToAESDecrypt();
                }
            }

            return result;
        }

        public static List<T> vToList<T>(this IQueryable<T> query, ISessionContext ctx, Expression<Func<T, bool>> predicate = null)
            where T : TenantEntity
        {
            if (predicate.xIsNotEmpty())
            {
                query = query.Where(predicate);
            }
            
            var list = query
                .AsNoTracking()
                .Where(m => m.TenantId == ctx.TenantId)
                .OrderByDescending(m => m.CreatedOn)
                .ToList();
            
            if (ctx.IsDecrypt.xIsTrue())
            {
                list.ForEach(item =>
                {
                    item.CreatedName = item.CreatedName.vToAESDecrypt();
                    item.LastModifiedName = item.LastModifiedName.vToAESDecrypt();
                });
            }

            return list;
        }

        public static async Task<List<T>> vToListAsync<T>(this IQueryable<T> query, ISessionContext ctx, 
            Expression<Func<T, bool>> predicate = null)
            where T : TenantEntity
        {
            if (predicate.xIsNotEmpty())
            {
                query = query.Where(predicate);
            }

            var list = await query
                .AsNoTracking()
                .Where(m => m.TenantId == ctx.TenantId)
                .OrderByDescending(m => m.CreatedOn)
                .ThenByDescending(m => m.LastModifiedOn)
                .ToListAsync();
            
            if (ctx.IsDecrypt.xIsTrue())
            {
                list.ForEach(item =>
                {
                    item.CreatedName = item.CreatedName.vToAESDecrypt();
                    item.LastModifiedName = item.LastModifiedName.vToAESDecrypt();
                });
            }

            return list;
        }

        #region [offset paging]

        public static async Task<PaginatedResult<T>> vToPaginatedListAsync<T>(this IQueryable<T> query
            , ISessionContext ctx, int pageNumber, int pageSize)
            where T : TenantEntity
        {
            if (query.xIsEmpty()) throw new Exception("queriable is empty");
            if (ctx.xIsEmpty()) throw new Exception("context is empty");

            pageNumber = pageNumber == 0 ? 1 : pageNumber + 1;
            pageSize = pageSize == 0 ? 10 : pageSize;
            int count = await query.AsNoTracking().CountAsync();
            List<T> items = await query
                    .Where(m => m.TenantId == ctx.TenantId)
                    .OrderByDescending(m => m.CreatedOn)
                    .ThenByDescending(m => m.LastModifiedOn)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)      
                    .AsNoTracking()
                    .ToListAsync();
            
            if (ctx.IsDecrypt.xIsTrue())
            {
                items.ForEach(item =>
                {
                    item.CreatedName = item.CreatedName.vToAESDecrypt();
                    item.LastModifiedName = item.LastModifiedName.vToAESDecrypt();
                });
            }

            return await PaginatedResult<T>.SuccessAsync(items, count, pageNumber, pageSize);
        }

        public static async Task<PaginatedResult<T>> vToPaginatedListAsync<T, TRequest>(this IQueryable<T> query, ISessionContext ctx, TRequest request)
            where T : TenantEntity
            where TRequest : PaginatedRequest
        {
            if (query == null) throw new Exception("queriable is empty");
            if (ctx.xIsEmpty()) throw new Exception("context is empty");

            var pageNo = request.PageNo == 0 ? 1 : request.PageNo + 1;
            var pageSize = request.PageSize == 0 ? 10 : request.PageSize;
            int count = await query.AsNoTracking().CountAsync();

            List<T> items = null;

            if (request.SortName.xIsEmpty())
            {
                items = await query
                    .AsNoTracking()
                    .Where(m => m.TenantId == ctx.TenantId)
                    .Where(m => m.IsActive == request.IsActive)
                    .OrderByDescending(m => m.CreatedOn)
                    .ThenByDescending(m => m.LastModifiedOn)
                    .Skip((pageNo - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();                
            }
            else
            {
                items = await query
                    .AsNoTracking()
                    .Where(m => m.TenantId == ctx.TenantId)
                    .Where(m => m.IsActive == request.IsActive)
                    .OrderBy($"{request.SortName} {request.OrderBy}")
                    .Skip((pageNo - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();                
            }

            if (ctx.IsDecrypt.xIsTrue())
            {
                items.ForEach(item =>
                {
                    item.CreatedName = item.CreatedName.vToAESDecrypt();
                    item.LastModifiedName = item.LastModifiedName.vToAESDecrypt();
                });
            }

            return await PaginatedResult<T>.SuccessAsync(items, count, pageNo, pageSize);
        }

        public static async Task<PaginatedResult<T2>> vToPaginatedListAsync<T1, T2, TRequest>(this IQueryable<T1> query, ISessionContext ctx, TRequest request, Expression<Func<T1, T2>> expression)
            where T1 : TenantEntity
            where T2 : DtoBase
            where TRequest : PaginatedRequest
        {
            if (query.xIsEmpty()) throw new Exception("queriable is empty");
            if (ctx.xIsEmpty()) throw new Exception("context is empty");

            var pageNo = request.PageNo == 0 ? 1 : request.PageNo + 1;
            var pageSize = request.PageSize == 0 ? 10 : request.PageSize;
            int count = await query.CountAsync();

            List<T2> items = null;

            if (request.SortName.xIsEmpty())
            {
                items = await query
                    .AsNoTracking()
                    .Where(m => m.IsActive == request.IsActive)
                    .Where(m => m.TenantId == ctx.TenantId)
                    .OrderByDescending(m => m.CreatedOn)
                    .ThenByDescending(m => m.LastModifiedOn)
                    .Skip((pageNo - 1) * pageSize)
                    .Take(pageSize)                    
                    .Select(expression)
                    .ToListAsync();
            }
            else
            {
                items = await query                        
                    .AsNoTracking()
                    .Where(m => m.IsActive == request.IsActive)
                    .Where(m => m.TenantId == ctx.TenantId)
                    .OrderBy($"{request.SortName} {request.OrderBy}")
                    .Skip((pageNo - 1) * pageSize)
                    .Take(pageSize)
                    .Select(expression)
                    .ToListAsync();
            }

            if (ctx.IsDecrypt.xIsTrue())
            {
                items.ForEach(item =>
                {
                    item.CreatedName = item.CreatedName.vToAESDecrypt();
                    item.LastModifiedName = item.LastModifiedName.vToAESDecrypt();
                });
            }

            return await PaginatedResult<T2>.SuccessAsync(items, count, pageNo, pageSize);
        }        

        #endregion

        #region [cursor paging]

        public static async Task<CursorResult<T>> vToPaginatedCursorListAsync<T, TRequest>(
            this IQueryable<T> query, ISessionContext ctx, TRequest request)
            where T : NumberEntity
            where TRequest : CursorRequest
        {
            if (query.xIsEmpty()) throw new Exception("query is empty");
            if (ctx.xIsEmpty()) throw new Exception("context is empty");

            var pageSize = request.PageSize == 0 ? 10 : request.PageSize;

            List<T> items = null;

            if (request.SortName.xIsEmpty())
            {
                items = await query
                    .AsNoTracking()
                    .Where(m => m.TenantId == ctx.TenantId)
                    .Where(m => m.IsActive == request.IsActive)
                    .Where(m => m.Id >= request.Cursor)
                    .Take(request.PageSize + 1)                    
                    .OrderByDescending(m => m.CreatedOn)
                    .ThenByDescending(m => m.LastModifiedOn)
                    .ToListAsync();                
            }
            else
            {
                items = await query.AsNoTracking()
                    .Where(m => m.TenantId == ctx.TenantId)
                    .Where(m => m.IsActive == request.IsActive)                        
                    .Where(m => m.Id >= request.Cursor)
                    .Take(request.PageSize + 1)                        
                    .OrderBy($"{request.SortName} {request.OrderBy}")
                    .ToListAsync();                
            }

            if (ctx.IsDecrypt.xIsTrue())
            {
                items.ForEach(item =>
                {
                    item.CreatedName = item.CreatedName.vToAESDecrypt();
                    item.LastModifiedName = item.LastModifiedName.vToAESDecrypt();
                });
            }

            var cursor = items[^1].Id;

            return await CursorResult<T>.SuccessAsync(items, cursor, pageSize);
        }
        
        public static async Task<CursorResult<T2>> vToPaginatedCursorListAsync<T1, T2, TRequest>(this IQueryable<T1> query, ISessionContext ctx, TRequest request, Expression<Func<T1, T2>> expression)
            where T1 : NumberEntity
            where T2 : NumberDtoBase
            where TRequest : CursorRequest
        {
            if (query == null) throw new Exception("queriable is empty");
            if (ctx.xIsEmpty()) throw new Exception("context is empty");

            var pageSize = request.PageSize == 0 ? 10 : request.PageSize;
            int count = await query.CountAsync();

            List<T2> items = null;

            if (request.SortName.xIsEmpty())
            {
                items = await query.AsNoTracking()
                    .Where(m => m.TenantId == ctx.TenantId)
                    .Where(m => m.IsActive == request.IsActive)
                    .Where(m => m.Id >= request.Cursor)
                    .Take(pageSize + 1)
                    .OrderByDescending(m => m.CreatedOn)
                    .ThenByDescending(m => m.LastModifiedOn)
                    .Select(expression)
                    .ToListAsync();
            }
            else
            {
                items = await query.AsNoTracking()
                    .Where(m => m.TenantId == ctx.TenantId)
                    .Where(m => m.IsActive == request.IsActive)
                    .Where(m => m.Id >= request.Cursor)
                    .Take(pageSize + 1)                        
                    .OrderBy($"{request.SortName} {request.OrderBy}")
                    .Select(expression)
                    .ToListAsync();
            }

            if (ctx.IsDecrypt.xIsTrue())
            {
                items.ForEach(item =>
                {
                    item.CreatedName = item.CreatedName.vToAESDecrypt();
                    item.LastModifiedName = item.LastModifiedName.vToAESDecrypt();
                });
            }
            
            var cursor = items[^1].Id;

            return await CursorResult<T2>.SuccessAsync(items, cursor, pageSize);
        }

        #endregion
    }
}