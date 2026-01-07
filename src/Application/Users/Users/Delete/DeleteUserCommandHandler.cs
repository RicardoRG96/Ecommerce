using Application.Abstractions.Common;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Users.Delete
{
    internal sealed class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
    {
        private readonly IIdentityService _identityService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserCommandHandler(IIdentityService identityService, IUnitOfWork unitOfWork)
        {
            _identityService = identityService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            IDomainUser? user = await _identityService.GetUserByIdAsync(command.UserId, cancellationToken);

            if (user is null)
            {
                return Result.Failure(UserErrors.NotFound(command.UserId));
            }

            Result identityResult = await _identityService.DeleteUserAsync(user);
            
            if (identityResult.IsSuccess)
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            
            return identityResult;
        }
    }
}
