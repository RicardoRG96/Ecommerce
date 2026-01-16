using Application.Abstractions.Common;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Roles.GetByName
{
    internal sealed class GetRoleByNameQueryHandler : IQueryHandler<GetRoleByNameQuery, RoleResponse>
    {
        private readonly IIdentityService _identityService;

        public GetRoleByNameQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result<RoleResponse>> Handle(GetRoleByNameQuery query, CancellationToken cancellationToken)
        {
            Role? role = await _identityService.GetRoleByNameAsync(query.Name, cancellationToken);

            if (role is null)
            {
                return Result.Failure<RoleResponse>(RoleErrors.NotFoundByName(query.Name));
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
