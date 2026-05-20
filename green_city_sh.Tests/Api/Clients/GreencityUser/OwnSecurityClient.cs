using Allure.Net.Commons.Attributes;
using green_city_sh.Tests.Api.Clients.Greencity;
using green_city_sh.Tests.Api.DTO;
using green_city_sh.Tests.Api.DTO.User;
using RestSharp;

namespace green_city_sh.Tests.Api.Clients.GreencityUser
{
    internal class OwnSecurityClient : BaseApiClient
    {

        protected override string BaseUrl => $"{base._baseUrl}/ownSecurity";
        public OwnSecurityClient(string baseUrl, string? authToken = null) : base(baseUrl, authToken)
        {
        }

        [AllureStep("Sign in with email: {signInModal}")]
        public RestResponse SignIn(SignInModal signInModal)
        {
            var request = PrepareRequest("/signIn", Method.Post, signInModal);
            var responce = Client.Execute(request);
            return responce;
        }

        [AllureStep("Sign up new user with email: {requestBody.Email}")]
        public RestResponse SignUp(SignUpRequestDto requestBody, string lang = "en")
        {
            var request = PrepareRequest($"{BaseUrl}/signUp", Method.Post, requestBody);
            request.AddQueryParameter("lang", lang);
            return Client.Execute(request);
        }
    }
}
