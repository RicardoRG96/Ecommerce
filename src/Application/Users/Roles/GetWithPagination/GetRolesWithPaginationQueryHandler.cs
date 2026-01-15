using Application.Abstractions.Common;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using SharedKernel;

namespace Application.Users.Roles.GetWithPagination
{
    internal sealed class GetRolesWithPaginationQueryHandler 
        : IQueryHandler<GetRolesWithPaginationQuery, PaginatedList<RoleResponse>>
    {
        private readonly IIdentityService _identityService;

        public GetRolesWithPaginationQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result<PaginatedList<RoleResponse>>> Handle(GetRolesWithPaginationQuery query, CancellationToken cancellationToken)
        {
            PaginatedList<Role> roles = await _identityService.GetAllRolesAsync(
                query.PageNumber, 
                query.PageSize, 
                cancellationToken);

            PaginatedList<RoleResponse> paginatedRolesResponse = MapToRoleResponsePaginatedList(
                roles,
                query);

            return Result.Success(paginatedRolesResponse);
        }

        private PaginatedList<RoleResponse> MapToRoleResponsePaginatedList(
           PaginatedList<Role> rolesPaginatedList,
           GetRolesWithPaginationQuery query)
        {
            List<RoleResponse> roleResponse = rolesPaginatedList.Items.Select(r =>
            {
                RoleResponse response = new()
                {
                    Id = r.Id,
                    Name = r.Name
                };

                return response;
            }).ToList();

            PaginatedList<RoleResponse> paginatedRoleResponse = PaginatedList<RoleResponse>.Create(
                roleResponse,
                rolesPaginatedList.TotalCount,
                rolesPaginatedList.PageNumber,
                query.PageSize);

            return paginatedRoleResponse;
        }
    }
}
