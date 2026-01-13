using Application.Abstractions.Authentication;
using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Users.Addresses.AssignDefaultAddress
{
    internal sealed class AssignDefaultAddressCommandHandler : ICommandHandler<AssignDefaultAddressCommand>
    {
        private readonly IAddressUserRepository _addressUserRepository;
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _unitOfWork;

        public AssignDefaultAddressCommandHandler(
            IAddressUserRepository addressUserRepository, 
            IUserContext userContext, 
            IUnitOfWork unitOfWork)
        {
            _addressUserRepository = addressUserRepository;
            _userContext = userContext;
            _unitOfWork = unitOfWork;
        }

        public Task<Result> Handle(AssignDefaultAddressCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
