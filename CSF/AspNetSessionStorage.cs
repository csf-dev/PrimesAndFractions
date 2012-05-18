//  
//  AspNetSessionStorage.cs
//  
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
// 
//  Copyright (c) 2012 CSF Software Limited
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
using System.Threading;
using System.Web;
using System.Web.SessionState;

namespace CSF
{
  /// <summary>
  /// <para>Implementation of <see cref="ISessionStorage"/> using the ASP.NET session state as the backend.</para>
  /// </summary>
  public class AspNetSessionStorage : ISessionStorage
  {
    #region properties
    
    /// <summary>
    /// <para>A <see cref="ReaderWriterLockSlim"/> for controlling synchronised access to this instance.</para>
    /// </summary>
    protected ReaderWriterLockSlim SyncRoot = new ReaderWriterLockSlim();
    
    #endregion
    
    #region SessionStorage implementation
    
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
    /// <exception cref="InvalidOperationException">
    /// If there is no current <c>HttpContext</c>.
    /// </exception>
    public virtual TValue Get<TValue>(string key)
    {
      TValue output;
      
      if(key == null)
      {
        throw new ArgumentNullException("key");
      }
      
      try
      {
        this.SyncRoot.EnterReadLock();
        HttpSessionState state = this.GetStorageBackend();
        object tempOutput = state[key];
        if(tempOutput == null)
        {
          throw new ArgumentException(String.Format("There is no value stored at the key '{0}'.", key));
        }
        output = (TValue) tempOutput;
      }
      finally
      {
        if(this.SyncRoot.IsReadLockHeld)
        {
          this.SyncRoot.ExitReadLock();
        }
      }
      
      return output;
    }
    
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
    /// <exception cref="InvalidOperationException">
    /// If there is no current <c>HttpContext</c>.
    /// </exception>
    public virtual bool TryGet<TValue>(string key, out TValue output)
    {
      bool result = false;
      output = default(TValue);
      
      try
      {
        output = this.Get<TValue>(key);
        result = true;
      }
      // Just drop these on the floor, we will be returning false if they occur
      catch(ArgumentException) {}
      catch(InvalidCastException) {}
      
      return result;
    }
    
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
    /// <exception cref="InvalidOperationException">
    /// If there is no current <c>HttpContext</c>.
    /// </exception>
    public virtual void Store<TValue>(string key, TValue value)
    {
      if(key == null)
      {
        throw new ArgumentNullException("key");
      }
      
      try
      {
        this.SyncRoot.EnterWriteLock();
        HttpSessionState backend = this.GetStorageBackend();
        backend[key] = value;
      }
      finally
      {
        if(this.SyncRoot.IsWriteLockHeld)
        {
          this.SyncRoot.ExitWriteLock();
        }
      }
    }
    
    /// <summary>
    /// Removes any values that may have been present at the specified key.
    /// </summary>
    /// <param name='key'>
    /// The identifier of the value to remove.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="key"/> is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// If there is no current <c>HttpContext</c>.
    /// </exception>
    public virtual void Remove(string key)
    {
      if(key == null)
      {
        throw new ArgumentNullException("key");
      }
      
      try
      {
        this.SyncRoot.EnterWriteLock();
        HttpSessionState backend = this.GetStorageBackend();
        backend.Remove(key);
      }
      finally
      {
        if(this.SyncRoot.IsWriteLockHeld)
        {
          this.SyncRoot.ExitWriteLock();
        }
      }
    }
    
    /// <summary>
    /// Abandons this instance and clears all contained values.  The state of the current instance after calling this
    /// method is undefined.
    /// </summary>
    public virtual void Abandon()
    {
      try
      {
        this.SyncRoot.EnterWriteLock();
        HttpSessionState backend = this.GetStorageBackend();
        backend.Abandon();
      }
      finally
      {
        if(this.SyncRoot.IsWriteLockHeld)
        {
          this.SyncRoot.ExitWriteLock();
        }
      }
    }
    
    #endregion
    
    #region access to the session storage
    
    /// <summary>
    /// <para>Gets the ASP.NET session storage backend.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="HttpSessionState"/>
    /// </returns>
    protected HttpSessionState GetStorageBackend()
    {
      if(HttpContext.Current == null)
      {
        throw new InvalidOperationException("There is no current HttpContext.");
      }
      else if(HttpContext.Current.Session == null)
      {
        throw new InvalidOperationException("The current HttpContext does not expose a session state.");
      }
      
      return HttpContext.Current.Session;
    }
    
    #endregion
  }
}

