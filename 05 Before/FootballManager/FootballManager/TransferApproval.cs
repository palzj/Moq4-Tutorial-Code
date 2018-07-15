using System;

namespace FootballManager
{
    public class TransferApproval
    {
        private const int RemainingTotalBudget = 300; // 剩余预算(百万)

        private readonly IPhysicalExamination _physicalExamination;

        public TransferApproval(IPhysicalExamination physicalExamination)
        {
            _physicalExamination = physicalExamination
                ?? throw new ArgumentNullException(nameof(physicalExamination));
        }

        public TransferResult Evaluate(TransferApplication transfer)
        {
            if (_physicalExamination.MediacalRoom.Status.IsAvailable == "停用")
            {
                return TransferResult.Postponed;
            }

            var isHealthy = _physicalExamination
                .IsHealthy(transfer.PlayerAge, transfer.PlayerStrength, transfer.PlayerSpeed);

            if (!isHealthy)
            {
                _physicalExamination.PhysicalGrade = PhysicalGrade.Failed;
                return TransferResult.Rejected;
            }
            else
            {
                if (transfer.PlayerAge < 25)
                {
                    _physicalExamination.PhysicalGrade = PhysicalGrade.Superb;
                }
                else
                {
                    _physicalExamination.PhysicalGrade = PhysicalGrade.Passed;
                }
            }

            var totalTransferFee = transfer.TransferFee + transfer.ContractYears * transfer.AnnualSalary;
            if (RemainingTotalBudget < totalTransferFee)
            {
                return TransferResult.Rejected;
            }
            if (transfer.PlayerAge < 30)
            {
                return TransferResult.Approved;
            }
            if (transfer.IsSuperStar)
            {
                return TransferResult.ReferredToBoss;
            }
            return TransferResult.Rejected;
        }
    }
}
