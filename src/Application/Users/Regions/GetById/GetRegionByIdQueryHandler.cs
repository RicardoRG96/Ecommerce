using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Regions.GetById
{
    internal sealed class GetRegionByIdQueryHandler : IQueryHandler<GetRegionByIdQuery, RegionResponse>
    {
        private readonly IRegionRepository _regionRepository;

        public GetRegionByIdQueryHandler(IRegionRepository regionRepository)
        {
            _regionRepository = regionRepository;
        }

        public async Task<Result<RegionResponse>> Handle(GetRegionByIdQuery query, CancellationToken cancellationToken)
        {
            Region? region = await _regionRepository.GetByIdAsync(query.RegionId, cancellationToken);

            if (region is null)
            {
                return Result.Failure<RegionResponse>(RegionErrors.NotFound(query.RegionId));
            }

            RegionResponse response = new()
            {
                Id = region.RegionId,
                Name = region.Name
            };

            return Result.Success(response);
        }
    }
}
