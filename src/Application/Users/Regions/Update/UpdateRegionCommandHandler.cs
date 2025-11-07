using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Regions.Update
{
    internal sealed class UpdateRegionCommandHandler : ICommandHandler<UpdateRegionCommand>
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRegionCommandHandler(IRegionRepository regionRepository, IUnitOfWork unitOfWork)
        {
            _regionRepository = regionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateRegionCommand command, CancellationToken cancellationToken)
        {
            Region? region = await _regionRepository.GetByIdAsync(command.RegionId, cancellationToken);

            if (region is null)
            {
                return Result.Failure(RegionErrors.NotFound(command.RegionId));
            }

            region.Name = command.Name;

            _regionRepository.Update(region);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
