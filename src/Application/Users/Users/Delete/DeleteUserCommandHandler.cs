using Application.Abstractions.Common;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
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
            Result identityResult = await _identityService.DeleteUserAsync(command.UserId);
            
            if (identityResult.IsSuccess)
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            
            return identityResult;
        }
    }
}
