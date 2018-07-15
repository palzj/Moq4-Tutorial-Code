using System;

namespace FootballManager
{
    public class PhysicalExamination : IPhysicalExamination
    {
        public event EventHandler HealthChecked;

        public bool IsHealthy(int age, int strength, int speed)
        {
            throw new NotImplementedException();
        }

        void IPhysicalExamination.IsHealthy(int age, int strength, int speed, out bool isHealthy)
        {
            throw new NotImplementedException();
        }

        public IMedicalRoom MediacalRoom
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public PhysicalGrade PhysicalGrade
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
    }
}
