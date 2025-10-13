using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using SharedKernel;

namespace Application.Users.Regions.Create
{
    internal sealed class CreateRegionCommandHandler : ICommandHandler<CreateRegionCommand, long>
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateRegionCommandHandler(IRegionRepository regionRepository, IUnitOfWork unitOfWork)
        {
            _regionRepository = regionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<long>> Handle(CreateRegionCommand command, CancellationToken cancellationToken)
        {
            Region? region = new()
            {
                Name = command.Name,
            };

            await _regionRepository.AddAsync(region, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(region.RegionId);
        }
    }
}
