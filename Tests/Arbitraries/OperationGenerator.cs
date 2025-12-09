using FsCheck;
using FsCheck.Fluent;
using System;

namespace WalletPropertyTesting.Tests.Arbitraries
{
    public enum WalletOpType { Deposit, Withdraw }
    public record WalletOperation(WalletOpType Type, decimal Amount);

    public static class OperationGenerator
    {
        public static Arbitrary<WalletOperation> Operation()
        {
            var amountGen = Gen.Choose(1, 1000) 
                               .Select(x => (decimal)x);

            var typeGen = Gen.Elements(WalletOpType.Deposit, WalletOpType.Withdraw);

            var gen = from amount in amountGen
                      from type in typeGen
                      select new WalletOperation(type, amount);

            return Arb.From(gen);
        }
    }
}