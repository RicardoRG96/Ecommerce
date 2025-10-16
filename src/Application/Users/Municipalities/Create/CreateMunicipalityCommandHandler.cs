using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Municipalities.Create
{
    internal sealed class CreateMunicipalityCommandHandler : ICommandHandler<CreateMunicipalityCommand, long>
    {
        private readonly IMunicipalityRepository _municipalityRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateMunicipalityCommandHandler(
            IMunicipalityRepository municipalityRepository,
            IRegionRepository regionRepository,
            IUnitOfWork unitOfWork)
        {
            _municipalityRepository = municipalityRepository;
            _regionRepository = regionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<long>> Handle(CreateMunicipalityCommand command, CancellationToken cancellationToken)
        {
            Region? region = await _regionRepository.GetByIdAsync(command.RegionId, cancellationToken);

            if (region is null)
            {
                return Result.Failure<long>(RegionErrors.NotFound(command.RegionId));
            }

            Municipality? municipality = new()
            {
                RegionId = region.RegionId,
                Region = region,
                Name = command.Name
            };

            await _municipalityRepository.AddAsync(municipality, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(municipality.MunicipalityId);
        }
    }
}
