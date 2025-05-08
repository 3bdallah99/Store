using Shared;
using Shared.OrderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IAuthenticationService
    {
        Task<UserResultDto> LoginAsync(LoginDto loginDto);
        Task<UserResultDto> RegisterAsync(RegisterDto registerDto);
        // Get Current User
        Task<UserResultDto> GetUserByEmail(string email);
        // Check Email Exist
        Task<bool> CheckEmailExistsAsync(string email);
        // Get USer Address
        Task<AddressDto> GetUserAddress(string email);
        // Update User Address
        Task<AddressDto> UpdateUserAddress(AddressDto address , string email); 
    }
}
