using Application.Abstractions.Authentication;
using Application.Abstractions.Common;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Users.Users.UpdatePassword
{
    internal sealed class UpdatePasswordCommandHandler : ICommandHandler<UpdatePasswordCommand>
    {
        private readonly IIdentityService _identityService;
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePasswordCommandHandler(
            IIdentityService identityService,
            IUserContext userContext,
            IUnitOfWork unitOfWork)
        {
            _identityService = identityService;
            _userContext = userContext;
            _unitOfWork = unitOfWork;
        }

        public Task<Result> Handle(UpdatePasswordCommand command, CancellationToken cancellationToken)
        {
            
        }
    }
}
