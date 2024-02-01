using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eXtensionSharp;
using Jina.Domain.Account;
using Jina.Domain.SharedKernel;
using Jina.Domain.SharedKernel.Abstract;
using Jina.Passion.Client.Base;
using Jina.Passion.Client.Pages.Account.Services;

namespace Jina.Passion.Client.Pages.Account.ViewModels
{
    public class UserListViewModel : ViewModelBase<UserDto>
    {
        private readonly UserService _userService;

        public UserListViewModel(UserService service)
        {
            _userService = service;
        }

        public async Task<IPaginatedResult> GetUsersAsync(PaginatedRequest<UserDto> request)
        {
            if (request.xIsEmpty())
            {
                request = new PaginatedRequest<UserDto>()
                {
                    PageNo = 1,
                    PageSize = 10,
                };
            }

            var result = await this._userService.GetUsersAsync(request);
            if (result.Succeeded)
            {
                this.Items = result.Data;
                this.PageNo = result.PageNo;
                this.PageSize = result.PageSize;
                this.TotalCount = result.TotalCount;
            }

            return result;
        }

        public async Task<IResultBase<UserDto>> GetUserAsync(string userId)
        {
            return await this._userService.GetUserAsync(userId);
        }

        public async Task<IResultBase<string>> SaveAsync(UserDto user)
        {
            return await this._userService.SaveAsync(user);
        }

        public async Task<IResultBase<bool>> SaveAsync(IEnumerable<UserDto> users)
        {
            return await this._userService.SaveAsync(users);
        }

        public async Task<IResultBase<bool>> RemoveAsync(string id)
        {
            var exist = this.Items.First(x => x.Id == id);
            var result = await this._userService.RemoveAsync(new[] { exist });
            if (result.Succeeded)
            {
                this.Items.Remove(exist);
            }
            return result;
        }

        public async Task<IResultBase<bool>> RemoveRangeAsync()
        {
            var result = await this._userService.RemoveAsync(this.SelectedItems);
            if (result.Succeeded)
            {
                foreach (var item in this.SelectedItems)
                {
                    this.Items.Remove(item);
                }
            }
            return result;
        }
    }
}