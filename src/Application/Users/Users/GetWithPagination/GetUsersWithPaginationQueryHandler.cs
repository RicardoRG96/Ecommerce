using Application.Abstractions.Common;
using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using SharedKernel;

namespace Application.Users.Users.GetWithPagination
{
    internal sealed class GetUsersWithPaginationQueryHandler : IQueryHandler<GetUsersWithPaginationQuery, PaginatedList<UserResponse>>
    {
        //private readonly IUserRepository _userRepository;
        private readonly IIdentityService _identityService;

        public GetUsersWithPaginationQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result<PaginatedList<UserResponse>>> Handle(GetUsersWithPaginationQuery query, CancellationToken cancellationToken)
        {
            PaginatedList<IDomainUser> users = await _identityService.GetAllUsersAsync(
                query.PageNumber,
                query.PageSize,
                cancellationToken);

            PaginatedList<UserResponse> paginatedUserResponse = MapToUserResponsePaginatedList(
                users,
                query);

            return Result.Success(paginatedUserResponse);
        }

        private static PaginatedList<UserResponse> MapToUserResponsePaginatedList(
            PaginatedList<IDomainUser> userPaginatedList,
            GetUsersWithPaginationQuery query)
        {
            List<UserResponse> userResponse = userPaginatedList.Items.Select(u =>
            {
                UserResponse userResponse = new()
                {
                    Id = u.Id,
                    Avatar = u.Avatar!,
                    FirstName = u.FirstName!,
                    LastName = u.LastName!,
                    Username = u.UserName!,
                    Email = u.Email!,
                    DateOfBirth = u.DateOfBirth,
                    PhoneNumber = u.PhoneNumber!
                };
                return userResponse;
            }).ToList();

            PaginatedList<UserResponse> paginatedUserResponse = PaginatedList<UserResponse>.Create(
                userResponse,
                userPaginatedList.TotalCount,
                userPaginatedList.PageNumber,
                query.PageSize);

            return paginatedUserResponse;
        }
    }
}
