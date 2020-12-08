﻿using BlazorBattles.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BlazorBattles.Server.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        public Task<string> Login(string email, string password)
        {
            throw new System.NotImplementedException();
        }

        public async Task<int> Register(User user, string password)
        {
            if (await UserExists(user.Email))
            {
                return -1;
            }
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async  Task<bool> UserExists(string email)
        {
            if (await _context.Users.AnyAsync( x => x.Email.ToLower() == email.ToLower()))
            {
                return true;
            }
            return false;
        }
    }
}