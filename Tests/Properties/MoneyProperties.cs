using FsCheck;
using FsCheck.Xunit;
using System;
using WalletPropertyTesting.Domain;
using WalletPropertyTesting.Tests;

namespace WalletPropertyTesting.Tests.Properties
{
    public class MoneyProperties
    {
        [Property(Arbitrary = new[] { typeof(WalletArbitraries) })]
        public bool Addition_Is_Commutative(Money a, Money b)
        {
            return (a + b).Amount == (b + a).Amount;
        }

        [Property(Arbitrary = new[] { typeof(WalletArbitraries) })]
        public bool Addition_Is_Associative(Money a, Money b, Money c)
        {
            return ((a + b) + c).Amount == (a + (b + c)).Amount;
        }

        [Property(Arbitrary = new[] { typeof(WalletArbitraries) })]
        public bool Subtraction_Is_Inverse_Of_Addition(Money a, Money b)
        {
            return ((a + b) - b).Amount == a.Amount;
        }

        [Property]
        public bool Negative_Amount_Throws_Exception(decimal amount)
        {
            if (amount < 0)
            {
                try
                {
                    new Money(amount);
                    return false; 
                }
                catch (ArgumentException)
                {
                    return true; 
                }
            }
            return true; 
        }
    }
}