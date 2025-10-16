using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Messaging;
using Application.Users.Regions.GetByName;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Municipalities.GetByName
{
    internal sealed class GetMunicipalityByNameQueryHandler 
        : IQueryHandler<GetMunicipalityByNameQuery, MunicipalityResponse>
    {
        private readonly IMunicipalityRepository _municipalityRepository;

        public GetMunicipalityByNameQueryHandler(IMunicipalityRepository municipalityRepository)
        {
            _municipalityRepository = municipalityRepository;
        }

        public async Task<Result<MunicipalityResponse>> Handle(GetMunicipalityByNameQuery query, CancellationToken cancellationToken)
        {
            Municipality? municipality = await _municipalityRepository.GetByNameAsync(
                query.Name, 
                cancellationToken);

            if (municipality is null)
            {
                return Result.Failure<MunicipalityResponse>(MunicipalityErrors.NotFoundByName);
            }

            RegionResponse regionResponse = new()
            {
                Id = municipality.Region.RegionId,
                Name = municipality.Region.Name!
            };

            MunicipalityResponse response = new()
            {
                Id = municipality.MunicipalityId,
                Region = regionResponse,
                Name = municipality.Name!
            };

            return Result.Success(response);
        }
    }
}
