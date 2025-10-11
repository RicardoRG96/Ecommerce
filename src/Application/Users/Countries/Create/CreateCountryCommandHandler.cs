using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using SharedKernel;

namespace Application.Users.Countries.Create
{
    internal sealed class CreateCountryCommandHandler : ICommandHandler<CreateCountryCommand, long>
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCountryCommandHandler(ICountryRepository countryRepository, IUnitOfWork unitOfWork)
        {
            _countryRepository = countryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<long>> Handle(CreateCountryCommand command, CancellationToken cancellationToken)
        {
            Country country = new()
            {
                Name = command.Name
            };

            await _countryRepository.AddAsync(country, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(country.CountryId);
        }
    }
}
