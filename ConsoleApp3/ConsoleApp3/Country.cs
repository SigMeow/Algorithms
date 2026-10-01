namespace TourAgencyApp
{
    public class Country
    {
        public int Id { get; }
        public string Name { get; private set; }
        public string Continent { get; private set; }

        public Country(int id, string name, string continent)
        {
            Id = id;
            Name = name;
            Continent = continent;
        }

        public bool IsEurope()
        {
            return Continent == "Европа";
        }

        public string GetInfo()
        {
            return $"{Name} ({Continent})";
        }
    }
}