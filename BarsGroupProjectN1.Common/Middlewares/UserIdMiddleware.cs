﻿using System.Security.AccessControl;
using System.Security.Claims;
using BarsGroupProjectN1.Core.AppSettings;
using BarsGroupProjectN1.Core.Contracts;
using BarsGroupProjectN1.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BarsGroupProjectN1.Core.Middlewares;

public class UserIdMiddleware(
    RequestDelegate next,
    ILogger<UserIdMiddleware> Logger)
{
    private readonly RequestDelegate _next = next;
    private readonly string _unauthorizedUserViewUrl = "http://localhost:5025/home/unauthorizedUser";


    public async Task Invoke(HttpContext context)
    {
        var userIdString = GetFromCookiesOrHeaders(context, UserClaimsStrings.UserId);
        var roleString = GetFromCookiesOrHeaders(context, UserClaimsStrings.Role);


        if (roleString == ((int)RoleCode.Admin).ToString())
        {
            Logger.LogInformation($"Get admin user {userIdString}");
      
            await _next(context);
            return;
        }

        if (!Guid.TryParse(userIdString, out Guid userId) || string.IsNullOrEmpty(roleString))
        {
            Logger.LogInformation(
                $"Get unauthorized user {userIdString} with role {roleString} redirecting to {_unauthorizedUserViewUrl}");
            
            context.Response.Redirect(_unauthorizedUserViewUrl);
            return;
        }


        context.User = new ClaimsPrincipal(
            new ClaimsIdentity(
                new Claim[]
                {
                    new(UserClaimsStrings.UserId, userId.ToString()),
                    new(UserClaimsStrings.Role, roleString)
                }
            )
        );
        
        Logger.LogInformation($"Get authorized user {userIdString} with role {roleString}");


        await _next(context);
    }

    private string GetFromCookiesOrHeaders(HttpContext context, string key)
    {
        return context.Request.Cookies[key] ??
               context.Request.Headers[key].ToString();
    }
}