using AntDesign;
using AntDesign.TableModels;
using eXtensionSharp;
using Jina.Domain.Account;
using Jina.Domain.SharedKernel;
using Jina.Domain.SharedKernel.Abstract;
using Jina.Passion.Client.Base;
using Jina.Passion.Client.Pages.Account.Services;

namespace Jina.Passion.Client.Pages.Account.ViewModels
{
    public class UserViewModel : FeViewModelBase
    {
        private readonly UserService _userService;

        public List<UserDto> Users { get; set; }
        public UserDto SelectedItem { get; set; }
        public IEnumerable<UserDto> SelectedItems { get; set; }

        public UserViewModel(UserService service)
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
                this.Users = result.Data;
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

        public async Task<IResultBase<bool>> RemoveAsync()
        {
            return await this._userService.RemoveAsync(new[] { this.SelectedItem });
        }

        public async Task<IResultBase<bool>> RemoveRangeAsync()
        {
            return await this._userService.RemoveAsync(this.SelectedItems);
        }
    }
}