using Shared.DTOs.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IAuthenticationService
    {
        Task<UserDto> LoginAsync (LoginDto loginDto);

        Task<UserDto> RegisterAsync (RegisterDto registerDto);

        //Check EMail take string Emal ret boolen
        Task<bool> CheckEmailAsync (string email);

        Task<AddressDto> GetCurrentUserAddress(string email);

        Task<AddressDto> UpdateCurrentUserAddress(string email , AddressDto addressDto);

        Task<UserDto> GetCurrentUserAsync(string email);    

    }
}
