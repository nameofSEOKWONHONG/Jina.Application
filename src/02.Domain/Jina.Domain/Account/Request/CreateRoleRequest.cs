using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jina.Domain.Account.Request
{
    public class CreateRoleRequest
    {
        public string TenantId { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}