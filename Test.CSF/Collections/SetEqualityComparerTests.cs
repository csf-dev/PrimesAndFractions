using System;
using System.Collections.Generic;
using CSF.Collections;
using Moq;
using NUnit.Framework;

namespace Test.CSF.Collections
{
    [TestFixture,Parallelizable]
    public class SetEqualityComparerTests
    {
        #region Equals
        
        [Test, AutoMoqData]
        public void Equals_returns_true_for_two_collections_which_are_equal(SetEqualityComparer<string> sut)
        {
            var collectionOne = new[] {"one", "two", "three"};
            var collectionTwo = new[] {"one", "two", "three"};

            Assert.That(sut.Equals(collectionOne, collectionTwo), Is.True);
        }

        [Test, AutoMoqData]
        public void Equals_returns_true_for_two_collections_which_are_equal_but_ordered_differently(SetEqualityComparer<string> sut)
        {
            var collectionOne = new[] {"one", "two", "three"};
            var collectionTwo = new[] {"one", "three", "two"};

            Assert.That(sut.Equals(collectionOne, collectionTwo), Is.True);
        }

        [Test, AutoMoqData]
        public void Equals_returns_true_for_two_collections_which_are_equal_but_have_duplicates(SetEqualityComparer<string> sut)
        {
            var collectionOne = new[] {"one", "two", "two", "three", "two"};
            var collectionTwo = new[] {"one", "three", "one", "two"};

            Assert.That(sut.Equals(collectionOne, collectionTwo), Is.True);
        }

        [Test, AutoMoqData]
        public void Equals_returns_true_for_reference_equal_collections(SetEqualityComparer<string> sut)
        {
            var collectionOne = new[] {"one", "two", "three"};

            Assert.That(sut.Equals(collectionOne, collectionOne), Is.True);
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_when_second_collecion_is_null(SetEqualityComparer<string> sut)
        {
            var collectionOne = new[] {"one", "two", "three"};

            Assert.That(sut.Equals(collectionOne, null), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_when_first_collecion_is_null(SetEqualityComparer<string> sut)
        {
            var collectionTwo = new[] {"one", "two", "three"};

            Assert.That(sut.Equals(null, collectionTwo), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_true_when_both_collecions_are_null(SetEqualityComparer<string> sut)
        {
            Assert.That(sut.Equals(null, null), Is.True);
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_for_two_collections_which_have_different_elements(SetEqualityComparer<string> sut)
        {
            var collectionOne = new[] {"one", "two", "three"};
            var collectionTwo = new[] {"one", "two", "THREE"};

            Assert.That(sut.Equals(collectionOne, collectionTwo), Is.False);
        }

        [Test]
        public void Equals_returns_result_respecting_alternative_item_equality_comparer()
        {
            var sut = new SetEqualityComparer<string>(StringComparer.InvariantCultureIgnoreCase);

            var collectionOne = new[] {"one", "two", "three"};
            var collectionTwo = new[] {"one", "two", "THREE"};

            Assert.That(sut.Equals(collectionOne, collectionTwo), Is.True);
        }

        #endregion

        #region GetHashCode
                
        [Test, AutoMoqData]
        public void GetHashCode_throws_ane_for_null_collection(SetEqualityComparer<string> sut)
        {
            Assert.That(() => sut.GetHashCode(null), Throws.ArgumentNullException);
        }

        [Test, AutoMoqData]
        public void GetHashCode_returns_same_value_for_the_same_collection_hashed_twice(SetEqualityComparer<string> sut)
        {
            var collection = new[] {"one", "two", "three"};

            var result1 = sut.GetHashCode(collection);
            var result2 = sut.GetHashCode(collection);

            Assert.That(result1, Is.EqualTo(result2));
        }

        [Test, AutoMoqData]
        public void GetHashCode_does_not_pass_null_items_to_equality_comparer_hash_code_method(IEqualityComparer<string> comparer)
        {
            var sut = new SetEqualityComparer<string>(comparer);
            Mock.Get(comparer)
                .Setup(x => x.GetHashCode(It.IsAny<string>()))
                .Returns(1);
            var collection = new[] {"one", null, "three"};

            sut.GetHashCode(collection);

            Mock.Get(comparer)
                .Verify(x => x.GetHashCode(null), Times.Never);
        }

        [Test, AutoMoqData]
        public void GetHashCode_returns_same_value_for_two_collections_in_different_order(SetEqualityComparer<string> sut)
        {
            var collectionOne = new[] {"one", "two", "three"};
            var collectionTwo = new[] {"one", "three", "two"};

            var result1 = sut.GetHashCode(collectionOne);
            var result2 = sut.GetHashCode(collectionTwo);

            Assert.That(result1, Is.EqualTo(result2));
        }

        [Test, AutoMoqData]
        public void GetHashCode_returns_same_value_for_two_collections_with_same_elements_but_duplicates(SetEqualityComparer<string> sut)
        {
            var collectionOne = new[] {"one", "two", "two", "three", "two"};
            var collectionTwo = new[] {"one", "three", "one", "two"};

            var result1 = sut.GetHashCode(collectionOne);
            var result2 = sut.GetHashCode(collectionTwo);

            Assert.That(result1, Is.EqualTo(result2));
        }

        [Test, AutoMoqData]
        public void GetHashCode_returns_different_value_for_two_collections_with_different_elements(SetEqualityComparer<string> sut)
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
            var sut = new SetEqualityComparer<string>(StringComparer.InvariantCultureIgnoreCase);

            var collectionOne = new[] {"one", "two", "three"};
            var collectionTwo = new[] {"one", "two", "THREE"};

            var result1 = sut.GetHashCode(collectionOne);
            var result2 = sut.GetHashCode(collectionTwo);

            Assert.That(result1, Is.EqualTo(result2));
        }

        #endregion
    }
}