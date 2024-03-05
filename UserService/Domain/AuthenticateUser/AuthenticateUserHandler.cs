﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UserService.Common;
using UserService.Data;
using UserService.Services;

namespace UserService.Domain.AuthenticateUser
{
    public class AuthenticateUserHandler : IHandler<AuthenticateUserCommand, CustomResponse<AuthenticateUserResult>>
    {
        private readonly UserContext dbContext;
        private readonly IAuthenticationService authenticationService;


        public AuthenticateUserHandler(UserContext dbContext, IAuthenticationService authenticationService)
        {
            this.dbContext = dbContext;
            this.authenticationService = authenticationService;
        }
        public async Task<CustomResponse<AuthenticateUserResult>> Handle(AuthenticateUserCommand command)
        {
            var user = await this.dbContext.Users.Where(t => t.Email == command.email).FirstOrDefaultAsync();
            var result = new CustomResponse<AuthenticateUserResult>()
            {
                Data = new AuthenticateUserResult(string.Empty),
            };

            if(user == null)
            {
                return CustomHttpResult.NotFound<AuthenticateUserResult>("User doesn't exist");
            }

            var inputPassword = command.password.ComputeSHA256Hash();

            if(inputPassword != user.Password)
            {
                return CustomHttpResult.UnAuthorized<AuthenticateUserResult>("Invalid email or password");
            }

            var token = this.authenticationService.IssueJWT(user.Email);

            return CustomHttpResult.Ok(new AuthenticateUserResult(token));
        }

        
    }
}
