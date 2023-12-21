using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jina.Domain.Account.Request
{
    public class CreateRoleClaimRequest
    {
        public string TenantId { get; set; }
        public int Id { get; set; }
        public string RoleId { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string Group { get; set; }
        public bool Selected { get; set; }
    }
}