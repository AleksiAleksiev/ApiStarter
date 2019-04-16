namespace NewInterlex.Core
{
    using Autofac;
    using Interfaces.UseCases;
    using UseCases;

    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ExchangeRefreshTokenUseCase>().As<IExchangeRefreshTokenUseCase>()
                .InstancePerLifetimeScope();
            builder.RegisterType<LoginUseCase>().As<ILoginUseCase>().InstancePerLifetimeScope();
            builder.RegisterType<RegisterUserUseCase>().As<IRegisterUserUseCase>().InstancePerLifetimeScope();
        }
    }
}