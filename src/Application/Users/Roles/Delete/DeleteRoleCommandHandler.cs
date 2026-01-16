using Application.Abstractions.Common;
using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Users.Roles.Delete
{
    internal sealed class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand>
    {
        private readonly IIdentityService _identityService;

        public DeleteRoleCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result> Handle(DeleteRoleCommand command, CancellationToken cancellationToken)
        {
            return await _identityService.DeleteRoleAsync(command.Id);
        }
    }
}
