using FsCheck;
using FsCheck.Xunit;
using System;
using System.Collections.Generic;
using WalletPropertyTesting.Domain;
using WalletPropertyTesting.Tests;

namespace WalletPropertyTesting.Tests.Properties
{
    public class RepositoryProperties
    {
        [Property(Arbitrary = new[] { typeof(WalletArbitraries) })]
        public bool Saved_Wallet_Can_Be_Retrieved(Money initialBalance)
        {
            var repo = new InMemoryWalletRepository();
            var wallet = new Wallet();
            wallet.Deposit(initialBalance);

            repo.Save(wallet);
            var retrieved = repo.Get(wallet.Id);

            return retrieved.Id == wallet.Id
                && retrieved.Balance.Amount == initialBalance.Amount;
        }

        [Property(Arbitrary = new[] { typeof(WalletArbitraries) })]
        public bool Getting_NonExistent_Wallet_Throws_Exception(Guid randomId)
        {
            var repo = new InMemoryWalletRepository();
            try
            {
                repo.Get(randomId);
                return false;
            }
            catch (KeyNotFoundException)
            {
                return true;
            }
        }
    }
}