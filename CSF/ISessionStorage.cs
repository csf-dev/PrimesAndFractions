//  
//  ISessionStorage.cs
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

namespace CSF
{
  /// <summary>
  /// Interface for a type that provides session storage.
  /// </summary>
  public interface ISessionStorage
  {
    #region storage and retrieval
    
    /// <summary>
    /// Stores a value in this instance, at the specified key.
    /// </summary>
    /// <param name='key'>
    /// The identifier under which to store this value.
    /// </param>
    /// <param name='value'>
    /// The value/data to store.
    /// </param>
    /// <typeparam name='TValue'>
    /// The type of the <paramref name="value"/>.
    /// </typeparam>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="key"/> is null.
    /// </exception>
    void Store<TValue>(string key, TValue value);
    
    /// <summary>
    /// Removes any values that may have been present at the specified key.
    /// </summary>
    /// <param name='key'>
    /// The identifier of the value to remove.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="key"/> is null.
    /// </exception>
    void Remove(string key);
    
    /// <summary>
    /// Gets a previously-stored value at the specified key.
    /// </summary>
    /// <param name='key'>
    /// The identifier from which to retrieve the value.
    /// </param>
    /// <typeparam name='TValue'>
    /// The type of the value expected.
    /// </typeparam>
    /// <exception cref="ArgumentException">
    /// Thrown if there is no value stored against the specified <paramref name="key"/>.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="key"/> is null.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// Thrown if the value stored against the specified <paramref name="key"/> is not of the expected generic type.
    /// </exception>
    TValue Get<TValue>(string key);
    
    /// <summary>
    /// Gets a previously-stored value at the specified key.
    /// </summary>
    /// <returns>
    /// A value that indicates whether or not the operation was successful.
    /// </returns>
    /// <param name='key'>
    /// The identifier from which to retrieve the value.
    /// </param>
    /// <param name='output'>
    /// The value returned by this operation.  If the return value of this method is <c>false</c> then this value is
    /// undefined.
    /// </param>
    /// <typeparam name='TValue'>
    /// The type of output value expected.
    /// </typeparam>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="key"/> is null.
    /// </exception>
    bool TryGet<TValue>(string key, out TValue output);
    
    #endregion
    
    #region life-cycle
    
    /// <summary>
    /// Abandons this instance and clears all contained values.  The state of the current instance after calling this
    /// method is undefined.
    /// </summary>
    void Abandon();

    #endregion
  }
}

