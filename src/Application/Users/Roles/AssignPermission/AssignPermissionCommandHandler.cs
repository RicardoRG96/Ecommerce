using Application.Abstractions.Common;
using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Users.Roles.AssignPermission
{
    internal sealed class AssignPermissionCommandHandler : ICommandHandler<AssignPermissionCommand>
    {
        private readonly IIdentityService _identityService;

        public AssignPermissionCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result> Handle(AssignPermissionCommand command, CancellationToken cancellationToken)
        {
            return await _identityService.AddPermissionToRole(command, cancellationToken);
        }
    }
}
