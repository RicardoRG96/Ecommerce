using Application.Abstractions.Common;
using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Users.Roles.Update
{
    internal sealed class UpdateRoleCommandHandler : ICommandHandler<UpdateRoleCommand>
    {
        private readonly IIdentityService _identityService;

        public UpdateRoleCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result> Handle(UpdateRoleCommand command, CancellationToken cancellationToken)
        {
            return await _identityService.UpdateRoleAsync(command);
        }
    }
}
