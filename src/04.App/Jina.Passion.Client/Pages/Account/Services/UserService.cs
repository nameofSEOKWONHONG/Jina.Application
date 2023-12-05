using Jina.Domain.Account;
using Jina.Domain.Example;
using Jina.Domain.SharedKernel;
using Jina.Domain.SharedKernel.Abstract;
using Jina.Passion.Client.Base;

namespace Jina.Passion.Client.Pages.Account.Services
{
    public class UserService : FeServiceBase
    {
        public UserService(HttpClient client) : base(client)
        {
        }

        public async Task<PaginatedResult<UserDto>> GetUsersAsync(PaginatedRequest<UserDto> request)
        {
            await Task.Delay(500);

            return await PaginatedResult<UserDto>.SuccessAsync(new List<UserDto>()
            {
                new UserDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = "test@gmail.com",
                    Name = "test",
                    Phone = "010-1111-2222",
                    RoleName = "admin",
                    GroupName = "manage",
                    CreatedName = "test",
                    LastModifiedName = "test"
                }
            }, 20, 1, 10);
        }

        public async Task<IResultBase<UserDto>> GetUserAsync(string userId)
        {
            return null;
        }

        public async Task<IResultBase<string>> SaveAsync(UserDto user)
        {
            return null;
        }

        public async Task<IResultBase<bool>> SaveAsync(IEnumerable<UserDto> users)
        {
            return null;
        }

        public async Task<IResultBase<bool>> RemoveAsync(IEnumerable<UserDto> users)
        {
            return null;
        }
    }
}