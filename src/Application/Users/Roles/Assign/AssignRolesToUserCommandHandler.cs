using Application.Abstractions.Common;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Roles.Assign
{
    internal sealed class AssignRolesToUserCommandHandler : ICommandHandler<AssignRolesToUserCommand>
    {
        private readonly IIdentityService _identityService;
        private readonly IUnitOfWork _unitOfWork;

        public AssignRolesToUserCommandHandler(
            IIdentityService identityService,
            IUnitOfWork unitOfWork)
        {
            _identityService = identityService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(AssignRolesToUserCommand command, CancellationToken cancellationToken)
        {
            IDomainUser? user = await _identityService.GetUserByIdAsync(command.UserId, cancellationToken);

            if (user is null)
            {
                return Result.Failure(UserErrors.NotFound(command.UserId));
            }

            Result identityResult = await _identityService.AddRolesToUserAsync(command, cancellationToken);

            if (identityResult.IsSuccess)
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return identityResult;
        }
    }
}
