using AutoMapper;
using Domain.Exceptions;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Abstractions;
using Shared;
using Shared.OrderDtos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services
{
    public class AuthenticationService(UserManager<AppUser> userManager, IOptions<JwtOptions> options , IMapper mapper) : IAuthenticationService
    {
		public async Task<bool> CheckEmailExistsAsync(string email)
		{
            var user = await userManager.FindByEmailAsync(email);
            return user != null;
		}

		public async Task<AddressDto> GetUserAddress(string email)
		{
			var user = await userManager.Users.Include(u => u.Address)
                                              .FirstOrDefaultAsync(u => u.Email == email)
                                              ?? throw new UserNotFoundException(email);
           var result = mapper.Map<AddressDto>(user.Address);
            return result;
		}

		public async Task<UserResultDto> GetUserByEmail(string email)
		{
            var user = await userManager.FindByEmailAsync(email)
			?? throw new UserNotFoundException(email);
            return new UserResultDto() {
			 DisplayName =user.DisplayName , 
                Email = user.Email , 
                Token = await GenerateJwtToken(user)
			};
		}

		public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) throw new UnAuthorizedException();
            var flag =await userManager.CheckPasswordAsync(user,loginDto.Password);
            if (!flag) throw new UnAuthorizedException(); 
            return new UserResultDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateJwtToken(user),
            };
        }

        public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
        {
            if (await CheckEmailExistsAsync(registerDto.Email))
            {
                throw new DuplicatedEmailBadRequestException(registerDto.Email);    
            }
            var user = new AppUser()
            {
                DisplayName= registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber,
            };
            var result = await userManager.CreateAsync(user, registerDto.Password);
            
            
            if (!result.Succeeded) 
            { 
                var errors = result.Errors.Select(erorr => erorr.Description);
                throw new ValidationException(errors);
            }
            return new UserResultDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateJwtToken(user),
            };
        }

		public async Task<AddressDto> UpdateUserAddress(AddressDto address, string email)
		{
			var user = await userManager.Users.Include(u => u.Address)
											 .FirstOrDefaultAsync(u => u.Email == email)
											 ?? throw new UserNotFoundException(email);
			var result = mapper.Map<AddressDto>(user.Address);
            if (user.Address != null)
            {
                user.Address.FirstName = result.FirstName;

                user.Address.Street = result.Street;
                user.Address.City = result.City;
                user.Address.Country = result.Country;
            }
            else
            {
                var userAddress = mapper.Map<Address>(address);
                user.Address = userAddress;
            }
            await userManager.UpdateAsync(user);
            return address;
		}

		private async Task<string> GenerateJwtToken(AppUser user)
        {
            // Header 
            // PayLoad
            // Signature
            var jwtOptions = options.Value;
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
            };
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));
            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issure,
                audience: jwtOptions.Audience,
                claims:authClaims,
                expires: DateTime.UtcNow.AddDays(jwtOptions.DurationInDays ),
                signingCredentials: new SigningCredentials (secretKey, SecurityAlgorithms.HmacSha256Signature)
                );
            // Token 
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
