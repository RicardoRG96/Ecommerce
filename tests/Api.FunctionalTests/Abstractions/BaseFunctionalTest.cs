namespace Api.FunctionalTests.Abstractions
{
    public class BaseFunctionalTest : IClassFixture<FunctionalTestWebAppFactory>
    {
        protected const string usersBaseUrl = "api/v1/users";

        protected const string countriesBaseUrl = "api/v1/countries";

        public BaseFunctionalTest(FunctionalTestWebAppFactory factory)
        {
            HttpClient = factory.CreateClient();
        }

        protected HttpClient HttpClient { get; init; }
    }
}
