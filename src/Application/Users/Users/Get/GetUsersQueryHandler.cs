using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using SharedKernel;

namespace Application.Users.Users.Get
{
    internal sealed class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, List<UserResponse>>
    {
        private readonly IUserRepository _userRepository;
        
        public GetUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<List<UserResponse>>> Handle(GetUsersQuery query)
        {
            List<User> users = (List<User>)await _userRepository.GetAllAsync();

            List<UserResponse> usersResponse = users.Select(u =>
            {
                UserResponse userResponse = new()
                {
                    Id = u.UserId,
                    Avatar = u.Avatar!,
                    FirstName = u.FirstName!,
                    LastName = u.LastName!,
                    Username = u.Username!,
                    Email = u.Email!,
                    DateOfBirth = u.DateOfBirth
                };
                return userResponse;
            }).ToList();

            return Result.Success(usersResponse);
        }
    }
}
