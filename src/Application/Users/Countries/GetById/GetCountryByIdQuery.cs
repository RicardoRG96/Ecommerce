using Application.Abstractions.Messaging;

namespace Application.Users.Countries.GetById
{
    public sealed record GetCountryByIdQuery(long Id) : IQuery<CountryResponse>;
}
