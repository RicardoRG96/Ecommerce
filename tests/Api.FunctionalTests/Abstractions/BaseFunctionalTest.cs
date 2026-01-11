namespace Api.FunctionalTests.Abstractions
{
    public class BaseFunctionalTest : IClassFixture<FunctionalTestWebAppFactory>
    {
        protected const string usersBaseUrl = "api/v1/users";

        protected const string countriesBaseUrl = "api/v1/countries";

        protected const string regionsBaseUrl = "api/v1/regions";

        protected const string municipalitiesBaseUrl = "api/v1/municipalities";

        protected const string addressesBaseUrl = "api/v1/addresses";

        public BaseFunctionalTest(FunctionalTestWebAppFactory factory)
        {
            HttpClient = factory.AuthenticatedClient;
            Auth = factory.Auth;
        }

        protected HttpClient HttpClient { get; init; }
        protected AuthFixture Auth { get; }
    }
}
