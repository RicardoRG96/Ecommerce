using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using SharedKernel;

namespace Application.Users.Regions.GetWithPagination
{
    internal sealed class GetRegionsWithPaginationQueryHandler
        : IQueryHandler<GetRegionsWithPaginationQuery, PaginatedList<RegionResponse>>
    {
        private readonly IRegionRepository _regionRepository;

        public GetRegionsWithPaginationQueryHandler(IRegionRepository regionRepository)
        {
            _regionRepository = regionRepository;
        }

        public async Task<Result<PaginatedList<RegionResponse>>> Handle(
            GetRegionsWithPaginationQuery query, 
            CancellationToken cancellationToken)
        {
            PaginatedList<Region> regions = await _regionRepository.GetAllAsync(
                query.PageNumber, 
                query.PageSize, 
                cancellationToken);

            PaginatedList<RegionResponse> paginatedRegionsResponse = MapToRegionResponsePaginatedList(
                regions,
                query);

            return Result.Success(paginatedRegionsResponse);
        }

        private PaginatedList<RegionResponse> MapToRegionResponsePaginatedList(
            PaginatedList<Region> regionsPaginatedList, 
            GetRegionsWithPaginationQuery query)
        {
            List<RegionResponse> regionResponse = regionsPaginatedList.Items.Select(r =>
            {
                RegionResponse response = new()
                {
                    Id = r.RegionId,
                    Name = r.Name
                };

                return response;
            }).ToList();

            PaginatedList<RegionResponse> paginatedRegionResponse = PaginatedList<RegionResponse>.Create(
                regionResponse,
                regionsPaginatedList.TotalCount,
                regionsPaginatedList.PageNumber,
                query.PageSize);

            return paginatedRegionResponse;
        }
    }
}
