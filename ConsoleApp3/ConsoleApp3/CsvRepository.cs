using System;
using System.Collections.Generic;
using System.IO;

namespace TourAgencyApp
{
    public class CsvRepository
    {
        private string _basePath;

        public CsvRepository(string basePath)
        {
            _basePath = basePath;
        }

        public List<Country> GetCountries()
        {
            List<Country> result = new List<Country>();
            string path = Path.Combine(_basePath, "countries.csv");
            if (!File.Exists(path)) return result;

            string[] lines = File.ReadAllLines(path);
            if (lines.Length < 2) return result;

            for (int i = 1; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(' ');
                if (parts.Length < 3) continue;

                // Считываем данные в переменные
                int id = int.Parse(parts[0]);
                string name = parts[1].Replace('_', ' ');
                string continent = parts[2];

                // Передаем их в конструктор
                result.Add(new Country(id, name, continent));
            }
            return result;
        }

        public List<Manager> GetManagers()
        {
            List<Manager> result = new List<Manager>();
            string path = Path.Combine(_basePath, "managers.csv");
            if (!File.Exists(path)) return result;

            string[] lines = File.ReadAllLines(path);
            if (lines.Length < 2) return result;

            for (int i = 1; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(' ');
                if (parts.Length < 4) continue;

                int id = int.Parse(parts[0]);
                string fullName = parts[1].Replace('_', ' ');
                string phone = parts[2];
                int experience = int.Parse(parts[3]);

                result.Add(new Manager(id, fullName, phone, experience));
            }
            return result;
        }

        public List<Tour> GetTours()
        {
            List<Tour> result = new List<Tour>();
            string path = Path.Combine(_basePath, "tours.csv");
            if (!File.Exists(path)) return result;

            string[] lines = File.ReadAllLines(path);
            if (lines.Length < 2) return result;

            for (int i = 1; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(' ');
                if (parts.Length < 6) continue;

                int id = int.Parse(parts[0]);
                string name = parts[1];
                int countryId = int.Parse(parts[2]);
                int managerId = int.Parse(parts[3]);
                decimal price = decimal.Parse(parts[4]);
                int days = int.Parse(parts[5]);

                result.Add(new Tour(id, name, countryId, managerId, price, days));
            }
            return result;
        }
    }
}