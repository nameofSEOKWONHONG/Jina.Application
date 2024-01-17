using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
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

        public async Task<IResultBase<UserDto>> GetUserAsync(string userId)
        {
            return await Result<UserDto>.SuccessAsync(new UserDto()
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

        public async Task<IResultBase<string>> SaveAsync(UserDto user)
        {
            return await Result<string>.SuccessAsync(data: Guid.NewGuid().ToString("N"));
        }

        public async Task<IResultBase<bool>> SaveAsync(IEnumerable<UserDto> users)
        {
            return await Result<bool>.SuccessAsync();
        }

        public async Task<IResultBase<bool>> RemoveAsync(IEnumerable<UserDto> users)
        {
            return await Result<bool>.SuccessAsync();
        }
    }
}