using Application.Abstractions.Authentication;
using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Addresses.Update
{
    internal sealed class UpdateAddressCommandHandler : ICommandHandler<UpdateAddressCommand>
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMunicipalityRepository _municipalityRepository;
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAddressCommandHandler(
            IAddressRepository addressRepository,
            IMunicipalityRepository municipalityRepository,
            IUserContext userContext,
            IUnitOfWork unitOfWork)
        {
            _addressRepository = addressRepository;
            _municipalityRepository = municipalityRepository;
            _userContext = userContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateAddressCommand command, CancellationToken cancellationToken)
        {
            Address? address = await _addressRepository.GetByIdIncludingAddressUserAsync(command.AddressId, cancellationToken);

            if (address is null)
            {
                return Result.Failure(AddressErrors.NotFound(command.AddressId));
            }

            long userId = address.AddressUsers.Select(au => au.ApplicationUserId).FirstOrDefault();

            if (userId != _userContext.UserId)
            {
                return Result.Failure<AddressResponse>(UserErrors.Unauthorized());
            }

            Municipality? municipality = await _municipalityRepository.GetByIdAsync(command.MunicipalityId, cancellationToken);

            if (municipality is null)
            {
                return Result.Failure(MunicipalityErrors.NotFound(command.MunicipalityId));
            }

            address.Municipality = municipality;
            address.Title = command.Title;
            address.City = command.City;
            address.Street = command.Street;
            address.Number = command.Number;
            address.Apartament = command.Apartament;
            address.Reference = command.Reference;
            address.PostalCode = command.PostalCode;

            _addressRepository.Update(address);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
