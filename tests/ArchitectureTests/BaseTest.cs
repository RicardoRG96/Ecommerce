using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Infrastructure.Persistence.Database;
using System.Reflection;
using Web.Api;

namespace ArchitectureTests
{
    public abstract class BaseTest
    {
        protected static readonly Assembly DomainAssembly = typeof(IDomainUser).Assembly;
        protected static readonly Assembly ApplicationAssembly = typeof(ICommand).Assembly;
        protected static readonly Assembly InfrastructureAssembly = typeof(ApplicationDbContext).Assembly;
        protected static readonly Assembly PresentationAssembly = typeof(Program).Assembly;
    }
}
