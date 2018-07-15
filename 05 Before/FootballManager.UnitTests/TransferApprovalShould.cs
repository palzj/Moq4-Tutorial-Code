using Moq;
using Xunit;

namespace FootballManager.UnitTests
{
    public class TransferApprovalShould
    {
        [Fact]
        public void ApproveYoungCheapPlayerTransfer()
        {
            //var medicalRoomStatus = new Mock<IMedicalRoomStatus>();
            //medicalRoomStatus.Setup(x => x.IsAvailable).Returns(true);

            //var medicalRoom = new Mock<IMedicalRoom>();
            //medicalRoom.Setup(x => x.Status).Returns(medicalRoomStatus.Object);

            //var mockExamination = new Mock<IPhysicalExamination>();
            //mockExamination.Setup(x => x.MediacalRoom).Returns(medicalRoom.Object);

            var mockExamination = new Mock<IPhysicalExamination>();
            mockExamination.Setup(x => x.MediacalRoom.Status.IsAvailable).Returns("Available");

            mockExamination.Setup(x =>
                x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(true);
            //mockExamination.Setup(x => 
            //    x.IsHealthy(
            //        It.Is<int>(age => age < 30), 
            //        It.IsIn<int>(80, 85, 90), 
            //        It.IsInRange<int>(75, 99, Range.Inclusive)))
            //    .Returns(true);

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
        public void ReferredToBossWhenTransferringSuperStar()
        {
            var mockExamination = new Mock<IPhysicalExamination>(MockBehavior.Strict);

            mockExamination.Setup(x => x.MediacalRoom.Status.IsAvailable).Returns("Available");
            mockExamination.SetupProperty(x => x.PhysicalGrade, PhysicalGrade.Passed);
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

        [Fact]
        public void RejectedWhenPlayerIsNotHealthy()
        {
            var mockExamination = new Mock<IPhysicalExamination>
            {
                DefaultValue = DefaultValue.Mock
            };

            mockExamination.Setup(x => x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(false);

            var approval = new TransferApproval(mockExamination.Object);

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
            var mockExamination = new Mock<IPhysicalExamination>
            {
                DefaultValue = DefaultValue.Mock
            };

            mockExamination.Setup(x => x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            var approval = new TransferApproval(mockExamination.Object);

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
            var mockExamination = new Mock<IPhysicalExamination>
            {
                DefaultValue = DefaultValue.Mock
            };

            //bool isHealthy = true;
            //mockExamination.Setup(x => x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), out isHealthy));
            mockExamination.Setup(x => x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            var approval = new TransferApproval(mockExamination.Object);

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
            var mockExamination = new Mock<IPhysicalExamination>
            {
                DefaultValue = DefaultValue.Mock
            };

            mockExamination.SetupAllProperties();

            // 开始追踪PhysicalGrade属性
            mockExamination.SetupProperty(x => x.PhysicalGrade, PhysicalGrade.Failed);

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

            Assert.Equal(PhysicalGrade.Passed, mockExamination.Object.PhysicalGrade);
        }

        [Fact]
        public void ShouldPhysicalExamineWhenTransferringSuperStar()
        {
            var mockExamination = new Mock<IPhysicalExamination>();

            mockExamination.Setup(x => x.MediacalRoom.Status.IsAvailable).Returns("Available");
            mockExamination.SetupProperty(x => x.PhysicalGrade, PhysicalGrade.Passed);
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

            //mockExamination.Verify(x => x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()));
            //mockExamination.Verify(x => x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            //mockExamination.Verify(x => x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(1);
            mockExamination.Verify(x => x.IsHealthy(33, 95, 88), Times.Never);
        }

        [Fact]
        public void ShouldCheckMedicalRoomIsAvailableWhenTransferringSuperStar()
        {
            var mockExamination = new Mock<IPhysicalExamination>();

            mockExamination.Setup(x => x.MediacalRoom.Status.IsAvailable).Returns("Available");
            mockExamination.SetupProperty(x => x.PhysicalGrade, PhysicalGrade.Passed);
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

            mockExamination.VerifyGet(x => x.MediacalRoom.Status.IsAvailable);
        }
        
        [Fact]
        public void ShouldSetPhysicalGradeWhenTransferringSuperStar()
        {
            var mockExamination = new Mock<IPhysicalExamination>();

            mockExamination.Setup(x => x.MediacalRoom.Status.IsAvailable).Returns("Available");
            mockExamination.SetupProperty(x => x.PhysicalGrade, PhysicalGrade.Passed);
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

            mockExamination.VerifySet(x => x.PhysicalGrade = PhysicalGrade.Passed);
        }
    }
}
