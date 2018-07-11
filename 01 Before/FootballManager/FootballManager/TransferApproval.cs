namespace FootballManager
{
    public class TransferApproval
    {
        private const int RemainingTotalBudget = 300; // 剩余预算(百万)

        public TransferResult Evaluate(TransferApplication transfer)
        {
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
