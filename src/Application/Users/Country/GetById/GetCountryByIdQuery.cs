using Application.Abstractions.Messaging;

namespace Application.Users.Country.GetById
{
    public sealed record GetCountryByIdQuery(long Id) : IQuery<CountryResponse>;
}
