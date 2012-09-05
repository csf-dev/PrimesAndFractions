using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CSF.Reflection;
using NUnit.Framework;
using System.Text;

namespace CSF.Testing.Reflection
{
  /// <summary>
  /// Testing helper type that checks assemblies and ensures that all of the types contained within are decorated with
  /// a given attribute.
  /// </summary>
  public class CheckClassAttributes<TAttribute> where TAttribute : Attribute
  {
    #region fields

    private Assembly _assembly;

    #endregion

    #region properties

    /// <summary>
    /// Gets the assembly to check.
    /// </summary>
    /// <value>
    /// The assembly.
    /// </value>
    protected Assembly Assembly
    {
      get {
        return _assembly;
      }
      private set {
        if(value == null)
        {
          throw new ArgumentNullException("value");
        }

        _assembly = value;
      }
    }

    /// <summary>
    /// Gets a list of the types that are ignored/skipped in the checking process.
    /// </summary>
    /// <value>
    /// The ignored types.
    /// </value>
    protected IList<Type> IgnoredTypes
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets a list of the namespaces that are ignored/skipped in the checking process.
    /// </summary>
    /// <value>
    /// The ignored namespaces.
    /// </value>
    protected IList<string> IgnoredNamespaces
    {
      get;
      private set;
    }

    #endregion

    #region methods

    /// <summary>
    /// Adds a new type to the list of types that are ignored by the checking process.
    /// </summary>
    /// <typeparam name='TType'>
    /// A type to ignore.
    /// </typeparam>
    public CheckClassAttributes<TAttribute> Except<TType>()
    {
      return this.Except(typeof(TType));
    }

    /// <summary>
    /// Adds a new type to the list of types that are ignored by the checking process.
    /// </summary>
    /// <param name='type'>
    /// A type to ignore.
    /// </param>
    public CheckClassAttributes<TAttribute> Except(Type type)
    {
      if(type == null)
      {
        throw new ArgumentNullException("type");
      }

      if(!this.IgnoredTypes.Contains(type))
      {
        this.IgnoredTypes.Add(type);
      }

      return this;
    }

    /// <summary>
    /// Adds a namespace, under which all types will be ignored in the checking process.
    /// </summary>
    /// <typeparam name='TType'>
    /// A type, the namespace of which will be ignored.
    /// </typeparam>
    public CheckClassAttributes<TAttribute> ExceptNamespace<TType>()
    {
      return this.ExceptNamespace(typeof(TType).Namespace);
    }

    /// <summary>
    /// Adds a namespace, under which all types will be ignored in the checking process.
    /// </summary>
    /// <param name='type'>
    /// A type, the namespace of which will be ignored.
    /// </param>
    public CheckClassAttributes<TAttribute> ExceptNamespace(Type type)
    {
      if(type == null)
      {
        throw new ArgumentNullException("type");
      }

      return this.ExceptNamespace(type.Namespace);
    }

    /// <summary>
    /// Adds a namespace, under which all types will be ignored in the checking process.
    /// </summary>
    /// <param name='typeNamespace'>
    /// A namespace to ignore.
    /// </param>
    public CheckClassAttributes<TAttribute> ExceptNamespace(string typeNamespace)
    {
      if(typeNamespace == null)
      {
        throw new ArgumentNullException("typeNamespace");
      }

      if(!this.IgnoredNamespaces.Contains(typeNamespace))
      {
        this.IgnoredNamespaces.Add(typeNamespace);
      }

      return this;
    }

    /// <summary>
    /// Performs all of the checks.  If the assembly contains any types that are not decorated with the selected
    /// attribute and that also are not ignored by type or by namespace then an <see cref="AssertionException"/> will be
    /// raised.
    /// </summary>
    public void ExecuteChecks()
    {
      List<Type> failures = new List<Type>();
      StringBuilder message = new StringBuilder();

      foreach(Type type in this.Assembly.GetTypes())
      {
        if(!type.IsClass)
        {
          continue;
        }

        if(this.IgnoredTypes.Contains(type))
        {
          continue;
        }

        if(this.IgnoredNamespaces.Any(x => type.Namespace.StartsWith(x)))
        {
          continue;
        }

        if(!type.HasAttribute<TAttribute>())
        {
          failures.Add(type);
        }
      }

      if(failures.Count > 0)
      {
        message.AppendFormat("Failed checks for {0}, types that failed are:", typeof(TAttribute).Name);
        foreach(Type type in failures)
        {
          message.AppendFormat("\n{0}", type.FullName);
        }
        Assert.Fail(message.ToString());
      }
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <c>CSF.Testing.Reflection.CheckClassAttributes`1</c> class.
    /// </summary>
    /// <param name='assembly'>
    /// The assembly to check.
    /// </param>
    protected CheckClassAttributes(Assembly assembly)
    {
      this.Assembly = assembly;
      this.IgnoredTypes = new List<Type>();
      this.IgnoredNamespaces = new List<string>();
    }

    #endregion

    #region static factory methods

    /// <summary>
    /// Creates a new checking instance from an assembly.
    /// </summary>
    /// <returns>
    /// A class-checking instance.
    /// </returns>
    /// <param name='assembly'>
    /// The assembly.
    /// </param>
    public static CheckClassAttributes<TAttribute> FromAssembly(Assembly assembly)
    {
      return new CheckClassAttributes<TAttribute>(assembly);
    }

    /// <summary>
    /// Creates a new checking instance from an assembly.
    /// </summary>
    /// <returns>
    /// A class-checking instance.
    /// </returns>
    /// <typeparam name='TType'>
    /// A type that is contained by the assembly to check.
    /// </typeparam>
    public static CheckClassAttributes<TAttribute> FromAssemblyOfType<TType>()
    {
      return FromAssembly(typeof(TType).Assembly);
    }

    #endregion
  }
}

