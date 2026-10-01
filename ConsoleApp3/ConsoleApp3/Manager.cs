namespace TourAgencyApp
{
    public class Manager
    {
        public int Id { get; }
        public string FullName { get; private set; }
        public string Phone { get; private set; }
        public int Experience { get; private set; }

        // Конструктор
        public Manager(int id, string fullName, string phone, int experience)
        {
            Id = id;
            FullName = fullName;
            Phone = phone;
            Experience = experience;
        }

        public bool IsExperienced()
        {
            return Experience > 3;
        }

        public string GetInfo()
        {
            return $"{FullName} ({Experience} лет опыта)";
        }
    }
}