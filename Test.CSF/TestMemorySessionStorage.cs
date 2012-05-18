//  
//  TestMemorySessionStorage.cs
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
using NUnit.Framework;
using CSF;

namespace Test.CSF
{
  [TestFixture]
  [Category("Requires configuration")]
  [Description("The tests in this fixture depend upon a working session storage configuration.")]
  public class TestMemorySessionStorage
  {
    #region fields
    
    private ISessionStorage storage;
    
    #endregion
    
    #region setup and teardown
    
    [TestFixtureSetUp]
    public void FixtureSetUp()
    {
      storage = new MemorySessionStorage();
    }
    
    [SetUp]
    public void Setup()
    {
      storage.Abandon();
    }
    
    #endregion
    
    #region tests
    
    [Test]
    public void TestStoreValue()
    {
      storage.Store<DateTime>("test", DateTime.Today);
      Assert.AreEqual(DateTime.Today, storage.Get<DateTime>("test"), "Test value is as expected");
    }
    
    [Test]
    public void TestClearValue()
    {
      DateTime discarded;
      storage.Store<DateTime>("test", DateTime.Today);
      Assert.AreEqual(DateTime.Today, storage.Get<DateTime>("test"), "Test value is as expected");
      
      storage.Remove("test");
      Assert.IsFalse(storage.TryGet<DateTime>("test", out discarded), "Data has been removed");
    }
    
    [Test]
    public void TestAbandon()
    {
      DateTime discarded;
      storage.Store<DateTime>("test", DateTime.Today);
      Assert.AreEqual(DateTime.Today, storage.Get<DateTime>("test"), "Test value is as expected");
      
      storage.Abandon();
      Assert.IsFalse(storage.TryGet<DateTime>("test", out discarded), "Data has been removed");
    }
    
    #endregion
  }
}

