using Application.Abstractions.Messaging;

namespace Application.Users.Countries.GetByName
{
    public sealed record GetCountryByNameQuery(string Name) : IQuery<CountryResponse>;
}
