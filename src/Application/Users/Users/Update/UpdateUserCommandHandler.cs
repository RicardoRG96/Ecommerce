using Application.Abstractions.Authentication;
using Application.Abstractions.Common;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Users.Update
{
    internal sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand>
    {
        private readonly IIdentityService _identityService;
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserCommandHandler(
            IIdentityService identityService,
            IUserContext userContext,
            IUnitOfWork unitOfWork)
        {
            _identityService = identityService;
            _userContext = userContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            if (command.UserId != _userContext.UserId)
            {
                return Result.Failure(UserErrors.Unauthorized());
            }

            IDomainUser? user = await _identityService.GetUserByIdAsync(command.UserId, cancellationToken);

            if (user is null)
            {
                return Result.Failure(UserErrors.NotFound(command.UserId));
            }

            user.Avatar = command.Avatar!;
            user.FirstName = command.FirstName;
            user.LastName = command.LastName;
            user.PhoneNumber = command.PhoneNumber;

            _identityService.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
