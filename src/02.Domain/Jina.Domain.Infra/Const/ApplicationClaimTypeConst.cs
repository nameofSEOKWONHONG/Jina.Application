using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jina.Domain.Infra.Const
{
    public class ApplicationClaimTypeConst
    {
        public const string Permission = "Permission";
        public const string TenantId = "http://schemas.microsoft.com/identity/claims/identityprovider";
        public const string TimeZone = "http://schemas.microsoft.com/identity/claims/timezone";
    }
}