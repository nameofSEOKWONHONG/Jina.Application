using Jina.Domain.Account;
using Jina.Domain.Shared;
using Jina.Domain.Shared.Abstract;
using Jina.Passion.Client.Base;
using Jina.Passion.Client.Common.Infra;

namespace Jina.Passion.Client.Pages.Account.Services
{
    public class UserService : ServiceBase
    {
        public UserService(IRestClient client) : base(client)
        {
        }

        public async Task<PaginatedResult<UserDto>> GetUsersAsync(PaginatedRequest<UserDto> request)
        {
            await Task.Delay(500);

            return await PaginatedResult<UserDto>.SuccessAsync(new List<UserDto>()
            {
                new UserDto()
                {
                    Id = "1",
                    Email = "test1@gmail.com",
                    Name = "test1",
                    Phone = "010-1111-2222",
                    RoleName = "admin",
                    GroupName = "manage",
                    CreatedName = "test",
                    LastModifiedName = "test"
                },
                new UserDto()
                {
                    Id = "2",
                    Email = "test2@gmail.com",
                    Name = "test2",
                    Phone = "010-1111-2222",
                    RoleName = "admin",
                    GroupName = "manage",
                    CreatedName = "test",
                    LastModifiedName = "test"
                }
                
            }, 20, 1, 10);
        }

        public async Task<IResults<UserDto>> GetUserAsync(string userId)
        {
            return await Results<UserDto>.SuccessAsync(new UserDto()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@gmail.com",
                Name = "test",
                Phone = "010-1111-2222",
                RoleName = "admin",
                GroupName = "manage",
                CreatedName = "test",
                LastModifiedName = "test"
            });
        }

        public async Task<IResults<string>> SaveAsync(UserDto user)
        {
            return await Results<string>.SuccessAsync(data: Guid.NewGuid().ToString("N"));
        }

        public async Task<IResults<bool>> SaveAsync(IEnumerable<UserDto> users)
        {
            return await Results<bool>.SuccessAsync();
        }

        public async Task<IResults<bool>> RemoveAsync(IEnumerable<UserDto> users)
        {
            return await Results<bool>.SuccessAsync();
        }
    }
}