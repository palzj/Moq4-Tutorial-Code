namespace FootballManager
{
    public class TransferApplication
    {
        public int Id { get; set; }
        public string PlayerName { get; set; } // 球员名字
        public int PlayerAge { get; set; } // 年龄
        public decimal TransferFee { get; set; }  // 转会费(百万)
        public decimal AnnualSalary { get; set; }  // 年薪(百万)
        public int ContractYears { get; set; } // 合同年限
        public bool IsSuperStar { get; set; } // 是否是超级巨星

        public int PlayerStrength { get; set; } // 球员的力量
        public int PlayerSpeed { get; set; } // 球员的速度
    }
}
