using System;
using System.Collections.Generic;

namespace bank;

/// <summary>
/// Главный класс приложения, координирующий запуск 
/// демонстрационных сценариев работы с банковскими счетами.
/// </summary>
public class Program
{
    /// <summary>
    /// Точка входа в программу. Выполняет инициализацию различных типов аккаунтов, 
    /// проведение транзакций, симуляцию закрытия расчетного периода и вывод логов.
    /// </summary>
    /// <param name="args">Аргументы командной строки запуска приложения.</param>
    static void Main(string[] args)
    {
        BankAccount account1 = new BankAccount("Sasha", 100000);
        BankAccount account2 = new BankAccount("Pasha", 1000);
        Console.WriteLine($"account {account1.Balance} №{account1.Number} {account1.Owner}");
        Console.WriteLine($"account {account2.Balance} №{account2.Number} {account2.Owner}");

        account1.MakeDeposit(2000000, DateTime.UtcNow, ":)");
        Console.WriteLine(account1.Balance);
        account1.MakeWithdrawal(200, DateTime.UtcNow, ":(");
        Console.WriteLine(account1.Balance);
        Console.WriteLine(account1.GetAccountHistory());
        try
        {
            account2.MakeWithdrawal(10000, DateTime.UtcNow, ":(");
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
        }
        InterestEarningAccount interest = new InterestEarningAccount("Sasha", 1000m);
        interest.PerformMonthAndTransactions();
        Console.WriteLine(interest.GetAccountHistory());


        LineOfCreditAccount lineOfCredit = new LineOfCreditAccount("Sasha", 0, 1000m);
        lineOfCredit.MakeWithdrawal(500m, DateTime.UtcNow, "credit");

        GiftCardAccount giftcart = new GiftCardAccount("Sasha", 1000m, 5000m);

        List<BankAccount> accounts = new List<BankAccount>();
        accounts.Add(account1);
        accounts.Add(interest);
        accounts.Add(lineOfCredit);
        accounts.Add(giftcart);

        foreach (BankAccount account in accounts)
        {
            Console.WriteLine(account);
            account.PerformMonthAndTransactions();
            Console.WriteLine(account.GetAccountHistory());
        }

        lineOfCredit.MakeWithdrawal(600m, DateTime.UtcNow, "credit");
        Console.WriteLine(lineOfCredit.GetAccountHistory());

        // Демонстрация работы Переводов (Задача 3)
        Console.WriteLine("--- ДЕМОНСТРАЦИЯ ПЕРЕВОДА ---");
        TransferService transferService = new TransferService();
        transferService.Transfer(account1, account2, 50000m);
        Console.WriteLine(account1.GetAccountHistory());
        Console.WriteLine(account2.GetAccountHistory());
    }
}
