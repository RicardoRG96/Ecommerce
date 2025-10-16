using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Messaging;
using Application.Users.Regions.GetById;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Municipalities.GetById
{
    internal sealed class GetMunicipalityByIdQueryHandler : IQueryHandler<GetMunicipalityByIdQuery, MunicipalityResponse>
    {
        private readonly IMunicipalityRepository _municipalityRepository;

        public GetMunicipalityByIdQueryHandler(IMunicipalityRepository municipalityRepository)
        {
            _municipalityRepository = municipalityRepository;
        }

        public async Task<Result<MunicipalityResponse>> Handle(GetMunicipalityByIdQuery query, CancellationToken cancellationToken)
        {
            Municipality? municipality = await _municipalityRepository.GetByIdAsync(query.MunicipalityId, cancellationToken);

            if (municipality is null)
            {
                return Result.Failure<MunicipalityResponse>(MunicipalityErrors.NotFound(query.MunicipalityId));
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