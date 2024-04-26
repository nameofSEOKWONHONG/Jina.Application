using eXtensionSharp;
using Jina.Domain.Entity.Base;
using Jina.Domain.Example;
using Jina.Domain.Shared;
using Jina.Domain.Shared.Consts;
using Jina.Session.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Jina.Domain.Service.Infra
{
	public static class EntityExtensions
    {
        public static async Task<T> vFirstAsync<T>(this IQueryable<T> query, ISessionContext ctx, Expression<Func<T, bool>> predicate = null)
             where T : TenantBase
        {
            query = query.Where(m => m.TenantId == ctx.TenantId);
            T result = default;

            if (predicate.xIsNotEmpty())
                result = await query.AsNoTracking().FirstOrDefaultAsync(predicate);
            else
                result = await query.AsNoTracking().FirstOrDefaultAsync();

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
             where T1 : TenantBase
            where T2 : DtoBase
        {
            query = query.Where(m => m.TenantId == ctx.TenantId);

            T2 result = default;

            if (predicate.xIsNotEmpty())
                result = await query.AsNoTracking().Where(predicate).Select(expression).FirstOrDefaultAsync();
            else
                result = await query.AsNoTracking().Select(expression).FirstOrDefaultAsync();

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

        public static List<T> vToList<T>(this IQueryable<T> query, ISessionContext ctx, Expression<Func<T, bool>> predicate = null)
            where T : TenantBase
        {
            query = query.Where(m => m.TenantId == ctx.TenantId);
            if (predicate.xIsNotEmpty())
            {
                query = query.Where(predicate);
            }
            query = query.AsNoTracking().OrderByDescending(m => m.CreatedOn);
            var list = query.ToList();
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
            where T : TenantBase
        {
            query = query.Where(m => m.TenantId == ctx.TenantId);
            if (predicate.xIsNotEmpty())
            {
                query = query.Where(predicate);
            }
            query = query.AsNoTracking().OrderByDescending(m => m.CreatedOn)
                .ThenByDescending(m => m.LastModifiedOn);
            
            var list = await query.ToListAsync();
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

        public static async Task<PaginatedResult<T>> vToPaginatedListAsync<T>(this IQueryable<T> query, ISessionContext ctx, int pageNumber, int pageSize)
            where T : TenantBase
        {
            if (query == null) throw new Exception("queriable is empty");
            if (ctx.xIsEmpty()) throw new Exception("context is empty");

            if (ctx.CurrentUser.RoleName != RoleConstants.AdminRole)
            {
                query = query.Where(m => m.IsActive == true);
            }

            query = query.Where(m => m.TenantId == ctx.TenantId);

            pageNumber = pageNumber == 0 ? 1 : pageNumber + 1;
            pageSize = pageSize == 0 ? 10 : pageSize;
            int count = await query.CountAsync();
            List<T> items = await query.AsNoTracking().OrderByDescending(m => m.CreatedOn)
                .ThenByDescending(m => m.LastModifiedOn)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
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
            where T : TenantBase
            where TRequest : PaginatedRequest
        {
            if (query == null) throw new Exception("queriable is empty");
            if (ctx.xIsEmpty()) throw new Exception("context is empty");

            if (ctx.CurrentUser.RoleName != RoleConstants.AdminRole)
            {
                query = query.Where(m => m.IsActive == true);
            }
            else
            {
                query = query.Where(m => m.IsActive == request.IsActive);
            }

            query = query.Where(m => m.TenantId == ctx.TenantId);

            var pageNo = request.PageNo == 0 ? 1 : request.PageNo + 1;
            var pageSize = request.PageSize == 0 ? 10 : request.PageSize;
            int count = await query.CountAsync();

            List<T> items = null;

            if (request.SortName.xIsEmpty())
            {
                items = await query.AsNoTracking().OrderByDescending(m => m.CreatedOn)
                    .ThenByDescending(m => m.LastModifiedOn)
                    .Skip((pageNo - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();                
            }
            else
            {
                items = await query
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
            where T1 : TenantBase
            where T2 : DtoBase
            where TRequest : PaginatedRequest
        {
            if (query == null) throw new Exception("queriable is empty");
            if (ctx.xIsEmpty()) throw new Exception("context is empty");

            if (ctx.CurrentUser.RoleName != RoleConstants.AdminRole)
            {
                query = query.Where(m => m.IsActive == true);
            }
            else
            {
                query = query.Where(m => m.IsActive == request.IsActive);
            }

            query = query.Where(m => m.TenantId == ctx.TenantId);

            var pageNo = request.PageNo == 0 ? 1 : request.PageNo + 1;
            var pageSize = request.PageSize == 0 ? 10 : request.PageSize;
            int count = await query.CountAsync();

            List<T2> items = null;

            if (request.SortName.xIsEmpty())
            {
                items = await query.AsNoTracking().OrderByDescending(m => m.CreatedOn)
                    .ThenByDescending(m => m.LastModifiedOn)
                    .Skip((pageNo - 1) * pageSize)
                    .Take(pageSize)
                    .Select(expression)
                    .ToListAsync();
            }
            else
            {
                items = await query
                    .OrderBy($"{request.SortName} {request.OrderBy}")
                    .Skip((pageNo - 1) * pageSize)
                    .Take(pageSize).Select(expression)
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
    }
}