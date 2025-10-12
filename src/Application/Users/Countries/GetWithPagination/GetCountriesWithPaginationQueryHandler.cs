using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using SharedKernel;

namespace Application.Users.Countries.GetWithPagination
{
    internal sealed class GetCountriesWithPaginationQueryHandler
        : IQueryHandler<GetCountriesWithPaginationQuery, PaginatedList<CountryResponse>>
    {
        private readonly ICountryRepository _countryRepository;

        public GetCountriesWithPaginationQueryHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<Result<PaginatedList<CountryResponse>>> Handle(GetCountriesWithPaginationQuery query, CancellationToken cancellationToken)
        {
            PaginatedList<Country> countries = await _countryRepository.GetAllAsync(
                query.PageNumber, 
                query.PageSize, 
                cancellationToken);

            PaginatedList<CountryResponse> paginatedCountriesResponse = MapToCountryResponsePaginatedList(
                countries,
                query);

            return Result.Success(paginatedCountriesResponse);
        }

        private static PaginatedList<CountryResponse> MapToCountryResponsePaginatedList(
            PaginatedList<Country> countriesPaginatedList,
            GetCountriesWithPaginationQuery query)
        {
            List<CountryResponse> countryResponse = countriesPaginatedList.Items.Select(c =>
            {
                CountryResponse countryResponse = new()
                {
                    Id = c.CountryId,
                    Name = c.Name
                };

                return countryResponse;
            }).ToList();

            PaginatedList<CountryResponse> paginatedCountryResponse = PaginatedList<CountryResponse>.Create(
                countryResponse,
                countriesPaginatedList.TotalCount,
                countriesPaginatedList.PageNumber,
                query.PageSize);

            return paginatedCountryResponse;
        }
    }
}
