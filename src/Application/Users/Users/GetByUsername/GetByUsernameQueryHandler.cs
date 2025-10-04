using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Users.GetByUsername
{
    internal sealed class GetByUsernameQueryHandler : IQueryHandler<GetByUsernameQuery, UserResponse>
    {
        private readonly IUserRepository _userRepository;

        public GetByUsernameQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<UserResponse>> Handle(GetByUsernameQuery query)
        {
            User? user = await _userRepository.GetByUsernameAsync(query.Username);

            if (user is null)
            {
                return Result.Failure<UserResponse>(UserErrors.NotFoundByUsername);
            }

            UserResponse userResponse = new()
            {
                Id = user.UserId,
                Avatar = user.Avatar!,
                FirstName = user.FirstName!,
                LastName = user.LastName!,
                Username = user.Username!,
                Email = user.Email!,
                DateOfBirth = user.DateOfBirth
            };

            return Result.Success(userResponse);
        }
    }
}
