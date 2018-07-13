using System;

namespace FootballManager
{
    public class PhysicalExamination : IPhysicalExamination
    {
        public bool IsHealthy(int age, int strength, int speed)
        {
            throw new NotImplementedException();
        }

        public void IsHealthy(int age, int strength, int speed, out bool isHealthy)
        {
            throw new NotImplementedException();
        }

        public bool IsMedicalRoomAvailable
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

    }
}
