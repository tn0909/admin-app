using AdminApp.Dtos;

namespace AdminApp.Services
{
    public interface IUserService
    {
        
        Task<IEnumerable<UserDto>> Search(SearchUserDto searchParams);
        Task<UserDto> GetById(string id);
        Task<bool> Add(UserDto userDto);
        Task<bool> Update(UserDto userDto);
        Task<bool> DeleteById(string id);
    }
}
