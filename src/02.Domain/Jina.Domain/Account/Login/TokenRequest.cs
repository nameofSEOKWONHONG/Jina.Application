using FluentValidation;
using Jina.Lang.Abstract;
using Jina.Validate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jina.Domain.Account.Token
{
    public class TokenRequest
    {
        public string TenantId { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConnectionId { get; set; }

        public bool RemamberMe { get; set; }
        public string LoginType { get; set; } = "1";

        public class Validator : Validator<TokenRequest>
        {
            public Validator(ILocalizer localizer)
            {
                RuleFor(m => m.TenantId)
                    .NotEmpty()
                    ;
                //.WithMessage(localizer[""].xValue(""));

                RuleFor(m => m.Email)
                    .NotEmpty();

                RuleFor(m => m.Password)
                    .NotEmpty();
            }
        }
    }
}