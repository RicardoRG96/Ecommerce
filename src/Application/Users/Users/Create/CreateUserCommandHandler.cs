using Application.Abstractions.Common;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Users.Create
{
    internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, long>
    {
        private readonly IIdentityService _identityService;
        private readonly IUnitOfWork _unitOfWork;
        
        public CreateUserCommandHandler(
            IIdentityService identityService,
            IUnitOfWork unitOfWork
            )
        {
            _identityService = identityService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<long>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            bool isEmailUnique = await _identityService.IsEmailUnique(command.Email, cancellationToken);
            bool isUsernameUnique = await _identityService.IsUserNameUnique(command.UserName, cancellationToken);

            if (!isEmailUnique)
            {
                return Result.Failure<long>(UserErrors.EmailNotUnique);
            }

            if (!isUsernameUnique)
            {
                return Result.Failure<long>(UserErrors.UsernameNotUnique);
            }

            Result<long> identityResult = await _identityService.CreateUserAsync(command);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return identityResult;
        }
    }
}
