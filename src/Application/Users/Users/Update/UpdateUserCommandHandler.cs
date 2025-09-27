using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Users.Update
{
    internal sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateUserCommand command)
        {
            User? user = await _userRepository.GetByIdAsync(command.UserId);

            if (user == null)
            {
                return Result.Failure(UserErrors.NotFound(command.UserId));
            }

            user.Avatar = command.Avatar;
            user.FirstName = command.FirstName;
            user.LastName = command.LastName;
            user.PhoneNumber = command.PhoneNumber;

            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}
