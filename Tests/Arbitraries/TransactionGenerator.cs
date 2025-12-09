using FsCheck;
using FsCheck.Fluent;
using System;
using WalletPropertyTesting.Domain;

namespace WalletPropertyTesting.Tests.Arbitraries
{
    public static class TransactionGenerator
    {
        public static Arbitrary<Transaction> Transaction()
        {
            var moneyGen = MoneyGenerator.Money().Generator;

            var depositGen = moneyGen.Select(Domain.Transaction.Deposit);
            var withdrawGen = moneyGen.Select(Domain.Transaction.Withdraw);
            var gen = Gen.OneOf(depositGen, withdrawGen);

            return Arb.From(gen);
        }
    }
}