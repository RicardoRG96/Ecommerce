using Application.Abstractions.Common;
using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Users.Create
{
    internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, long>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityService _identityService;

        public CreateUserCommandHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IIdentityService identityService)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _identityService = identityService;
        }

        public async Task<Result<long>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            bool isEmailUnique = await _userRepository.IsEmailUnique(command.Email, cancellationToken);
            bool isUsernameUnique = await _userRepository.IsUserNameUnique(command.UserName, cancellationToken);

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
