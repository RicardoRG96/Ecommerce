using Application.Abstractions.Authentication;
using Application.Abstractions.Common;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Roles.Unassign
{
    internal sealed class UnassignRolesToUserCommandHandler : ICommandHandler<UnassignRolesToUserCommand>
    {
        private readonly IIdentityService _identityService;
        private readonly IUnitOfWork _unitOfWork;

        public UnassignRolesToUserCommandHandler(
            IIdentityService identityService,
            IUnitOfWork unitOfWork)
        {
            _identityService = identityService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UnassignRolesToUserCommand command, CancellationToken cancellationToken)
        {
            IDomainUser user = await _identityService.GetUserByIdAsync(command.UserId, cancellationToken);

            if (user is null)
            {
                return Result.Failure(UserErrors.NotFound(command.UserId));
            }

            Result identityResult = await _identityService.RemoveRolesFromUserAsync(command, cancellationToken);

            if (identityResult.IsSuccess)
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return identityResult;
        }
    }
}
