using Xunit;

namespace FootballManager.UnitTests
{
    public class TransferApprovalShould
    {
        [Fact]
        public void ApproveYoungCheapPlayerTransfer()
        {
            var approval = new TransferApproval(null);

            var emreTransfer = new TransferApplication
            {
                PlayerName = "Emre Can",
                PlayerAge = 24,
                TransferFee = 0,
                AnnualSalary = 4.52m,
                ContractYears = 4,
                IsSuperStar = false,
                PlayerStrength = 80,
                PlayerSpeed = 75,
            };

            var result = approval.Evaluate(emreTransfer);

            Assert.Equal(TransferResult.Approved, result);
        }

        [Fact]
        public void ReferredToBossWhenTransferringSuperStar()
        {
            var approval = new TransferApproval(null);

            var cr7Transfer = new TransferApplication
            {
                PlayerName = "Cristiano Ronaldo",
                PlayerAge = 33,
                TransferFee = 112m,
                AnnualSalary = 30m,
                ContractYears = 4,
                IsSuperStar = true,
                PlayerStrength = 90,
                PlayerSpeed = 90
            };

            var result = approval.Evaluate(cr7Transfer);

            Assert.Equal(TransferResult.ReferredToBoss, result);
        }
    }
}
