using Application.Users.Roles.Assign;
using Application.Users.Roles.AssignPermission;
using Application.Users.Roles.Unassign;
using Application.Users.Roles.UnassignPermission;
using Application.Users.Users.Create;
using Application.Users.Users.UpdatePassword;
using Domain.Entities.Users;
using SharedKernel;

namespace Application.Abstractions.Common
{
    public interface IIdentityService
    {
        Task<IDomainUser> GetUserByIdAsync(long id, CancellationToken cancellationToken);

        Task<IDomainUser> GetUserByEmailAsync(string email);

        Task<IDomainUser> GetByUsernameAsync(string username);

        Task<PaginatedList<IDomainUser>> GetAllUsersAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<Result<long>> CreateUserAsync(CreateUserCommand command);

        Task<Result> LoginUserAsync(string email, string password);

        void Update(IDomainUser user);

        Task<Result> DeleteUserAsync(IDomainUser user);

        Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken);

        Task<bool> IsUserNameUniqueAsync(string username, CancellationToken cancellation);

        Task<Result> UpdatePasswordAsync(UpdatePasswordCommand command, CancellationToken cancellationToken);

        Task<Result> AddRolesToUserAsync(AssignRolesToUserCommand command, CancellationToken cancellationToken);

        Task<Result> RemoveRolesFromUserAsync(UnassignRolesToUserCommand command, CancellationToken cancellationToken);

        Task<Result> AddPermissionToRoleAsync(AssignPermissionToRoleCommand command);

        Task<Result> RemovePermissionToRoleAsync(UnassignPermissionToRoleCommand command);

        Task<Result<long>> CreateRoleAsync(string roleName);

        Task<Result> DeleteRoleAsync(long roleId);
    }
}
