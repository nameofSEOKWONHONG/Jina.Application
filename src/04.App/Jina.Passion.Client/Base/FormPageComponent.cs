﻿using eXtensionSharp;
using Jina.Domain.Shared.Abstract;
using Jina.Passion.Client.Client.Base;

namespace Jina.Passion.Client.Base
{
    public abstract class FormPageComponent<TOption, TResult> : PageComponentBase<TOption, TResult>
        where TOption : DlgOptionsBase
    {
        protected override async Task OnLoadAsync()
        {
            await OnSearch(null);
        }

        public virtual async Task OnSearch(Func<Task<IResults>> callback)
        {
            if (callback.xIsEmpty()) return;

            //this.Loading = true;

            var result = await callback();
            await this.StateChangedAsync();

            //this.Loading = false;
        }
    }
}