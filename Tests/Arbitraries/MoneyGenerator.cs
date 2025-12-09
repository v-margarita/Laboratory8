using FsCheck;
using FsCheck.Fluent;
using System;
using WalletPropertyTesting.Domain;

namespace WalletPropertyTesting.Tests.Arbitraries
{
    public static class MoneyGenerator
    {
        public static Arbitrary<Money> Money()
        {
            var gen = Gen.Choose(0, 1000000)
                         .Select(x => (decimal)x + (decimal)new Random().NextDouble())
                         .Select(d => Math.Round(d, 2)) 
                         .Select(d => new Domain.Money(d));

            return Arb.From(gen);
        }
    }
}