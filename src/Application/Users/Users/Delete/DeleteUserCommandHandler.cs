using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Users.Delete
{
    internal sealed class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteUserCommand command)
        {
            User? user = await _userRepository.GetByIdAsync(command.UserId);

            if (user is null)
            {
                return Result.Failure(UserErrors.NotFound(command.UserId));
            }

            _userRepository.Delete(user);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}
