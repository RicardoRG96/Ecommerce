using Application.Abstractions.Authentication;
using Application.Abstractions.Common;
using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Addresses.AssignDefaultAddress
{
    internal sealed class AssignDefaultAddressCommandHandler : ICommandHandler<AssignDefaultAddressCommand>
    {
        private readonly IAddressUserRepository _addressUserRepository;
        private readonly IIdentityService _identityService;
        private readonly IAddressRepository _addressRepository;
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _unitOfWork;

        public AssignDefaultAddressCommandHandler(
            IAddressUserRepository addressUserRepository,
            IIdentityService identityService,
            IAddressRepository addressRepository,
            IUserContext userContext, 
            IUnitOfWork unitOfWork)
        {
            _addressUserRepository = addressUserRepository;
            _identityService = identityService;
            _addressRepository = addressRepository;
            _userContext = userContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(AssignDefaultAddressCommand command, CancellationToken cancellationToken)
        {
            if (command.UserId != _userContext.UserId)
            {
                return Result.Failure(UserErrors.Unauthorized());
            }

            IDomainUser user = await _identityService.GetUserByIdAsync(command.UserId, cancellationToken);

            if (user is null)
            {
                return Result.Failure(UserErrors.NotFound(command.UserId));
            }

            Address? adddres = await _addressRepository.GetByIdAsync(command.AddressId, cancellationToken);

            if (adddres is null)
            {
                return Result.Failure(AddressErrors.NotFound(command.AddressId));
            }

            await _addressUserRepository.SetAddressAsDefault(command.UserId, command.AddressId, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
