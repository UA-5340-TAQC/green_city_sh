using RestSharp;

namespace green_city_sh.Tests.Api.Clients.GreencityUser
{
    internal class OwnSecurityClient : BaseApiClient
    {

        protected override string BaseUrl => $"{base._baseUrl}/ownSecurity";
        public OwnSecurityClient(string baseUrl, string? authToken = null) : base(baseUrl, authToken)
        {
        }

        public RestResponse SignIn(string email, string password, string projectName = "GREENCITY")
        {


            var body = new
            {
                email,
                password,
                projectName
            };
            var request = PrepareRequest("/signIn", Method.Post, body);
            return Client.Execute(request);
        }
    }
}
