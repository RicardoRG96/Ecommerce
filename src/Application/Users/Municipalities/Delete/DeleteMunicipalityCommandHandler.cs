using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Municipalities.Delete
{
    internal sealed class DeleteMunicipalityCommandHandler : ICommandHandler<DeleteMunicipalityCommand>
    {
        private readonly IMunicipalityRepository _municipalityRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteMunicipalityCommandHandler(
            IMunicipalityRepository municipalityRepository,
            IUnitOfWork unitOfWork)
        {
            _municipalityRepository = municipalityRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteMunicipalityCommand command, CancellationToken cancellationToken)
        {
            Municipality? municipality = await _municipalityRepository.GetByIdAsync(
                command.MunicipalityId, 
                cancellationToken);

            if (municipality is null)
            {
                return Result.Failure(MunicipalityErrors.NotFound(command.MunicipalityId));
            }

            _municipalityRepository.Delete(municipality);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
