using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jina.Domain.Account.Request
{
    public class RegisterRequest
    {
        public string TenantId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Notes { get; set; }

        public string Memo { get; set; }
    }
}