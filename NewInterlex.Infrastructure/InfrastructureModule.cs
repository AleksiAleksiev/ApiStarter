namespace NewInterlex.Infrastructure
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Auth;
    using Autofac;
    using Autofac.Core.Activators.Reflection;
    using Core.Interfaces.Gateways.Repositories;
    using Core.Interfaces.Services;
    using Data.Repositories;
    using Interfaces;
    using Module = Autofac.Module;

    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<JwtFactory>().As<IJwtFactory>().SingleInstance()
                .FindConstructorsWith(new InternalConstructorFinder());
            builder.RegisterType<JwtTokenHandler>().As<IJwtTokenHandler>().SingleInstance()
                .FindConstructorsWith(new InternalConstructorFinder());
            builder.RegisterType<TokenFactory>().As<ITokenFactory>().SingleInstance();
            builder.RegisterType<JwtTokenValidator>().As<IJwtTokenValidator>().SingleInstance()
                .FindConstructorsWith(new InternalConstructorFinder());
        }
    }

    public class InternalConstructorFinder : IConstructorFinder
    {
        public ConstructorInfo[] FindConstructors(Type targetType)
        {
            return targetType.GetTypeInfo().DeclaredConstructors.Where(c => !c.IsPrivate && !c.IsPublic).ToArray();
        }
    }
}