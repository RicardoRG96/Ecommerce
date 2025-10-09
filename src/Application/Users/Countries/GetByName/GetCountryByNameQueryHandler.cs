using Application.Abstractions.Messaging;

namespace Application.Users.Countries.GetByName
{
    public sealed record GetCountryByNameQueryHandler(string Name) : IQuery<CountryResponse>;
}
