using AutoMapper;
using Domain.Exceptions;
using Domain.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceAbstraction;
using Shared.DTOs.IdentityDtos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AuthenticationService(UserManager<ApplicationUser> _userManager,IConfiguration _configuration,IMapper _mapper) : IAuthenticationService
    {
        public async Task<bool> CheckEmailAsync(string email)
        {
           var User = await _userManager.FindByEmailAsync(email);
            return User is not null;
        }

        public async Task<UserDto> GetCurrentUserAsync(string email)
        {
          var User = await _userManager.FindByEmailAsync(email)?? throw new UserNotFoundException(email);
            return new UserDto() { DisplayName= User.DisplayName,Email=User.Email,Token= await CreateTokenAsync(User)};
        }

        public async Task<AddressDto> GetCurrentUserAddress(string email)
        {
           var User = await  _userManager.Users.Include(u=>u.Address)
                               .FirstOrDefaultAsync(U=>U.Email==email)?? throw new UserNotFoundException(email);
            //if (User.Address is not null)
                return _mapper.Map<AddressDto>(User.Address);
            //else
                //throw new AddressNotFoundException(User.UserName);               
        }
        public async Task<AddressDto> UpdateCurrentUserAddress(string email, AddressDto addressDto)
        {
            var User = await _userManager.Users.Include(u => u.Address)
                               .FirstOrDefaultAsync(U => U.Email == email) ?? throw new UserNotFoundException(email);
            if(User.Address is not null)
            {
                User.Address.FirstName = addressDto.FirstName;
                User.Address.LastName = addressDto.LastName;
                User.Address.City = addressDto.City;
                User.Address.Country = addressDto.Country;
                User.Address.Street = addressDto.Street;

            }
            else
            {
                User.Address = _mapper.Map<AddressDto,Address>(addressDto);
            }
           await _userManager.UpdateAsync(User);
            return _mapper.Map<AddressDto>(User.Address);
        }

        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            var User = await _userManager.FindByEmailAsync(loginDto.Email) ?? throw new UserNotFoundException(loginDto.Email);
            //Check Password
            var IsPasswordValid = await _userManager.CheckPasswordAsync(User, loginDto.Password);
            if (IsPasswordValid)
            {
                //return User
                return new UserDto()
                {
                    DisplayName = User.DisplayName,
                    Email = User.Email,
                    Token = await CreateTokenAsync(User)
                };
            }
            else
            {
                throw new UnauthorizedException();
            }
        }


        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            var User = new ApplicationUser()
            {
                Email = registerDto.Email, 
                DisplayName = registerDto.DisplayName,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber,
            };
            var Result = await _userManager.CreateAsync(User,registerDto.Password);
            if (Result.Succeeded)
            {
                return new UserDto() { DisplayName = User.DisplayName, Email = User.Email,Token = await CreateTokenAsync(User) };
            }
            else
            {
                var Errors = Result.Errors.Select(E=>E.Description).ToList();
                throw new BadRequestException(Errors);
            }

        }

      
        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var Claims =  new List<Claim>()
            {
                new(ClaimTypes.Email,user.Email!),
                new(ClaimTypes.Name, user.UserName!),
                new(ClaimTypes.NameIdentifier, user.Id!),
            };
            var Roles = await _userManager.GetRolesAsync(user);
            foreach (var role in Roles) 
                Claims.Add(new Claim(ClaimTypes.Role, role));
            var SecretKey = _configuration.GetSection("JWTOptions")["SecretKey"];
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var Creds = new SigningCredentials(Key,SecurityAlgorithms.HmacSha256);

            var Token = new JwtSecurityToken(
                issuer: _configuration["JWTOptions:Issuer"],
                audience: _configuration["JWTOptions:Audience"],
                claims: Claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: Creds);
            return new JwtSecurityTokenHandler().WriteToken(Token);


               

        }
    }
}
