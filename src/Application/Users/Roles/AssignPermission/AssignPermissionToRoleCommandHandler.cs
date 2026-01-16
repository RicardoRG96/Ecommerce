using Application.Abstractions.Common;
using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Users.Roles.AssignPermission
{
    internal sealed class AssignPermissionToRoleCommandHandler : ICommandHandler<AssignPermissionToRoleCommand>
    {
        private readonly IIdentityService _identityService;

        public AssignPermissionToRoleCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result> Handle(AssignPermissionToRoleCommand command, CancellationToken cancellationToken)
        {
            return await _identityService.AddPermissionToRoleAsync(command);
        }
    }
}
