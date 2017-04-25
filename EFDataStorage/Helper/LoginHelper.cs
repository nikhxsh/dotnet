﻿using System;

namespace EFDataStorage.Helper
{
    public class LoginRequest : Patterns.QueryFor<LoginResponse>
    {
        public string Username { get; set; }

        public string Password { get; set; }        
    }


    public class LoginResponse
    {
        public Guid UserId { get; set; }

        public bool IsSuccessfull { get; set; }

        public int? RemainingLoginAttempts { get; set; }

        public string Token { get; set; }
        
        public bool IsFirstLogin { get; set; }

        public UserStatus UserCurrentStatus { get; set; }

        public bool PasswordExpired { get; set; }

        public bool IsUnlockedByLogin { get; set; }
    }

    public enum UserStatus
    {
        Active, Deactive, Locked
    }
}
