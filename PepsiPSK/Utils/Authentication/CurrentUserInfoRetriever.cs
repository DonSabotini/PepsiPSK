using PepsiPSK.Constants;
using System.Security.Claims;

namespace PepsiPSK.Utils.Authentication
{
    public class CurrentUserInfoRetriever : ICurrentUserInfoRetriever
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserInfoRetriever(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string RetrieveCurrentUserId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue("id");
        }

        public bool CheckIfCurrentUserIsAdmin()
        {
            return _httpContextAccessor.HttpContext.User.IsInRole(RoleList.Admin);
        }
    }
}
