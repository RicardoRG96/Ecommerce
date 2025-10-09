using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Countries.GetByName
{
    internal sealed class GetCountryByNameQueryHandler : IQueryHandler<GetCountryByNameQuery, CountryResponse>
    {
        private readonly ICountryRepository _countryRepository;

        public GetCountryByNameQueryHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<Result<CountryResponse>> Handle(GetCountryByNameQuery query, CancellationToken cancellationToken)
        {
            Country? country = await _countryRepository.GetByNameAsync(query.Name, cancellationToken);

            if (country is null)
            {
                return Result.Failure<CountryResponse>(CountryErrors.NotFoundByName);
            }

            CountryResponse response = new()
            {
                Id = country.CountryId,
                Name = country.Name
            };

            return Result.Success(response);
        }
    }
}
