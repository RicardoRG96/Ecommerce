using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Users.GetById
{
    internal sealed class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserResponse>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<UserResponse>> Handle(GetUserByIdQuery query)
        {
            long userId = query.UserId;
            User? user = await _userRepository.GetByIdAsync(userId);

            if (user is null)
            {
                return Result.Failure<UserResponse>(UserErrors.NotFound(userId));
            }

            UserResponse userResponse = new()
            {
                Id = user.UserId,
                Avatar = user.Avatar!,
                FirstName = user.FirstName!,
                LastName = user.LastName!,
                Username = user.Username!,
                Email = user.Email!,
                DateOfBirth = user.DateOfBirth,
                PhoneNumber = user.PhoneNumber!
            };

            return Result.Success(userResponse);
        }
    }
}
