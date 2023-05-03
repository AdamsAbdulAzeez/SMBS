using Prism.Ioc;

internal class Ioc
{
    internal static T Resolve<T>() => ContainerLocator.Container.Resolve<T>();
}