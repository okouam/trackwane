﻿using System.Security.Claims;
using System.Web.Http;
using Trackwane.Framework.Common;

namespace Trackwane.Framework.Infrastructure.Web.Security
{
    public class BaseApiController : ApiController
    {
        protected UserClaims CurrentClaims
        {
            get { return new UserClaims((User as ClaimsPrincipal).Claims); }
        }
    }
}