﻿using forge_app.Models;
using Microsoft.AspNetCore.Mvc;

namespace forge_app.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        public record AccessToken(string access_token, long expires_in);

        private readonly APS _aps;

        public AuthController(APS aps)
        {
            _aps = aps;
        }

        //End points

        [HttpGet("token")]
        public async Task<AccessToken> GetAccessToken()
        {
            var token = await _aps.GetPublicToken();
            return new AccessToken(
                token.AccessToken,
                (long)Math.Round((token.ExpiresAt - DateTime.UtcNow).TotalSeconds)
            );
        }
    }
}