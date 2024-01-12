using eXtensionSharp;
using Jina.Lang.Abstract;
using Jina.Validate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jina.Domain.Account.Token
{
    public class RefreshTokenRequest
    {
        public string TenantId { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }

        public class Valdiator : RuleValidator<RefreshTokenRequest>
        {
            public Valdiator(ILocalizer localizer)
            {
                NotEmpty(m => m.Token);
                //NotEmpty(m => m.Token, message: localizer[""].xValue(""));

                NotEmpty(m => m.RefreshToken);
                //NotEmpty(m => m.RefreshToken, message: localizer[""].xValue(""));
            }
        }
    }
}