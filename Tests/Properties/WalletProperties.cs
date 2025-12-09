using FsCheck;
using FsCheck.Xunit;
using System;
using System.Linq;
using WalletPropertyTesting.Domain;
using WalletPropertyTesting.Tests;

namespace WalletPropertyTesting.Tests.Properties
{
    public class WalletProperties
    {
        [Property(Arbitrary = new[] { typeof(WalletArbitraries) })]
        public bool New_Wallet_Has_Zero_Balance()
        {
            var wallet = new Wallet();
            return wallet.Balance.Amount == 0 && wallet.History.Count == 0;
        }

        [Property(Arbitrary = new[] { typeof(WalletArbitraries) })]
        public bool Deposit_Increases_Balance_Correctly(Money initial, Money depositAmount)
        {
            var wallet = new Wallet();
            wallet.Deposit(initial); 

            var balanceBefore = wallet.Balance;
            wallet.Deposit(depositAmount);

            return wallet.Balance.Amount == balanceBefore.Amount + depositAmount.Amount;
        }

        [Property(Arbitrary = new[] { typeof(WalletArbitraries) })]
        public bool Withdraw_Decreases_Balance_Correctly(Money initial, Money withdrawAmount)
        {
            var bigInitial = initial + withdrawAmount;

            var wallet = new Wallet();
            wallet.Deposit(bigInitial);

            var balanceBefore = wallet.Balance;
            wallet.Withdraw(withdrawAmount);

            return wallet.Balance.Amount == balanceBefore.Amount - withdrawAmount.Amount;
        }

        [Property(Arbitrary = new[] { typeof(WalletArbitraries) })]
        public bool History_Tracks_All_Operations(Money amount)
        {
            var wallet = new Wallet();
            wallet.Deposit(amount);
            wallet.Withdraw(amount);

            return wallet.History.Count == 2
                && wallet.History.Any(t => t.Type == TransactionType.Deposit)
                && wallet.History.Any(t => t.Type == TransactionType.Withdraw);
        }
        [Property(Arbitrary = new[] { typeof(WalletArbitraries) })]
        public bool Cannot_Withdraw_More_Than_Balance(Money initial, Money extra)
        {
            if (extra.Amount == 0) return true;

            var wallet = new Wallet();
            wallet.Deposit(initial);

            try
            {
                wallet.Withdraw(initial + extra);
                return false; 
            }
            catch (InvalidOperationException)
            {
                return true; 
            }
        }
    }
}