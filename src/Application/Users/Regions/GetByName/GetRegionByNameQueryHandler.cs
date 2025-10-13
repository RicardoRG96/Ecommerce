using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Regions.GetByName
{
    internal sealed class GetRegionByNameQueryHandler : IQueryHandler<GetRegionByNameQuery, RegionResponse>
    {
        private readonly IRegionRepository _regionRepository;

        public GetRegionByNameQueryHandler(IRegionRepository regionRepository)
        {
            _regionRepository = regionRepository;
        }

        public async Task<Result<RegionResponse>> Handle(GetRegionByNameQuery query, CancellationToken cancellationToken)
        {
            Region? region = await _regionRepository.GetByNameAsync(query.Name, cancellationToken);

            if (region is null)
            {
                return Result.Failure<RegionResponse>(RegionErrors.NotFoundByName);
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
