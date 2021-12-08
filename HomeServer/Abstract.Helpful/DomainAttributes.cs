using System;

namespace Abstract.Helpful.Lib
{

    // Domain: -------------------------------------------------------------------------------------
    
    public sealed class WebDependencyAttribute : Attribute
    {
        /// <summary>
        ///     Is concrete web dependency for solution. 
        /// <para/>
        ///     Usually mocked.
        /// </summary>
        public WebDependencyAttribute()
        {
        }
    }
    public sealed class A_DomainDependencyStorageAttribute : Attribute
    {
        /// <summary>
        ///     Is concrete storage dependency for solution. 
        /// <para/>
        ///     Usually mocked.
        /// </summary>
        public A_DomainDependencyStorageAttribute()
        {
        }
    }
    
    public sealed class ImpureWriteAttribute : Attribute
    {
        /// <summary>
        ///     Attributed method purpose is to perform NOT PURE WRITE operation.
        /// <para/>
        ///     Attributed method is Infrastructure method. 
        /// </summary>
        public ImpureWriteAttribute()
        {
        }
    }
    
    public sealed class ImpureReadAttribute : Attribute
    {
        /// <summary>
        ///     Attributed method purpose is to perform NOT PURE READ operation.
        /// <para/>
        ///     Attributed method is Infrastructure method. 
        /// </summary>
        public ImpureReadAttribute()
        {
        }
    }
    
    public sealed class PureDomainWithDependenciesAttribute : Attribute
    {
        /// <summary>
        ///     Is not a dependency. But use other dependencies through interfaces, not concrete.
        /// <para/>
        ///     Contains Domain logic.
        /// <para/>
        ///     Usually should not be mocked.
        /// </summary>
        public PureDomainWithDependenciesAttribute()
        {
        }

        public Type[] DirectImpureDependencies { get; set; }
    }
    public sealed class PureDomainAttribute : Attribute
    {
        /// <summary>
        ///     Clear domain logic.
        /// <para/>
        ///     Usually should not be mocked.
        /// </summary>
        public PureDomainAttribute()
        {
        }
    }

    public sealed class PureDomainInterfaceAttribute : Attribute
    {
        /// <summary>
        ///     Is not a dependency for others.
        /// <para/>
        ///     Such interfaces exists only for easy testing reason.
        /// </summary>
        public PureDomainInterfaceAttribute()
        {
        }
    }
    public sealed class DomainDependencyInterfaceAttribute : Attribute
    {
        /// <summary>
        ///     Is a dependency for others.
        /// <para/>
        ///     Such interfaces actually helps implement The Dependency Inversion Principle
        /// </summary>
        public DomainDependencyInterfaceAttribute()
        {
        }
    }

    // Service.Infrastructure: -------------------------------------------------------------------------------------
    
    public sealed class A_InfrastructureIndependentHelperAttribute : Attribute
    {
        /// <summary>
        ///     Helps to implement some abstract Service.Infrastructure.
        /// <para/>
        ///     Is not a dependency for others. Can contains any dependency as interfaces, not concrete.
        /// </summary>
        public A_InfrastructureIndependentHelperAttribute()
        {
        }
    }
    public sealed class A_InfrastructureDependencyAttribute : Attribute
    {
        /// <summary>
        ///     Actual implementation of concrete Service.Infrastructure
        /// <para/>
        ///     Is a dependency for others.
        /// </summary>
        public A_InfrastructureDependencyAttribute()
        {
        }
    }
}