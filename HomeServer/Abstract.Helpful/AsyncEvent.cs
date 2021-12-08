using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Abstract.Helpful.Lib.Utils;
using Autofac;
using Autofac.Builder;

namespace Abstract.Helpful.Lib
{
    public static class AutofacExtensions
    {
        public static void AsSelfAsImplementedInterfacesSingle<T>(this ContainerBuilder builder)
        {
             builder.AsSelfAsImplementedInterfacesSingle(typeof(T));
        }
        
        public static void AsSelfAsImplementedInterfaces<T>(this ContainerBuilder builder)
        {
            builder.RegisterType<T>().AsSelf().AsImplementedInterfaces().FixDisposables();
        }
        
        public static void AsSelfAsImplementedInterfaces(this ContainerBuilder builder, Type type)
        {
            builder.RegisterType(type).AsSelf().AsImplementedInterfaces().FixDisposables();
        }
        
        public static IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> InstanceAsSelfAsImplementedInterfacesSingle<T>(this ContainerBuilder builder, T instance) 
            where T : class
        {
            return builder.RegisterInstance<T>(instance)
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance()
                .FixDisposables(instance.GetType());
        }
        
        public static void AsSelfAsImplementedInterfacesSingle(this ContainerBuilder builder, Type type)
        {
            builder.RegisterType(type)
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance()
                .FixDisposables(type);
        }

        private static void FixDisposables<T>(
            this IRegistrationBuilder<T, ConcreteReflectionActivatorData, SingleRegistrationStyle> builder)
        {
            builder.FixDisposables(typeof(T));
        }
        
        private static IRegistrationBuilder<T1, T2, SingleRegistrationStyle> FixDisposables<T1, T2>(
                  this IRegistrationBuilder<T1, T2, SingleRegistrationStyle> builder, Type type)
        {
            if (typeof(IDisposable).IsAssignableFrom(type))
                return builder.As<IDisposable>();

            return builder;
        }
        
        public static void RegisterByMarker<TMarker>(this ContainerBuilder builder, bool isSingletons, 
            Assembly assembly = null)
        {
            assembly = assembly ?? typeof(TMarker).Assembly;
            var types = assembly.GetTypes();
            var markedTypes = types.Where(x => x.IsClass && !x.IsAbstract && x.IsAssignableTo<TMarker>());
            foreach (var markedType in markedTypes)
                if (isSingletons)
                    builder.AsSelfAsImplementedInterfacesSingle(markedType);
                else
                    builder.AsSelfAsImplementedInterfaces(markedType);
        }
    }
    
    public sealed class AsyncEvent : IAsyncEvent
    {
        private readonly List<AsyncEventSubscriber> _subscribers = new();
        
        public async Task InvokeAsync()
        {
            foreach (var subscriber in _subscribers)
                await subscriber.InvokeAsync();
        }

        public IDisposable Subscribe(AsyncEventSubscriber subscriber)
        {
            _subscribers.Add(subscriber);
            return new DisposableAction(() => _subscribers.Remove(subscriber));
        }
    }
    
    public class AsyncEvent<TArgs> : IAsyncEvent<TArgs>
    {
        private readonly List<AsyncEventSubscriber<TArgs>> _subscribers = new();
        
        public void Skip() {}
        
        public virtual async Task InvokeAsync(TArgs args)
        {
            List<Exception> exceptions = null;
            
            foreach (var subscriber in _subscribers)
            {
                try
                {
                    await subscriber.InvokeAsync(args);
                }
                catch (Exception exception)
                {
                    exceptions = exceptions.EnsureNotNull(_subscribers.Count);
                    exceptions.Add(exception);
                }
            }

            if (exceptions != null)
            {
                if (exceptions.Count == 1)
                    exceptions.First().Rethrow();
                
                throw new AggregateException(exceptions);
            }
        }

        public IDisposable Subscribe(AsyncEventSubscriber<TArgs> subscriber)
        {
            _subscribers.Add(subscriber);
            return new DisposableAction(() => _subscribers.TryRemove(subscriber));
        }
        
        public IDisposable Subscribe(Func<TArgs, Task> asyncAction)
        {
            if (asyncAction == null)
                return new DisposableAction(() => { });
        
            var subscriber = new AsyncEventSubscriber<TArgs>(asyncAction);
            _subscribers.Add(subscriber);
            return new DisposableAction(() => _subscribers.TryRemove(subscriber));
        }
    }
}