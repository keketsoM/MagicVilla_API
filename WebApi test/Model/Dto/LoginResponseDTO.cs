﻿namespace WebApi_test.Model.Dto
{
    public class LoginResponseDTO
    {
        public UserDto LocalUser { get; set; }

        public string Token { get; set; }
    }
}
