using Application.Abstractions.Common;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Users.GetById
{
    internal sealed class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserResponse>
    {
        private readonly IIdentityService _identitySerice;

        public GetUserByIdQueryHandler(IIdentityService identityService)
        {
            _identitySerice = identityService;
        }

        public async Task<Result<UserResponse>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            long userId = query.UserId;
            IDomainUser? user = await _identitySerice.GetUserByIdAsync(userId, cancellationToken);

            if (user is null)
            {
                return Result.Failure<UserResponse>(UserErrors.NotFound(userId));
            }

            UserResponse userResponse = new()
            {
                Id = user.Id,
                Avatar = user.Avatar!,
                FirstName = user.FirstName!,
                LastName = user.LastName!,
                Username = user.UserName!,
                Email = user.Email!,
                DateOfBirth = user.DateOfBirth,
                PhoneNumber = user.PhoneNumber!
            };

            return Result.Success(userResponse);
        }
    }
}
