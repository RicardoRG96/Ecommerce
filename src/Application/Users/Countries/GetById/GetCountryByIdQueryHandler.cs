using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Countries.GetById
{
    internal sealed class GetCountryByIdQueryHandler : IQueryHandler<GetCountryByIdQuery, CountryResponse>
    {
        private readonly ICountryRepository _countryRepository;

        public GetCountryByIdQueryHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<Result<CountryResponse>> Handle(GetCountryByIdQuery query, CancellationToken cancellationToken)
        {
            Country? country = await _countryRepository.GetByIdAsync(query.Id, cancellationToken);

            if (country is null)
            {
                return Result.Failure<CountryResponse>(CountryErrors.NotFound(query.Id));
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
