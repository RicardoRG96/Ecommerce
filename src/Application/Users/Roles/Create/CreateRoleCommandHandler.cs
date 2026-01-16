using Application.Abstractions.Common;
using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Users.Roles.Create
{
    internal sealed class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand, long>
    {
        private readonly IIdentityService _identityService;

        public CreateRoleCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result<long>> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
        {
            return await _identityService.CreateRoleAsync(command.Name);
        }
    }
}
