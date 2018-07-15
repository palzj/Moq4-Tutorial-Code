using Moq;
using Moq.Protected;
using System;
using Xunit;

namespace FootballManager.UnitTests
{
    public class TransferApprovalShould
    {
        private Mock<IPhysicalExamination> mockExamination = new Mock<IPhysicalExamination>();
        private Mock<TransferPolicyEvaluator> mockTransferPolicy = new Mock<TransferPolicyEvaluator>();
        private TransferApproval approval;

        public TransferApprovalShould()
        {
            mockExamination.SetupAllProperties();
            mockExamination.SetupProperty(x => x.PhysicalGrade, PhysicalGrade.Passed);
            mockExamination.Setup(x => x.MediacalRoom.Status.IsAvailable).Returns("Available");
            mockExamination.Setup(x => x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(true);

            mockTransferPolicy.Setup(x => x.IsInTransferPeriod()).Returns(true);

            approval = new TransferApproval(mockExamination.Object, mockTransferPolicy.Object);
        }

        [Fact]
        public void ApproveYoungCheapPlayerTransfer()
        {
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

        [Fact]
        public void RejectedWhenPlayerIsNotHealthy()
        {
            mockExamination.Setup(x => x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(false);

            var andreaContiTransfer = new TransferApplication
            {
                PlayerName = "Andrea Conti",
                PlayerAge = 22,
                TransferFee = 25m,
                AnnualSalary = 2.5m,
                ContractYears = 4,
                IsSuperStar = false,
                PlayerStrength = 75,
                PlayerSpeed = 91
            };

            var result = approval.Evaluate(andreaContiTransfer);

            Assert.Equal(TransferResult.Rejected, result);
        }

        [Fact]
        public void RejectedWhenExcessingBudget()
        {
            mockExamination.DefaultValue = DefaultValue.Mock;
            mockExamination.Setup(x => x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            var mbappeTransfer = new TransferApplication
            {
                PlayerName = "Kylian Mbappé",
                PlayerAge = 19,
                TransferFee = 500m,
                AnnualSalary = 10m,
                ContractYears = 5,
                IsSuperStar = true,
                PlayerStrength = 85,
                PlayerSpeed = 92
            };

            var result = approval.Evaluate(mbappeTransfer);

            Assert.Equal(TransferResult.Rejected, result);
        }

        [Fact]
        public void RejectedWhenNonSuperstarOldPlayer()
        {
            mockExamination.DefaultValue = DefaultValue.Mock;

            var carlosBaccaTransfer = new TransferApplication
            {
                PlayerName = "Carlos Bacca",
                PlayerAge = 32,
                TransferFee = 15m,
                AnnualSalary = 3.5m,
                ContractYears = 4,
                IsSuperStar = false,
                PlayerStrength = 80,
                PlayerSpeed = 70
            };

            var result = approval.Evaluate(carlosBaccaTransfer);

            Assert.Equal(TransferResult.Rejected, result);
        }

        [Fact]
        public void PhysicalGradeShouldPassWhenTransferringSuperStar()
        {
            mockExamination.DefaultValue = DefaultValue.Mock;
            // 开始追踪PhysicalGrade属性
            mockExamination.SetupProperty(x => x.PhysicalGrade, PhysicalGrade.Failed);

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

            Assert.Equal(PhysicalGrade.Passed, mockExamination.Object.PhysicalGrade);
        }

        [Fact]
        public void ShouldPhysicalExamineWhenTransferringSuperStar()
        {
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

            //mockExamination.Verify(x => x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()));
            //mockExamination.Verify(x => x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            //mockExamination.Verify(x => x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(1);
            mockExamination.Verify(x => x.IsHealthy(33, 95, 88), Times.Never);
        }

        [Fact]
        public void ShouldCheckMedicalRoomIsAvailableWhenTransferringSuperStar()
        {
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

            mockExamination.VerifyGet(x => x.MediacalRoom.Status.IsAvailable);
        }

        [Fact]
        public void ShouldSetPhysicalGradeWhenTransferringSuperStar()
        {
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

            mockExamination.VerifySet(x => x.PhysicalGrade = PhysicalGrade.Passed);
        }

        [Fact]
        public void PostponedWhenTransferringChildPlayer()
        {
            //mockExamination.Setup(x => x.IsHealthy(It.Is<int>(age => age < 16), It.IsAny<int>(), It.IsAny<int>()))
            //    .Throws<Exception>();
            mockExamination.Setup(x => x.IsHealthy(It.Is<int>(age => age < 16), It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception("The player is still a child"));

            var childTransfer = new TransferApplication
            {
                PlayerName = "Some Child Player",
                PlayerAge = 13,
                TransferFee = 0,
                AnnualSalary = 0.01m,
                ContractYears = 3,
                IsSuperStar = false,
                PlayerStrength = 40,
                PlayerSpeed = 50
            };

            var result = approval.Evaluate(childTransfer);

            Assert.Equal(TransferResult.Postponed, result);
        }

        [Fact]
        public void ShouldPlayerHealthCheckedWhenTransferringSuperStar()
        {
            mockExamination.Setup(x => x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(true)
                .Raises(x => x.HealthChecked += null, EventArgs.Empty);

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

            // mockExamination.Raise(x => x.HealthChecked += null, EventArgs.Empty);

            Assert.True(approval.PlayerHealthChecked);
        }

        [Fact]
        public void ShouldSetPhysicalGradeWhenTransferringSuperStar_Sequence()
        {
            mockExamination.SetupSequence(x => x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(true)
                .Returns(false);

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

            var result2 = approval.Evaluate(cr7Transfer);
            Assert.Equal(TransferResult.Rejected, result2);
        }

        [Fact]
        public void ShouldPostponedWhenNotInTransferPeriod()
        {
            mockTransferPolicy.Setup(x => x.IsInTransferPeriod()).Returns(false);

            //mockTransferPolicy.Protected()
            //    .Setup<bool>("IsBannedFromTransferring")
            //    // .Setup<bool>("IsBannedFromTransferring", ItExpr.IsAny<string>())
            //    .Returns(true);

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
            Assert.Equal(TransferResult.Postponed, result);
        }

        [Fact]
        public void LinqToMocks()
        {
            var mockExamination = Mock.Of<IPhysicalExamination>
            (
                me => me.MediacalRoom.Status.IsAvailable == "Available" &&
                      me.PhysicalGrade == PhysicalGrade.Passed &&
                      me.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()) == true
            );

            var mockTransferPolicy = Mock.Of<TransferPolicyEvaluator>
            (
                mt => mt.IsInTransferPeriod() == false
            );

            approval = new TransferApproval(mockExamination, mockTransferPolicy);

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
            Assert.Equal(TransferResult.Postponed, result);
        }
    }
}
