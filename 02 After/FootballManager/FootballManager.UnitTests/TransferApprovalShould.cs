using Moq;
using Xunit;

namespace FootballManager.UnitTests
{
    public class TransferApprovalShould
    {
        [Fact]
        public void ApproveYoungCheapPlayerTransfer()
        {
            Mock<IPhysicalExamination> mockExamination = new Mock<IPhysicalExamination>();

            mockExamination.Setup(x => 
                x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            var approval = new TransferApproval(mockExamination.Object);

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
        public void ReferredToBossWhenTransferringSuperStart()
        {
            Mock<IPhysicalExamination> mockExamination = new Mock<IPhysicalExamination>();

            mockExamination.Setup(x => x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            var approval = new TransferApproval(mockExamination.Object);

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
