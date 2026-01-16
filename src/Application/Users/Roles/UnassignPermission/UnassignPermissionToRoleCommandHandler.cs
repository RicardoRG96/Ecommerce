using Application.Abstractions.Common;
using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Users.Roles.UnassignPermission
{
    internal sealed class UnassignPermissionToRoleCommandHandler : ICommandHandler<UnassignPermissionToRoleCommand>
    {
        private readonly IIdentityService _identityService;

        public UnassignPermissionToRoleCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result> Handle(UnassignPermissionToRoleCommand command, CancellationToken cancellationToken)
        {
            return await _identityService.RemovePermissionToRoleAsync(command);
        }
    }
}
