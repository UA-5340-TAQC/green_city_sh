using green_city_sh.Tests.Api.DTO;
using RestSharp;

namespace green_city_sh.Tests.Api.Clients.GreencityUser
{
    internal class OwnSecurityClient : BaseApiClient
    {

        protected override string BaseUrl => $"{base._baseUrl}/ownSecurity";
        public OwnSecurityClient(string baseUrl, string? authToken = null) : base(baseUrl, authToken)
        {
        }

        public async Task<AuthResponce> SignIn(SignInModal signInModal)
        {
            var request = PrepareRequest("/signIn", Method.Post, signInModal);
            var responce = await Client.ExecuteAsync<AuthResponce>(request);
            if (responce.IsSuccessful)
            {
                return responce.Data;
            }
            else
            {
                throw new Exception($"Failed to sign in: {responce.StatusCode} - {responce.Content}");
            }
        }
    }
}
