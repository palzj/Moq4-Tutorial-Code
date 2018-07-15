using System;

namespace FootballManager
{
    public interface IPhysicalExamination
    {
        bool IsHealthy(int age, int strength, int speed);
        void IsHealthy(int age, int strength, int speed, out bool isHealthy);
        IMedicalRoom MediacalRoom { get; set; }
        PhysicalGrade PhysicalGrade { get; set; }

        event EventHandler HealthChecked;
    }

    public interface IMedicalRoom
    {
        string Name { get; set; }
        IMedicalRoomStatus Status { get; set; }
    }

    public interface IMedicalRoomStatus
    {
        string IsAvailable { get; set; }
    }
}
