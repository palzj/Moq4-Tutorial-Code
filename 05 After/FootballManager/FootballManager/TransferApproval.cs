using System;

namespace FootballManager
{
    public class TransferApproval
    {
        private readonly IPhysicalExamination _physicalExamination;
        private readonly TransferPolicyEvaluator _transferPolicyEvaluator;
        private const int RemainingTotalBudget = 300; // 剩余预算(百万)
        public bool PlayerHealthChecked { get; private set; }

        public TransferApproval(
            IPhysicalExamination physicalExamination,
            TransferPolicyEvaluator transferPolicyEvaluator)
        {
            _physicalExamination = physicalExamination
                ?? throw new ArgumentNullException(nameof(physicalExamination));
            _transferPolicyEvaluator = transferPolicyEvaluator;
            _physicalExamination.HealthChecked += PhysicalExaminationHealthChecked;
        }

        private void PhysicalExaminationHealthChecked(object sender, EventArgs e)
        {
            PlayerHealthChecked = true;
        }

        public TransferResult Evaluate(TransferApplication transfer)
        {
            if (!_transferPolicyEvaluator.IsInTransferPeriod())
            {
                return TransferResult.Postponed;
            }

            if (_physicalExamination.MediacalRoom.Status.IsAvailable == "停用")
            {
                return TransferResult.Postponed;
            }

            bool isHealthy;
            try
            {
                isHealthy = _physicalExamination
                    .IsHealthy(transfer.PlayerAge, transfer.PlayerStrength, transfer.PlayerSpeed);

            }
            catch (Exception)
            {
                return TransferResult.Postponed;
            }

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
