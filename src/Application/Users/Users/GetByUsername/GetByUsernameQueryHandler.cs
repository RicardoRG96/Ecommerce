using Application.Abstractions.Common;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Users.GetByUsername
{
    internal sealed class GetByUsernameQueryHandler : IQueryHandler<GetByUsernameQuery, UserResponse>
    {
        private readonly IIdentityService _identityService;

        public GetByUsernameQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result<UserResponse>> Handle(GetByUsernameQuery query, CancellationToken cancellationToken)
        {
            IDomainUser? user = await _identityService.GetByUsernameAsync(query.Username);

            if (user is null)
            {
                return Result.Failure<UserResponse>(UserErrors.NotFoundByUsername);
            }

            UserResponse userResponse = new()
            {
                Id = user.Id,
                Avatar = user.Avatar!,
                FirstName = user.FirstName!,
                LastName = user.LastName!,
                Username = user.UserName!,
                Email = user.Email!,
                DateOfBirth = user.DateOfBirth
            };

            return Result.Success(userResponse);
        }
    }
}
