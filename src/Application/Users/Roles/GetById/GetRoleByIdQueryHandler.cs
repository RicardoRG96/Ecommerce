using Application.Abstractions.Common;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Roles.GetById
{
    internal sealed class GetRoleByIdQueryHandler : IQueryHandler<GetRoleByIdQuery, RoleResponse>
    {
        private readonly IIdentityService _identityService;

        public GetRoleByIdQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result<RoleResponse>> Handle(GetRoleByIdQuery query, CancellationToken cancellationToken)
        {
            Role? role = await _identityService.GetRoleByIdAsync(query.Id, cancellationToken);

            if (role is null)
            {
                return Result.Failure<RoleResponse>(RoleErrors.NotFound(query.Id));
            }

            RoleResponse response = new()
            {
                Id = role.Id,
                Name = role.Name
            };

            return Result.Success(response);
        }
    }
}
