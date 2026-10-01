using System;

namespace TourAgencyApp
{
    public class Tour
    {
        public int Id { get; }
        public string Name { get; private set; }
        public int CountryId { get; private set; }
        public int ManagerId { get; private set; }
        public decimal Price { get; private set; }
        public int Days { get; private set; }

        public Tour(int id, string name, int countryId, int managerId, decimal price, int days)
        {
            if (days <= 0)
                throw new ArgumentException("Количество дней должно быть больше 0.");

            Id = id;
            Name = name;
            CountryId = countryId;
            ManagerId = managerId;
            Price = price;
            Days = days;
        }

        public decimal PricePerDay()
        {
            return Price / Days;
        }

        public bool IsLong()
        {
            return Days > 10;
        }

        public string GetInfo()
        {
            return $"{Name} ({Days} дней, {Price} руб.)";
        }
    }
}