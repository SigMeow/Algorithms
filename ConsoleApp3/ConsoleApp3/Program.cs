using System;
using System.Collections.Generic;

namespace TourAgencyApp
{
    internal class Program
    {
        /// <summary>
        /// Точка входа. Спрашивает источник данных, загружает списки и вызывает аналитические методы.
        /// Все операции обёрнуты в try/catch: при ошибке программа останавливается и выводит сообщение.
        /// </summary>
        /// <param name="args">Аргументы командной строки (не используются).</param>
        static void Main(string[] args)
        {

            try
            {
                Console.WriteLine("Выберите источник данных:");
                Console.WriteLine("1 - InMemoryRepository");
                Console.WriteLine("2 - CsvRepository");
                Console.Write("Ваш выбор: ");

                string input = Console.ReadLine();
                int choice;
                if (!int.TryParse(input, out choice))
                {
                    Console.WriteLine("Неверный выбор");
                    return;
                }

                List<Country> countries = new List<Country>();
                List<Manager> managers = new List<Manager>();
                List<Tour> tours = new List<Tour>();

                switch (choice)
                {
                    case 1:
                        InMemoryRepository repo1 = new InMemoryRepository();
                        countries = repo1.GetCountries();
                        managers = repo1.GetManagers();
                        tours = repo1.GetTours();
                        break;
                    case 2:
                        CsvRepository repo2 = new CsvRepository("data");
                        countries = repo2.GetCountries();
                        managers = repo2.GetManagers();
                        tours = repo2.GetTours();
                        break;
                    default:
                        Console.WriteLine("Неверный выбор");
                        return;
                }

                Console.WriteLine();

                Console.WriteLine("1. FindManager(\"Типичный отдых\"):");
                Manager manager = FindManager(tours, managers, "Типичный_отдых");
                Console.WriteLine(manager != null ? manager.GetInfo() : "Не найдено");

                Console.WriteLine("\n2. FindCountry(tour \"Типичный отдых\"):");
                Country country = FindCountry(tours, countries, "Типичный_отдых");
                Console.WriteLine(country != null ? country.GetInfo() : "Не найдено");

                Console.WriteLine("\n3. GetTotalDays:");
                Console.WriteLine(GetTotalDays(tours));

                Console.WriteLine("\n4. GetMostPopularCountry:");
                Country top = GetMostPopularCountry(tours, countries);
                Console.WriteLine(top != null ? $"{top.Name} ({CountToursByCountry(tours, top.Id)} тур.)" : "Не найдено");

                Console.WriteLine("\n5. PrintAllTours:");
                PrintAllTours(tours, managers, countries);

                Console.WriteLine();
                Manager notFound = FindManager(tours, managers, "Неизвестный_тур");
                Console.WriteLine("Не найдено: FindManager(\"Неизвестный тур\") -> " +
                                  (notFound == null ? "null" : notFound.GetInfo()));
            }
            catch (ArgumentException ex)
            {
                // Сработает, например, если Days <= 0 в конструкторе Tour
                Console.WriteLine($"Ошибка данных: {ex.Message}");
                Console.WriteLine("Дальнейшие команды не выполняются.");
            }
            catch (FormatException ex)
            {
                // Ошибка парсинга числа из CSV
                Console.WriteLine($"Ошибка формата числа: {ex.Message}");
                Console.WriteLine("Дальнейшие команды не выполняются.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неизвестная ошибка: {ex.Message}");
            }

            Console.ReadLine();
        }

        /// <summary>
        /// Поиск менеджера по названию тура.
        /// </summary>
        /// <param name="tours">Список всех туров.</param>
        /// <param name="managers">Список всех менеджеров.</param>
        /// <param name="tourName">Название искомого тура.</param>
        /// <returns>Объект Manager, либо null, если тур не найден.</returns>
        static Manager FindManager(List<Tour> tours, List<Manager> managers, string tourName)
        {
            Tour found = null;
            foreach (Tour t in tours)
            {
                if (t.Name == tourName) { found = t; break; }
            }
            if (found == null) return null;

            foreach (Manager m in managers)
            {
                if (m.Id == found.ManagerId) return m;
            }
            return null;
        }

        /// <summary>
        /// Поиск страны по названию тура.
        /// </summary>
        /// <param name="tours">Список всех туров.</param>
        /// <param name="countries">Список всех стран.</param>
        /// <param name="tourName">Название искомого тура.</param>
        /// <returns>Объект Country, либо null, если тур не найден.</returns>
        static Country FindCountry(List<Tour> tours, List<Country> countries, string tourName)
        {
            Tour found = null;
            foreach (Tour t in tours)
            {
                if (t.Name == tourName) { found = t; break; }
            }
            if (found == null) return null;

            foreach (Country c in countries)
            {
                if (c.Id == found.CountryId) return c;
            }
            return null;
        }

        /// <summary>
        /// Суммарное количество дней во всех турах.
        /// </summary>
        /// <param name="tours">Список туров.</param>
        /// <returns>Общее число дней. Пустой список — 0.</returns>
        static int GetTotalDays(List<Tour> tours)
        {
            int total = 0;
            foreach (Tour t in tours) total += t.Days;
            return total;
        }

        /// <summary>
        /// Поиск самой популярной страны (по числу туров).
        /// При равенстве — первая найденная.
        /// </summary>
        /// <param name="tours">Список туров.</param>
        /// <param name="countries">Список стран.</param>
        /// <returns>Объект Country с максимумом туров, либо null, если туров нет.</returns>
        static Country GetMostPopularCountry(List<Tour> tours, List<Country> countries)
        {
            if (tours.Count == 0) return null;

            int bestCountryId = -1;
            int bestCount = -1;

            foreach (Country c in countries)
            {
                int count = CountToursByCountry(tours, c.Id);

                if (count > bestCount)
                {
                    bestCount = count;
                    bestCountryId = c.Id;
                }
            }

            foreach (Country c in countries)
            {
                if (c.Id == bestCountryId) return c;
            }
            return null;
        }

        /// <summary>
        /// Считает количество туров в указанной стране.
        /// </summary>
        /// <param name="tours">Список туров.</param>
        /// <param name="countryId">Идентификатор страны.</param>
        /// <returns>Число туров в стране.</returns>
        static int CountToursByCountry(List<Tour> tours, int countryId)
        {
            int count = 0;
            foreach (Tour t in tours)
            {
                if (t.CountryId == countryId) count++;
            }
            return count;
        }

        /// <summary>
        /// Вывод всех туров с информацией о менеджере и стране.
        /// Если менеджер или страна не найдены — выводится "—".
        /// </summary>
        /// <param name="tours">Список туров.</param>
        /// <param name="managers">Список менеджеров.</param>
        /// <param name="countries">Список стран.</param>
        static void PrintAllTours(List<Tour> tours, List<Manager> managers, List<Country> countries)
        {
            foreach (Tour t in tours)
            {
                Manager manager = null;
                foreach (Manager m in managers)
                {
                    if (m.Id == t.ManagerId) { manager = m; break; }
                }

                Country country = null;
                foreach (Country c in countries)
                {
                    if (c.Id == t.CountryId) { country = c; break; }
                }

                string managerName = manager != null ? manager.FullName : "—";
                string countryName = country != null ? country.Name : "—";

                Console.WriteLine($"\"{t.GetInfo()}\" - менеджер {managerName}, страна \"{countryName}\"");
            }
        }
    }
}