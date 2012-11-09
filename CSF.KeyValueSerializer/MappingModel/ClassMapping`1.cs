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
using System.Linq;
using CSF.Reflection;
using System.Linq.Expressions;

namespace CSF.KeyValueSerializer.MappingModel
{
  /// <summary>
  /// Default implementation of a class mapping.
  /// </summary>
  public class ClassMapping<TValue> : MappingBase<TValue>, IClassMapping<TValue>
    where TValue : class
  {
    #region fields

    private ICollection<IMapping> _mappings;
    private Func<TValue> _factoryMethod;

    #endregion

    #region properties

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
    public virtual Func<TValue> FactoryMethod
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

    #region methods

    /// <summary>
    /// Validates this mapping instance. 
    /// </summary>
    /// <remarks>
    /// <para>
    /// A class mapping instance is valid if EITHER <see cref="MapAs"/> contains a mapping OR if <see cref="Mappings"/>
    /// contains one or more property mappings.  It is not valid if either both of these properties are null/empty or if
    /// both contain data; these two properties are mutually exclusive.
    /// </para>
    /// <para>
    /// This method also executes the validation method of the base type.
    /// </para>
    /// </remarks>
    public override void Validate()
    {
      base.Validate();

      if(this.Mappings.Count > 0 && this.MapAs != null)
      {
        Type targetType = typeof(TValue);
        string message = String.Format("Mapping for type `{0}' is invalid.  Cannot combine property mappings with a " +
                                       "mapping for the whole type.",
                                       targetType.FullName);
        throw new InvalidMappingException(message);
      }
      else if(this.Mappings.Count == 0 && this.MapAs == null)
      {
        Type targetType = typeof(TValue);
        string message = String.Format("Mapping for type `{0}' is invalid.  Mapping must contain at least one child " +
                                       "mapping.",
                                       targetType.FullName);
        throw new InvalidMappingException(message);
      }
    }

    /// <summary>
    /// Gets the mapping for the current item (the map-as).
    /// </summary>
    /// <returns>
    /// The mapping.
    /// </returns>
    public override IMapping GetMapping()
    {
      if(this.MapAs == null)
      {
        throw new InvalidOperationException("This class-like mapping is not mapped as another mapping.  Did you mean " +
                                            "to use the GetMapping overload that uses a property identifier?");
      }

      return this.MapAs;
    }

    /// <summary>
    /// Gets the mapping for the given property.
    /// </summary>
    /// <returns>
    /// The mapping.
    /// </returns>
    /// <param name='property'>
    /// An expression that indicates the property to retrieve the mapping for.
    /// </param>
    /// <typeparam name='TOuterObject'>
    /// The type associated with the current mapping (the type that 'hosts' the property).
    /// </typeparam>
    public override IMapping GetMapping<TOuterObject>(Expression<Func<TOuterObject, object>> property)
    {
      if(this.MapAs != null)
      {
        throw new InvalidOperationException("This class-like mapping is as another mapping.  Did you mean " +
                                            "to use the parameterless GetMapping overload?");
      }

      PropertyInfo propInfo = Reflect.Property<TOuterObject>(property);

      if(propInfo == null)
      {
        throw new ArgumentException("The property expression given does not indicate a valid property.");
      }

      IMapping output = this.Mappings.Where(x => x.Property == propInfo).FirstOrDefault();

      if(output == null)
      {
        throw new InvalidOperationException("This class-mapping does not contain a mapping for the given property.");
      }

      return output;
    }

    /// <summary>
    ///  Deserialize the specified data as an object instance. 
    /// </summary>
    /// <returns>
    ///  A value that indicates whether deserialization was successful or not. 
    /// </returns>
    /// <param name='data'>
    ///  The dictionary/collection of string data to deserialize from. 
    /// </param>
    /// <param name='result'>
    /// The output/deserialized object instance.  If the return value is false (unsuccessful deserialization) then the
    /// output value of this parameter is undefined.
    /// </param>
    /// <param name='collectionIndices'>
    ///  A collection of integers, indicating the indices of any collection mappings passed-through during the 
    /// </param>
    public override bool Deserialize(IDictionary<string, string> data,
                                     out TValue result,
                                     int[] collectionIndices)
    {
      bool output = false;

      result = default(TValue);

      this.Validate();

      if(this.SatisfiesFlag(data))
      {
        if(this.MapAs != null)
        {
          try
          {
            object tempResult;
            if(this.MapAs.Deserialize(data, out tempResult, collectionIndices))
            {
              result = (TValue) tempResult;
              output = true;
            }
          }
          catch(InvalidMappingException) { throw; }
          catch(Exception) { }
        }
        else
        {
          bool failed = false, iterationPass = false;

          try
          {
            result = this.FactoryMethod();
          }
          catch(Exception)
          {
            failed = true;
          }

          if(!failed)
          {
            foreach(IMapping mapping in this.Mappings.Where(x => x.Property != null))
            {
              try
              {
                object tempResult;
                if(mapping.Deserialize(data, out tempResult, collectionIndices))
                {
                  mapping.Property.SetValue(result, tempResult, null);
                  iterationPass = true;
                }
              }
              catch(InvalidMappingException) { throw; }
              catch(MandatorySerializationException)
              {
                failed = true;
              }
              catch(Exception) {}

              if(failed)
              {
                break;
              }
            }
          }

          if(failed || !iterationPass)
          {
            result = default(TValue);
          }
          else
          {
            output = true;
          }
        }
      }

      if(!output && this.Mandatory)
      {
        throw new MandatorySerializationException(this);
      }

      return output;
    }

    /// <summary>
    ///  Serialize the specified data, exposing the serialized results as an output parameter. 
    /// </summary>
    /// <param name='data'>
    ///  The object graph to serialize, the root of which should be an object instance that corresponds to the current
    /// mapping. 
    /// </param>
    /// <param name='result'>
    ///  The dictionary of string values containing the serialized data that is created from the current mapping
    /// instance. 
    /// </param>
    /// <param name='collectionIndices'>
    ///  A collection of integers, indicating the indices of any collection mappings passed-through during the
    /// serialization process. 
    /// </param>
    /// <returns>
    ///  A value that indicates whether or not the serialization was successful. 
    /// </returns>
    public override bool Serialize(TValue data, out IDictionary<string,string> result, int[] collectionIndices)
    {
      result = new Dictionary<string, string>();
      bool output;

      if(data == null)
      {
        output = false;
      }
      else if(this.MapAs != null)
      {
        output = this.MapAs.Serialize(data, out result, collectionIndices);
      }
      else
      {
        output = false;

        foreach(IMapping mapping in this.Mappings)
        {
          object propertyValue = mapping.Property.GetValue(data, null);
          IDictionary<string,string> propertySerialization;
          bool propertySuccess;

          try
          {
            propertySuccess = mapping.Serialize(propertyValue, out propertySerialization, collectionIndices);
          }
          catch(MandatorySerializationException)
          {
            output = false;
            break;
          }

          if(propertySuccess)
          {
            foreach(string key in propertySerialization.Keys)
            {
              result.Add(key, propertySerialization[key]);
            }
            output = true;
          }
        }
      }

      if(!output)
      {
        result = null;
      }
      else
      {
        this.WriteFlag(result);
      }

      return output;
    }

    /// <summary>
    /// The default factory method used to populate <see cref="FactoryMethod"/>.
    /// </summary>
    /// <returns>
    /// An instance of the generic type that this mapping represents.
    /// </returns>
    protected virtual TValue DefaultFactoryMethod()
    {
      Type targetType = typeof(TValue);
      ConstructorInfo constructor = targetType.GetConstructor(Type.EmptyTypes);

      if(constructor == null)
      {
        string message = String.Format("Cannot construct an instance of type `{0}' as it does not have a public " +
                                       "parameterless constructor.  Did you mean to use a custom factory method for " +
                                       "creating instances of this type?",
                                       targetType.FullName);
        throw new InvalidOperationException(message);
      }

      return (TValue) constructor.Invoke(null);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the the class-mapping type.
    /// </summary>
    /// <param name='parentMapping'>
    /// The 'parent' mapping.
    /// </param>
    /// <param name='property'>
    /// The associated property.
    /// </param>
    public ClassMapping(IMapping parentMapping, PropertyInfo property) : this(parentMapping, property, false) {}

    /// <summary>
    /// Initializes a new instance of the the class-mapping type.
    /// </summary>
    public ClassMapping() : this(null, null, true) {}

    /// <summary>
    /// Initializes a new instance of the the class-mapping type.
    /// </summary>
    /// <param name='parentMapping'>
    /// The 'parent' mapping.
    /// </param>
    /// <param name='property'>
    /// The associated property.
    /// </param>
    /// <param name='isRootMapping'>
    /// A value that indicates whether or not this mapping is the root of the hierarchy.
    /// </param>
    protected ClassMapping(IMapping parentMapping,
                           PropertyInfo property,
                           bool isRootMapping) : base(parentMapping, property, isRootMapping)
    {
      this.MapAs = null;
      this.Mappings = new List<IMapping>();
      this.FactoryMethod = DefaultFactoryMethod;
    }

    #endregion
  }
}

