namespace Api.FunctionalTests.Abstractions
{
    public class BaseFunctionalTest : IClassFixture<FunctionalTestWebAppFactory>
    {
        protected const string usersBaseUrl = "api/v1/users";

        protected const string countriesBaseUrl = "api/v1/countries";

        protected const string regionsBaseUrl = "api/v1/regions";

        public BaseFunctionalTest(FunctionalTestWebAppFactory factory)
        {
            HttpClient = factory.CreateClient();
        }

        protected HttpClient HttpClient { get; init; }
    }
}
