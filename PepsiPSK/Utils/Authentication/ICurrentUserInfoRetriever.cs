namespace PepsiPSK.Utils.Authentication
{
    public interface ICurrentUserInfoRetriever
    {
        string RetrieveCurrentUserId();
        bool CheckIfCurrentUserIsAdmin();
    }
}
