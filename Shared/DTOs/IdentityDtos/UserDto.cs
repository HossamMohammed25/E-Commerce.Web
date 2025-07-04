﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.IdentityDtos
{
    public class UserDto
    {
        [EmailAddress]
        public string Email { get; set; } = default!;
        public string Token { get; set; } = default!;
        public string DisplayName { get; set; } = default!;

    }
}
