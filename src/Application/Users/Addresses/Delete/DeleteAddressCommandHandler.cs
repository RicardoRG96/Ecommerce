using Application.Abstractions.Authentication;
using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Addresses.Delete
{
    internal sealed class DeleteAddressCommandHandler : ICommandHandler<DeleteAddressCommand>
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAddressCommandHandler(
            IAddressRepository addressRepository,
            IUserContext userContext,
            IUnitOfWork unitOfWork)
        {
            _addressRepository = addressRepository;
            _userContext = userContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteAddressCommand command, CancellationToken cancellationToken)
        {
            Address? address = await _addressRepository.GetByIdIncludingAddressUserAsync(
                command.AddressId, cancellationToken);

            if (address is null)
            {
                return Result.Failure(AddressErrors.NotFound(command.AddressId));
            }

            long userId = address.AddressUsers.Select(au => au.ApplicationUserId).FirstOrDefault();

            if (userId != _userContext.UserId)
            {
                return Result.Failure<AddressResponse>(UserErrors.Unauthorized());
            }

            _addressRepository.Delete(address);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
