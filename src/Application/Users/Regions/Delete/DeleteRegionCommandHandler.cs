using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Regions.Delete
{
    internal sealed class DeleteRegionCommandHandler : ICommandHandler<DeleteRegionCommand>
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRegionCommandHandler(IRegionRepository regionRepository, IUnitOfWork unitOfWork)
        {
            _regionRepository = regionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteRegionCommand command, CancellationToken cancellationToken)
        {
            Region? region = await _regionRepository.GetByIdAsync(command.RegionId, cancellationToken);

            if (region is null)
            {
                return Result.Failure(RegionErrors.NotFound(command.RegionId));
            }

            _regionRepository.Delete(region);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
