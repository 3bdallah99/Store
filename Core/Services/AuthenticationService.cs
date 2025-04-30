using Domain.Exceptions;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Abstractions;
using Shared;
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
    public class AuthenticationService(UserManager<AppUser> userManager, IOptions<JwtOptions> options) : IAuthenticationService
    {
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
            var user = new AppUser()
            {
                DisplayName= registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber,
            };
            var result = await userManager.CreateAsync(user, registerDto.Password);
            //user.Email ==
            var checkEmailExist =  userManager.FindByEmailAsync(user.Email);
            if (checkEmailExist != null) {
                var Exception = new IdentityError() {
                    Code = "50",
                    Description = "Email is Exist " };
                //result= Exception;   
            }
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
