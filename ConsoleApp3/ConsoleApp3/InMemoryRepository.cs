using System.Collections.Generic;

namespace TourAgencyApp
{
    public class InMemoryRepository
    {
        private List<Country> _countries;
        private List<Manager> _managers;
        private List<Tour> _tours;

        public InMemoryRepository()
        {
            // Теперь используем конструкторы!
            _countries = new List<Country>
            {
                new Country(1, "Турция",   "Европа"),
                new Country(2, "Италия",   "Европа"),
                new Country(3, "Египет",   "Африка"),
                new Country(4, "Тайланд",  "Азия"),
                new Country(5, "Испания",  "Европа")
            };

            _managers = new List<Manager>
            {
                new Manager(1, "Иванова А.А.",  "+7-900-111-11-11", 5),
                new Manager(2, "Петров П.П.",   "+7-900-222-22-22", 2),
                new Manager(3, "Сидорова Е.В.", "+7-900-333-33-33", 7),
                new Manager(4, "Кузнецов И.И.", "+7-900-444-44-44", 1),
                new Manager(5, "Орлова М.И.",   "+7-900-555-55-55", 10)
            };

            _tours = new List<Tour>
            {
                new Tour(1, "Типичный_отдых",       1, 1, 50000, 7),
                new Tour(2, "Экскурсионный",        2, 2, 40000, 5),
                new Tour(3, "Пляжный_рай",          3, 3, 60000, 10),
                new Tour(4, "Азиатское_приключение", 4, 5, 80000, 14),
                new Tour(5, "Европейское_турне",    5, 1, 90000, 12)
            };
        }

        public List<Country> GetCountries() { return _countries; }
        public List<Manager> GetManagers() { return _managers; }
        public List<Tour> GetTours() { return _tours; }
    }
}