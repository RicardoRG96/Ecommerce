using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Users.GetByEmail
{
    internal sealed class GetByEmailQueryHandler : IQueryHandler<GetByEmailQuery, UserResponse>
    {
        private readonly IUserRepository _userRepository;

        public GetByEmailQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<UserResponse>> Handle(GetByEmailQuery query)
        {
            User? user = await _userRepository.GetUserByEmailAsync(query.Email);

            if (user is null)
            {
                return Result.Failure<UserResponse>(UserErrors.NotFoundByEmail);
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
