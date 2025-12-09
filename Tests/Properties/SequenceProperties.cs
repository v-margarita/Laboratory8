using FsCheck;
using FsCheck.Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using WalletPropertyTesting.Domain;
using WalletPropertyTesting.Tests;
using WalletPropertyTesting.Tests.Arbitraries;

namespace WalletPropertyTesting.Tests.Properties
{
    public class SequenceProperties
    {
        [Property(Arbitrary = new[] { typeof(WalletArbitraries) })]
        public bool Valid_Sequence_Result_Is_Consistent(List<WalletOperation> operations)
        {
            var repo = new InMemoryWalletRepository();
            var service = new WalletService(repo);
            var wallet = service.CreateWallet();

            decimal expectedBalance = 0;

            foreach (var op in operations)
            {
                try
                {
                    if (op.Type == WalletOpType.Deposit)
                    {
                        service.Deposit(wallet.Id, op.Amount);
                        expectedBalance += op.Amount;
                    }
                    else
                    {
                        if (expectedBalance >= op.Amount)
                        {
                            service.Withdraw(wallet.Id, op.Amount);
                            expectedBalance -= op.Amount;
                        }
                        else
                        {
                            try
                            {
                                service.Withdraw(wallet.Id, op.Amount);
                                return false; 
                            }
                            catch (InvalidOperationException) { /* Очікувано */ }
                        }
                    }
                }
                catch (ArgumentException)
                {

                }
            }

            var actualBalance = service.GetBalance(wallet.Id).Amount;
            return actualBalance == expectedBalance;
        }

        [Property(Arbitrary = new[] { typeof(WalletArbitraries) })]
        public bool Multiple_Deposits_Sum_Up_Correctly(List<Money> deposits)
        {
            var wallet = new Wallet();

            foreach (var m in deposits)
            {
                wallet.Deposit(m);
            }

            decimal totalExpected = deposits.Sum(d => d.Amount);
            return wallet.Balance.Amount == totalExpected;
        }
    }
}