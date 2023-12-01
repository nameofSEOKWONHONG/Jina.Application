using eXtensionSharp;
using Jina.Domain.SharedKernel.Abstract;
using Jina.Passion.Client.Base.Abstract;
using Jina.Passion.Client.Client.Base;

namespace Jina.Passion.Client.Base
{
    public abstract class FormPageComponent<TOption, TResult> : PageComponentBase<TOption, TResult>
        where TOption : DlgOptionsBase
    {
        public virtual async Task OnSearch(Func<Task<IResultBase>> callback)
        {
            if (callback.xIsEmpty()) return;

            this.Loading = true;

            var result = await callback();
            await this.StateChangedAsync();

            this.Loading = false;
        }
    }
}