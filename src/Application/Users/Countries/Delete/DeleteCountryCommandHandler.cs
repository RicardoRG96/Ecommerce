using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Countries.Delete
{
    internal sealed class DeleteCountryCommandHandler : ICommandHandler<DeleteCountryCommand>
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCountryCommandHandler(ICountryRepository countryRepository, IUnitOfWork unitOfWork)
        {
            _countryRepository = countryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteCountryCommand command, CancellationToken cancellationToken)
        {
            Country? country = await _countryRepository.GetByIdAsync(command.Id, cancellationToken);

            if (country is null)
            {
                return Result.Failure(CountryErrors.NotFound(command.Id));
            }

            _countryRepository.Delete(country);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
