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
            IDomainUser? user = await _userRepository.GetUserByEmailAsync(command.Email, cancellationToken);

            if (user!.Email is not null)
            {
                return Result.Failure<long>(UserErrors.EmailNotUnique);
            }

            if (user!.UserName is not null)
            {
                return Result.Failure<long>(UserErrors.UsernameNotUnique);
            }

            Result<long> identityResult = await _identityService.CreateUserAsync(
                command.Username, 
                command.Email, 
                command.Password);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return identityResult;
        }
    }
}
