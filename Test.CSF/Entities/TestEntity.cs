//  
//  TestEntity.cs
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
using CSF.Entities;
using NUnit.Framework;
using System.Collections.Generic;

namespace Test.CSF.Entities
{
  [TestFixture]
  public class TestEntity
  {
    #region tests
    
    [Test]
    public void TestHasIdentity()
    {
      Entity<uint> entity = new Entity<uint>();
      
      Assert.IsFalse(entity.HasIdentity, "Entity has no identity");
      
      entity.SetIdentity(5);
      Assert.IsTrue(entity.HasIdentity, "Entity has an identity");
    }
    
    [Test]
    public void TestGetIdentity()
    {
      Person entity = new Person() { Id = 5 };
      IIdentity identity = entity.GetIdentity();
      Assert.AreEqual(String.Format("[{0}: {1}]", typeof(Person).FullName, 5),
                      identity.ToString(),
                      "Correct identity");
    }
    
    [Test]
    public void TestSetIdentity()
    {
      Person entity = new Person();
      IIdentity identity;
      
      entity.SetIdentity(5);
      identity = entity.GetIdentity();
      Assert.AreEqual(String.Format("[{0}: {1}]", typeof(Person).FullName, 5),
                      identity.ToString(),
                      "Correct identity");
    }
    
    [Test]
    [ExpectedException(ExceptionType = typeof(ArgumentException), ExpectedMessage = "Invalid identity value")]
    public void TestSetIdentityInvalid()
    {
      Entity<uint> entity = new Entity<uint>();
      entity.SetIdentity(0);
      Assert.Fail("Test should never reach this point");
    }
    
    [Test]
    public void TestClearIdentity()
    {
      Person entity = new Person() { Id = 5 };
      
      Assert.IsTrue(entity.HasIdentity, "Has identity");
      entity.ClearIdentity();
      Assert.IsFalse(entity.HasIdentity, "Identity removed");
    }
    
    [Test]
    public void TestValidateIdentity()
    {
      Person entity = new Person();
      Assert.IsTrue(entity.ValidateIdentity(5), "Valid identity");
      Assert.IsFalse(entity.ValidateIdentity(0), "Invalid identity");
    }
    
    [Test]
    public void TestEquals()
    {
      string stringTest = "foo bar";
      uint numericTest = 3; 
      Person three = new Person() { Id = 3 };
      Person four = new Person() { Id = 4 };
      Person threeAgain = new Person() { Id = 3 };
      Product threeProduct = new Product() { Id = 3 };
      
      Assert.IsFalse(three.Equals(stringTest), "Entity does not equal a string");
      Assert.IsFalse(three.Equals(numericTest), "Entity does not equal a uint");
      Assert.IsFalse(three.Equals(four), "Non-matching identities not equal");
      
      Assert.IsTrue(three.Equals(three), "Copies of the same object are equal");
      Assert.IsTrue(three.Equals(threeAgain), "Identical identities are equal");
      
      Assert.IsFalse(three.Equals(threeProduct), "Non-matching types not equal");
    }
    
    [Test]
    public void TestToString()
    {
      Person entity = new Person() { Id = 5 };
      
      Assert.AreEqual(String.Format("[{0}: {1}]", typeof(Person).FullName, 5),
                      entity.ToString(),
                      "Correct identity");
      
      entity.ClearIdentity();
      Assert.AreEqual(String.Format("[{0}: {1}]", typeof(Person).FullName, "no identity"),
                      entity.ToString(),
                      "Correct identity");
    }
    
    [Test]
    public void TestOperatorEquality()
    {
      Person three = new Person() { Id = 3 };
      Person four = new Person() { Id = 4 };
      Person threeAgain = new Person() { Id = 3 };
      Product threeProduct = new Product() { Id = 3 };
      
      Assert.IsFalse(three == four, "Non-matching identities not equal");
#pragma warning disable 1718
      // Disabling CS1718 - the point of this test is to compare the object to itself
      Assert.IsTrue(three == three, "Copies of the same object are equal");
#pragma warning restore 1718
      Assert.IsTrue(three == threeAgain, "Identical instances are equal");
      Assert.IsFalse(three == threeProduct, "Non-matching types not equal");
    }
    
    [Test]
    public void TestOperatorInequality()
    {
      Person three = new Person() { Id = 3 };
      Person four = new Person() { Id = 4 };
      Person threeAgain = new Person() { Id = 3 };
      Product threeProduct = new Product() { Id = 3 };
      
      Assert.IsTrue(three != four, "Non-matching identities not equal");
#pragma warning disable 1718
      // Disabling CS1718 - the point of this test is to compare the object to itself
      Assert.IsFalse(three != three, "Copies of the same object are equal");
#pragma warning restore 1718
      Assert.IsFalse(three != threeAgain, "Identical instances are equal");
      Assert.IsTrue(three != threeProduct, "Non-matching types not equal");
    }

    [Test]
    public void TestGetReciprocalReferenceList()
    {
      Person
        person = new Person() { Id = 1 },
        personTwo = new Person() { Id = 2 };

      Order
        orderOne = new Order(),
        orderTwo = new Order(),
        orderThree = new Order();

      person.Orders.Add(orderOne);
      person.Orders.Add(orderTwo);

      Assert.AreEqual(2, person.Orders.Count, "Correct order count");
      Assert.AreEqual(2, person.SourceList.Count, "Correct order count (source)");
      Assert.AreEqual(person, orderOne.Owner, "Correct owner (order 1)");
      Assert.AreEqual(person, orderTwo.Owner, "Correct owner (order 2)");
      Assert.IsNull(orderThree.Owner, "Owner is null (order 3)");

      Assert.IsTrue(person.Orders.Remove(orderTwo), "Truth when removing an order that was in the set");
      Assert.IsNull(orderTwo.Owner, "Owner is null after removal (order 2)");
      Assert.AreEqual(1, person.Orders.Count, "Correct order count (after removal)");
      Assert.AreEqual(1, person.SourceList.Count, "Correct order count (source, after removal)");

      person.Orders = new Order[] { orderThree };

      Assert.IsNull(orderOne.Owner, "Owner is null after overwriting list (order 1)");
      Assert.AreEqual(person, orderThree.Owner, "Correct owner after overwriting list (order 3)");
      Assert.AreEqual(1, person.Orders.Count, "Correct order count (after overwriting)");
      Assert.AreEqual(1, person.SourceList.Count, "Correct order count (source, after overwriting)");

      person.Orders = new List<Order>();

      personTwo.Orders.Add(orderOne);
      bool removed = person.Orders.Remove(orderOne);
      Assert.IsFalse(removed, "No item was removed");
      Assert.AreEqual(personTwo, orderOne.Owner, "Owner of order 1 remains intact");
    }

    [Test]
    [ExpectedException(ExceptionType = typeof(ArgumentNullException))]
    public void TestGetReciprocalReferenceListReplaceWithNull()
    {
      Person person = new Person() {
        Id = 1
      };
      person.Orders = null;
    }

    #endregion
    
    #region mocks
    
    public class Person : Entity<uint>
    {
      private IList<Order> _orders, _wrappedOrders;

      public virtual IList<Order> Orders
      {
        get {
          return this.GetOneToManyReferenceList(ref _wrappedOrders, ref _orders, x => x.Owner);
        }
        set {
          _wrappedOrders = this.ReplaceOneToManyReferenceList(_wrappedOrders, value, x => x.Owner);
          _orders = value;
        }
      }

      public virtual IList<Order> SourceList
      {
        get {
          return _orders;
        }
      }
    }
    
    public class Order : Entity<uint>
    {
      public virtual Person Owner
      {
        get;
        set;
      }
    }
    
    public class Product : Entity<uint>
    {}
    
    #endregion
  }
}

