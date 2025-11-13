using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Messaging;
using Application.Users.Regions.GetWithPagination;
using Domain.Entities.Users;
using SharedKernel;

namespace Application.Users.Municipalities.GetWithPagination
{
    internal sealed class GetMunicipalitiesWithPaginationQueryHandler
        : IQueryHandler<GetMunicipalitiesWithPaginationQuery, PaginatedList<MunicipalityResponse>>
    {
        private readonly IMunicipalityRepository _municipalityRepository;

        public GetMunicipalitiesWithPaginationQueryHandler(IMunicipalityRepository municipalityRepository)
        {
            _municipalityRepository = municipalityRepository;
        }

        public async Task<Result<PaginatedList<MunicipalityResponse>>> Handle(GetMunicipalitiesWithPaginationQuery query, CancellationToken cancellationToken)
        {
            PaginatedList<Municipality> municipalities = await _municipalityRepository.GetAllAsync(
                query.PageNumber, 
                query.PageSize, 
                cancellationToken);

            PaginatedList<MunicipalityResponse> paginatedMunicipalityResponse = MapToMunicipalityResponsePaginatedList(
                municipalities,
                query);

            return Result.Success(paginatedMunicipalityResponse);
        }

        private PaginatedList<MunicipalityResponse> MapToMunicipalityResponsePaginatedList(
            PaginatedList<Municipality> municipalitiesPaginatedList,
            GetMunicipalitiesWithPaginationQuery query)
        {
            List<MunicipalityResponse> municipalityResponse = municipalitiesPaginatedList.Items
                .Select(m =>
                {
                    RegionResponse regionResponse = new()
                    {
                        Id = m.Region.RegionId,
                        Name = m.Region.Name!
                    };

                    MunicipalityResponse response = new()
                    {
                        Id = m.MunicipalityId,
                        Region = regionResponse,
                        Name = m.Name!
                    };

                    return response;
                }).ToList();

            PaginatedList<MunicipalityResponse> paginatedMunicipalityResponse =
                PaginatedList<MunicipalityResponse>.Create(
                    municipalityResponse,
                    municipalitiesPaginatedList.TotalCount,
                    municipalitiesPaginatedList.PageNumber,
                    query.PageSize);

            return paginatedMunicipalityResponse;
        }
    }
}
