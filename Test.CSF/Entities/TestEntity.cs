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
    #region general tests
    
    [Test]
    public void TestHasIdentity()
    {
      Entity<Person,uint> entity = new Entity<Person,uint>();
      
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
      Entity<Person,uint> entity = new Entity<Person,uint>();
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

    #endregion

    #region testing event-bound reference lists

    [Test]
    [Description("Tests using the 'Add' method to add items to an existing list.")]
    public void TestGetOneToManyReferenceListAdd()
    {
      Person person = new Person() { Id = 1 };

      Order
        orderOne = new Order(),
        orderTwo = new Order(),
        orderThree = new Order();

      person.Orders.Add(orderOne);
      person.Orders.Add(orderTwo);

      Assert.AreEqual(2, person.Orders.Count, "Order count");
      Assert.AreEqual(2, person.SourceList.Count, "Order count (source list)");
      Assert.AreSame(person, orderOne.Owner, "Owner (order 1)");
      Assert.AreSame(person, orderTwo.Owner, "Owner (order 2)");
      Assert.IsNull(orderThree.Owner, "Owner (order 3)");
    }

    [Test]
    [Description("Tests using the 'Remove' method to remove items from an existing list.")]
    public void TestGetOneToManyReferenceListRemove()
    {
      Person person = new Person() { Id = 1 };

      Order
        orderOne = new Order(),
        orderTwo = new Order();

      person.Orders = new List<Order>(new Order[] { orderOne, orderTwo });

      Assert.IsTrue(person.Orders.Remove(orderTwo), "Return value of 'Remove' method");
      Assert.IsNull(orderTwo.Owner, "Owner after removal (order 2)");
      Assert.AreEqual(1, person.Orders.Count, "Order count");
      Assert.AreEqual(1, person.SourceList.Count, "Order count (source list)");
    }

    [Test]
    [Description("Tests replacing a list with a new one.")]
    public void TestGetOneToManyReferenceListReplaceList()
    {
      Person person = new Person() { Id = 1 };

      Order
        orderOne = new Order(),
        orderTwo = new Order(),
        orderThree = new Order();

      person.Orders = new Order[] { orderOne, orderTwo };
      person.Orders = new Order[] { orderThree };

      Assert.IsNull(orderOne.Owner, "Owner after replacement (order 1)");
      Assert.IsNull(orderTwo.Owner, "Owner after replacement (order 2)");
      Assert.AreSame(person, orderThree.Owner, "Owner after replacement (order 3)");
      Assert.AreEqual(1, person.Orders.Count, "Order count");
      Assert.AreEqual(1, person.SourceList.Count, "Order count (source list)");
    }

    [Test]
    [Description("Tests replacing a list with an empty one.")]
    public void TestGetOneToManyReferenceListReplaceWithEmptyList()
    {
      Person person = new Person() { Id = 1 };

      Order
        orderOne = new Order(),
        orderTwo = new Order();

      person.Orders = new Order[] { orderOne, orderTwo };
      person.Orders = new List<Order>();

      Assert.IsNull(orderOne.Owner, "Owner after replacement (order 1)");
      Assert.IsNull(orderTwo.Owner, "Owner after replacement (order 2)");
      Assert.AreEqual(0, person.Orders.Count, "Order count");
      Assert.AreEqual(0, person.SourceList.Count, "Order count (source list)");
    }

    [Test]
    [Description("Tests replacing a list with an empty one and then immediately trying to remove an item.")]
    public void TestGetOneToManyReferenceListReplaceWithEmptyListThenRemove()
    {
      Person person = new Person() { Id = 1 };

      Order
        orderOne = new Order(),
        orderTwo = new Order();

      person.Orders = new Order[] { orderOne, orderTwo };
      person.Orders = new List<Order>();

      Assert.IsFalse(person.Orders.Remove(orderOne), "Return value of 'Remove' method call.");
    }

    [Test]
    [Description("Tests replacing a list with an empty one and then immediately trying to remove an item.")]
    public void TestGetOneToManyReferenceListReplaceWithEmptyListThenAddToDifferentCollection()
    {
      Person
        person = new Person() { Id = 1 },
        personTwo = new Person() { Id = 2 };

      Order
        orderOne = new Order(),
        orderTwo = new Order();

      person.Orders = new Order[] { orderOne, orderTwo };
      person.Orders = new List<Order>();

      personTwo.Orders.Add(orderOne);
      Assert.AreSame(personTwo, orderOne.Owner, "Owner (order 1)");
    }

    [Test]
    public void TestGetOneToManyReferenceListCheckingSourceList()
    {
      Person person = new Person() { Id = 1 };
      Order order = new Order() { Id = 2 };

      person.Orders = new Order[] { order };

      Assert.AreEqual(1, person.SourceList.Count, "Count of orders in source list.");
      Assert.IsNotNull(order.Owner, "Order owner nullability");
      Assert.AreSame(person, person.SourceList[0].Owner, "Order owner in source list item");
    }

    [Test]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestGetOneToManyReferenceListReplaceWithNull()
    {
      Person person = new Person() { Id = 1 };
      person.Orders = null;
    }

    [Test]
    [Description("This test highlights a problem with the API that makes it easy to introduce a bug if the method " +
                 "is not used correctly.")]
    public void TestGetOneToManyReferenceListReplaceListBadAPI()
    {
      Person person = new Person() { Id = 1 };

      Order
        orderOne = new Order(),
        orderTwo = new Order(),
        orderThree = new Order();

      person.WrongOrders = new Order[] { orderOne, orderTwo };
      person.WrongOrders = new Order[] { orderThree };

      Assert.IsNull(orderOne.Owner, "Owner after replacement (order 1)");
      Assert.IsNull(orderTwo.Owner, "Owner after replacement (order 2)");
      Assert.AreSame(person, orderThree.Owner, "Owner after replacement (order 3)");
      Assert.AreEqual(1, person.WrongOrders.Count, "Order count");
      Assert.AreEqual(1, person.SourceList.Count, "Order count (source list)");
    }

    [Test]
    [Description("This test highlights a problem with the API that makes it easy to introduce a bug if the method " +
                 "is not used correctly.")]
    public void TestGetOneToManyReferenceListReplaceWithEmptyListBadAPI()
    {
      Person person = new Person() { Id = 1 };

      Order
        orderOne = new Order(),
        orderTwo = new Order();

      person.WrongOrders = new Order[] { orderOne, orderTwo };
      person.WrongOrders = new List<Order>();

      Assert.IsNull(orderOne.Owner, "Owner after replacement (order 1)");
      Assert.IsNull(orderTwo.Owner, "Owner after replacement (order 2)");
      Assert.AreEqual(0, person.WrongOrders.Count, "Order count");
      Assert.AreEqual(0, person.SourceList.Count, "Order count (source list)");
    }

    #endregion
    
    #region contained mocks
    
    public class Person : Entity<Person,uint>
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

      public virtual IList<Order> WrongOrders
      {
        get {
          return this.GetOneToManyReferenceList(ref _wrappedOrders, ref _orders, x => x.Owner);
        }
        set {
          this.ReplaceOneToManyReferenceList(_wrappedOrders, value, x => x.Owner);
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
    
    public class Order : Entity<Order,uint>
    {
      public virtual Person Owner
      {
        get;
        set;
      }
    }
    
    public class Product : Entity<Product,uint>
    {}
    
    #endregion
  }
}

