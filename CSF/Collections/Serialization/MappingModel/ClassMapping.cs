//
//  ClassMapping.cs
//
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
//
//  Copyright (c) 2012 Craig Fowler
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CSF.Collections.Serialization.MappingModel
{
  /// <summary>
  /// Default implementation of a class mapping.
  /// </summary>
  public class ClassMapping<TObject> : MappingBase, IClassMapping<TObject>
    where TObject : class
  {
    #region fields

    private ICollection<IMapping> _mappings;
    private Func<TObject> _factoryMethod;

    #endregion

    #region MappingBase overrides

    /// <summary>
    ///  Validates this mapping instance. 
    /// </summary>
    /// <remarks>
    /// <para>
    /// A class mapping instance is invalid if both <see cref="MapAs"/> is non-null and <see cref="Mappings"/> is not
    /// empty.  These two properties are mutually exclusive.
    /// </para>
    /// </remarks>
    public override void Validate()
    {
      base.Validate();

      if(this.Mappings.Count > 0 && this.MapAs != null)
      {
        Type targetType = typeof(TObject);
        string message = String.Format("Mapping for type `{0}' is invalid.  Cannot combine property mappings with a " +
                                       "mapping for the whole type.",
                                       targetType.FullName);
        throw new InvalidOperationException(message);
      }
    }

    #endregion

    #region IClassMapping implementation

    /// <summary>
    ///  If non-null then this entire object is mapped for serialization/deserialization using a single mapping
    /// instance. 
    /// </summary>
    /// <value>
    ///  The mapping for this entire type. 
    /// </value>
    public virtual IMapping MapAs
    {
      get;
      set;
    }

    /// <summary>
    ///  Gets a collection of the mappings for this type. 
    /// </summary>
    /// <value>
    ///  The mappings. 
    /// </value>
    public virtual ICollection<IMapping> Mappings
    {
      get {
        return _mappings;
      }
      set {
        if(value == null)
        {
          throw new ArgumentNullException("value");
        }

        _mappings = value;
      }
    }

    /// <summary>
    ///  Gets the factory method that is used for constructing instances of this type. 
    /// </summary>
    /// <value>
    ///  The factory method. 
    /// </value>
    public virtual Func<TObject> FactoryMethod
    {
      get {
        return _factoryMethod;
      }
      set {
        if(value == null)
        {
          throw new ArgumentNullException("value");
        }

        _factoryMethod = value;
      }
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the the class-mapping type.
    /// </summary>
    public ClassMapping(IMapping parentMapping, PropertyInfo property) : base(parentMapping, property)
    {
      this.MapAs = null;
      this.Mappings = new List<IMapping>();

      this.FactoryMethod = () => {
        Type targetType = typeof(TObject);
        ConstructorInfo constructor = targetType.GetConstructor(Type.EmptyTypes);

        if(constructor == null)
        {
          string message = String.Format("Cannot construct an instance of type `{0}' as it does not have a public " +
                                         "parameterless constructor.  Did you mean to use a custom factory method for " +
                                         "creating instances of this type?",
                                         targetType.FullName);
          throw new InvalidOperationException(message);
        }

        return (TObject) constructor.Invoke(null);
      };
    }

    /// <summary>
    /// Initializes a new instance of the the class-mapping type.
    /// </summary>
    public ClassMapping() : this(null, null)
    {
      this.PermitNullParent = true;
      this.PermitNullProperty = true;
    }

    #endregion
  }
}

