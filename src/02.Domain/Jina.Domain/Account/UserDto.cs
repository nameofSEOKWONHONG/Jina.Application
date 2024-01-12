using Jina.Domain.Example;
using Jina.Validate;

namespace Jina.Domain.Account
{
    public class UserDto : DtoBase
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public string Phone { get; set; }
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }

        public class Validator : Validator<UserDto>
        {
            public Validator()
            {
                NotEmpty(m => m.Id);
                NotEmpty(m => m.Name);
                NotEmpty(m => m.Password);
                NotEmpty(m => m.Email);
                NotEmpty(m => m.Phone);
                NotEmpty(m => m.GroupId);
                NotEmpty(m => m.RoleId);
            }
        }
    }
}