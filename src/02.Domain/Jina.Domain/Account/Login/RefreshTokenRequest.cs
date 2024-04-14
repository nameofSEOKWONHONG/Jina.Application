using Jina.Lang.Abstract;
using Jina.Validate;

namespace Jina.Domain.Account.Token
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }

        public class Valdiator : Validator<RefreshTokenRequest>
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