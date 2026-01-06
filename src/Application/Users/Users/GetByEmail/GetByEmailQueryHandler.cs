using Application.Abstractions.Common;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Users.GetByEmail
{
    internal sealed class GetByEmailQueryHandler : IQueryHandler<GetByEmailQuery, UserResponse>
    {
        private readonly IIdentityService _identityService;

        public GetByEmailQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result<UserResponse>> Handle(GetByEmailQuery query, CancellationToken cancellationToken)
        {
            IDomainUser? user = await _identityService.GetUserByEmailAsync(query.Email);

            if (user is null)
            {
                return Result.Failure<UserResponse>(UserErrors.NotFoundByEmail);
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
