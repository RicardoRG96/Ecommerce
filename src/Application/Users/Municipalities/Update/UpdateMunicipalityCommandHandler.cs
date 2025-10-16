using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Municipalities.Update
{
    internal sealed class UpdateMunicipalityCommandHandler : ICommandHandler<UpdateMunicipalityCommand>
    {
        private readonly IMunicipalityRepository _municipalityRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateMunicipalityCommandHandler(
            IMunicipalityRepository municipalityRepository,
            IRegionRepository regionRepository,
            IUnitOfWork unitOfWork)
        {
            _municipalityRepository = municipalityRepository;
            _regionRepository = regionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateMunicipalityCommand command, CancellationToken cancellationToken)
        {
            Municipality? municipality = await _municipalityRepository.GetByIdAsync(
                command.MunicipalityId, 
                cancellationToken);

            if (municipality is null)
            {
                return Result.Failure(MunicipalityErrors.NotFound(command.MunicipalityId));
            }

            Region? region = await _regionRepository.GetByIdAsync(command.RegionId, cancellationToken);

            if (region is null)
            {
                return Result.Failure(RegionErrors.NotFound(command.RegionId));
            }

            municipality.Region = region;
            municipality.RegionId = region.RegionId;
            municipality.Name = command.Name;

            _municipalityRepository.Update(municipality);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
