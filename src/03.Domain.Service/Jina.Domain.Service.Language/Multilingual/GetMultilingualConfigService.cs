// using Jina.Base.Service;
// using Jina.Base.Service.Abstract;
// using Jina.Domain.Entity;
// using Jina.Domain.Multilingual;
// using Jina.Domain.Service.Infra;
// using Jina.Domain.SharedKernel;
// using Jina.Domain.SharedKernel.Abstract;
// using Jina.Session.Abstract;
// using Mapster;
// using Microsoft.EntityFrameworkCore;
//
// namespace Jina.Domain.Service.Language;
//
// public interface IGetMultilingualConfigService : IServiceImplBase<bool, IResults<GetMultilingualConfigResult>>, IScopeService
// {
//     
// } 
//
// public class GetMultilingualConfigService : ServiceImplBase<GetMultilingualConfigService, AppDbContext, bool, IResults<GetMultilingualConfigResult>>, IGetMultilingualConfigService
// {
//     /// <summary>
//     /// ctor
//     /// </summary>
//     /// <param name="context"></param>
//     /// <param name="svc"></param>
//     public GetMultilingualConfigService(ISessionContext context, ServicePipeline svc) : base(context, svc)
//     {
//     }
//
//     public override Task OnExecutingAsync()
//     {
//         throw new NotImplementedException();
//     }
//
//     public override async Task OnExecuteAsync()
//     {
//         var result = await this.Db.MultilingualConfigs.Where(m => m.Id == m.ParentId)
//             .Include(m => m.ChildContents)
//             .OrderBy(m => m.Sort)
//             .ToListAsync();
//
//         var converted = result.Adapt<GetMultilingualConfigResult>();
//         this.Result = await Results<GetMultilingualConfigResult>.SuccessAsync(converted);
//     }
// }