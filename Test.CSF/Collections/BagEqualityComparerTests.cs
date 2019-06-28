using System;
using System.Collections.Generic;
using CSF.Collections;
using Moq;
using NUnit.Framework;

namespace Test.CSF.Collections
{
    [TestFixture,Parallelizable]
    public class BagEqualityComparerTests
    {
        #region Equals

        [Test, AutoMoqData]
        public void Equals_returns_true_for_two_collections_which_are_equal(BagEqualityComparer<string> sut)
        {
            var collectionOne = new[] {"one", "two", "three"};
            var collectionTwo = new[] {"one", "two", "three"};

            Assert.That(sut.Equals(collectionOne, collectionTwo), Is.True);
        }

        [Test, AutoMoqData]
        public void Equals_returns_true_for_two_collections_which_are_equal_but_ordered_differently(BagEqualityComparer<string> sut)
        {
            var collectionOne = new[] {"one", "two", "three"};
            var collectionTwo = new[] {"one", "three", "two"};

            Assert.That(sut.Equals(collectionOne, collectionTwo), Is.True);
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_for_two_collections_which_are_equal_but_have_duplicates(BagEqualityComparer<string> sut)
        {
            var collectionOne = new[] {"one", "two", "two", "three", "two"};
            var collectionTwo = new[] {"one", "three", "one", "two"};

            Assert.That(sut.Equals(collectionOne, collectionTwo), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_true_for_reference_equal_collections(BagEqualityComparer<string> sut)
        {
            var collectionOne = new[] {"one", "two", "three"};

            Assert.That(sut.Equals(collectionOne, collectionOne), Is.True);
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_when_second_collecion_is_null(BagEqualityComparer<string> sut)
        {
            var collectionOne = new[] {"one", "two", "three"};

            Assert.That(sut.Equals(collectionOne, null), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_when_first_collecion_is_null(BagEqualityComparer<string> sut)
        {
            var collectionTwo = new[] {"one", "two", "three"};

            Assert.That(sut.Equals(null, collectionTwo), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_true_when_both_collecions_are_null(BagEqualityComparer<string> sut)
        {
            Assert.That(sut.Equals(null, null), Is.True);
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_for_two_collections_which_have_different_elements(BagEqualityComparer<string> sut)
        {
            var collectionOne = new[] {"one", "two", "three"};
            var collectionTwo = new[] {"one", "two", "THREE"};

            Assert.That(sut.Equals(collectionOne, collectionTwo), Is.False);
        }

        [Test]
        public void Equals_returns_result_respecting_alternative_item_equality_comparer()
        {
            var sut = new BagEqualityComparer<string>(StringComparer.InvariantCultureIgnoreCase, StringComparer.InvariantCultureIgnoreCase);

            var collectionOne = new[] {"one", "two", "three"};
            var collectionTwo = new[] {"one", "two", "THREE"};

            Assert.That(sut.Equals(collectionOne, collectionTwo), Is.True);
        }
        
        [Test, AutoMoqData]
        public void Equals_returns_true_comparing_equivalent_object_collections_which_are_equatable_but_not_comparable(BagEqualityComparer<Pet> sut)
        {
            var coll1 = new[] {new Pet {Name = "A"}, new Pet {Name = "B"}, new Pet {Name = "C"},};
            var coll2 = new[] {new Pet {Name = "B"}, new Pet {Name = "C"}, new Pet {Name = "A"},};

            Assert.That(sut.Equals(coll1, coll2), Is.True);
        }
        
        [Test, AutoMoqData]
        public void Equals_returns_false_comparing_different_object_collections_which_are_equatable_but_not_comparable(BagEqualityComparer<Pet> sut)
        {
            var coll1 = new[] {new Pet {Name = "Z"}, new Pet {Name = "B"}, new Pet {Name = "C"},};
            var coll2 = new[] {new Pet {Name = "B"}, new Pet {Name = "C"}, new Pet {Name = "A"},};

            Assert.That(sut.Equals(coll1, coll2), Is.False);
        }

        #endregion

        #region GetHashCode
        
        [Test, AutoMoqData]
        public void GetHashCode_throws_ane_for_null_collection(BagEqualityComparer<string> sut)
        {
            Assert.That(() => sut.GetHashCode(null), Throws.ArgumentNullException);
        }

        [Test, AutoMoqData]
        public void GetHashCode_returns_same_value_for_the_same_collection_hashed_twice(BagEqualityComparer<string> sut)
        {
            var collection = new[] {"one", "two", "three"};

            var result1 = sut.GetHashCode(collection);
            var result2 = sut.GetHashCode(collection);

            Assert.That(result1, Is.EqualTo(result2));
        }
        
        [Test, AutoMoqData]
        public void GetHashCode_does_not_pass_null_items_to_equality_comparer_hash_code_method(IEqualityComparer<string> comparer)
        {
            var sut = new BagEqualityComparer<string>(comparer);
            Mock.Get(comparer)
                .Setup(x => x.GetHashCode(It.IsAny<string>()))
                .Returns(1);
            var collection = new[] {"one", null, "three"};

            sut.GetHashCode(collection);

            Mock.Get(comparer)
                .Verify(x => x.GetHashCode(null), Times.Never);
        }

        [Test, AutoMoqData]
        public void GetHashCode_returns_same_value_for_two_collections_in_different_order(BagEqualityComparer<string> sut)
        {
            var collectionOne = new[] {"one", "two", "three"};
            var collectionTwo = new[] {"one", "three", "two"};

            var result1 = sut.GetHashCode(collectionOne);
            var result2 = sut.GetHashCode(collectionTwo);

            Assert.That(result1, Is.EqualTo(result2));
        }

        [Test, AutoMoqData]
        public void GetHashCode_returns_different_values_for_two_collections_with_same_elements_but_duplicates(BagEqualityComparer<string> sut)
        {
            var collectionOne = new[] {"one", "two", "two", "three", "two"};
            var collectionTwo = new[] {"one", "three", "one", "two"};

            var result1 = sut.GetHashCode(collectionOne);
            var result2 = sut.GetHashCode(collectionTwo);

            Assert.That(result1, Is.Not.EqualTo(result2));
        }

        [Test, AutoMoqData]
        public void GetHashCode_returns_different_value_for_two_collections_with_different_elements(BagEqualityComparer<string> sut)
        {
            var collectionOne = new[] {"one", "two", "three"};
            var collectionTwo = new[] {"one", "two", "THREE"};

            var result1 = sut.GetHashCode(collectionOne);
            var result2 = sut.GetHashCode(collectionTwo);

            Assert.That(result1, Is.Not.EqualTo(result2));
        }

        [Test]
        public void GetHashCode_returns_same_value_for_collections_respecting_alternative_item_equality_comparer()
        {
            var sut = new BagEqualityComparer<string>(StringComparer.InvariantCultureIgnoreCase, StringComparer.InvariantCultureIgnoreCase);

            var collectionOne = new[] {"one", "two", "three"};
            var collectionTwo = new[] {"one", "two", "THREE"};

            var result1 = sut.GetHashCode(collectionOne);
            var result2 = sut.GetHashCode(collectionTwo);

            Assert.That(result1, Is.EqualTo(result2));
        }

        #endregion

        #region contained type

        public class Pet : IEquatable<Pet>
        {
            public string Name { get; set; }

            public bool Equals(Pet other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return string.Equals(Name, other.Name);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Pet) obj);
            }

            public override int GetHashCode()
            {
                return (Name != null ? Name.GetHashCode() : 0);
            }
        }

        #endregion
    }
}