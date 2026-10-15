using System;
using System.Collections.Generic;
using System.Text;

namespace bank;

// Добавляем перечисление статусов в пространство имен
public enum AccountStatus { Regular, Premium, VIP }

/// <summary>
/// Описывает банковский аккаунт, поддерживающий финансовые операции 
/// (зачисления, списания) и сохранение истории транзакций.
/// </summary>
public class BankAccount
{
    private readonly decimal _minimumBalance;
    static private int s_accountNumberSeed = 1000000000;
    public string Number { get; }
    public string Owner { get; private set; }
    private List<Transaction> _allTransactions = new List<Transaction>();

    /// <summary>
    /// Текущий статус клиента для расчета условий обслуживания.
    /// </summary>
    public AccountStatus Status { get; set; } = AccountStatus.Regular;

    /// <summary>
    /// Актуальный остаток денежных средств на счете, 
    /// рассчитанный на основе проведенных операций.
    /// </summary>
    public decimal Balance
    {
        get
        {
            decimal balance = 0;
            foreach (var transaction in _allTransactions)
            {
                balance += transaction.Amount;
            }
            return balance;
        }
    }
    /// <summary>
    /// Создает новый расчетный счет с привязкой к клиенту и фиксацией стартового баланса.
    /// </summary>
    /// <param name="name">Имя владельца счета</param>
    /// <param name="initialBalance">Начальный баланс</param>
    public BankAccount(string name, decimal initialBalance) : this(name, initialBalance, 0)
    {

    }
    /// <summary>
    /// Создает новый расчетный счет с учетом ограничений по минимальному остатку.
    /// </summary>
    /// <param name="name">Имя владельца счета</param>
    /// <param name="initialBalance">Начальный баланс</param>
    /// <param name="minimumBalance">Минимальный баланс</param>
    public BankAccount(string name, decimal initialBalance, decimal minimumBalance)
    {
        Owner = name;
        Number = s_accountNumberSeed.ToString();
        s_accountNumberSeed++;

        _minimumBalance = minimumBalance;

        if (initialBalance > 0)
            MakeDeposit(initialBalance, DateTime.UtcNow, "Initial balance");
    }
    /// <summary>
    /// Выполняет операцию пополнения счета на указанную сумму с добавлением описания.
    /// </summary>
    /// <param name="amount">Сумма депозита</param>
    /// <param name="date">Дата депозита</param>
    /// <param name="note">Примечание к депозиту</param>
    /// <exception cref="ArgumentOutOfRangeException">исключение на сумму депозита</exception>
    public void MakeDeposit(decimal amount, DateTime date, string note)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount of deposit must be positive");
        }
        var deposit = new Transaction(amount, date, note);
        _allTransactions.Add(deposit);
    }
    /// <summary>
    /// Осуществляет списание денежных средств со счета, проверяя доступные лимиты.
    /// </summary>
    /// <param name="amount">Сумма снятия</param>
    /// <param name="date">Дата снятия</param>
    /// <param name="note">Примечание к снятию</param>
    public void MakeWithdrawal(decimal amount, DateTime date, string note)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        Transaction? overdraftTransaction = CheckWithdrawalLimit(Balance - amount < _minimumBalance);
        Transaction? withdrawal = new(-amount, date, note);

        _allTransactions.Add(withdrawal);

        if (overdraftTransaction is not null)
            _allTransactions.Add(overdraftTransaction);
    }
    protected virtual Transaction? CheckWithdrawalLimit(bool isOverdrawn)
    {
        if (isOverdrawn)
        {
            throw new InvalidOperationException("Not sufficient rubls for this withdrawal");
        }
        else
        {
            return default;
        }
    }
    /// <summary>
    /// Формирует детализированный отчет по всем движениям денежных средств на аккаунте.
    /// </summary>
    /// <returns>  </returns>
    public string GetAccountHistory()
    {
        var report = new StringBuilder();
        decimal balance = 0;
        report.AppendLine("Date\t\tAmount\tBalance\tNote");
        foreach (var item in _allTransactions)
        {
            balance += item.Amount;
            report.AppendLine($"" +
                $"{item.Date.ToShortDateString()}\t" +
                $"{item.Amount}\t{balance}\t{item.Note}");
        }
        return report.ToString();
    }
    // Ключевое слово virtual позволяет в дочернем классе предоставить другую реализацию 
    // Метода PerformMonthAndTransactions
    /// <summary>
    /// Запускает регламентные процедуры обработки счетов по завершении расчетного периода.
    /// </summary>
    public virtual void PerformMonthAndTransactions()
    {
        // Начисление комиссии для Regular
        if (Status == AccountStatus.Regular)
        {
            MakeWithdrawal(50m, DateTime.UtcNow, "Monthly maintenance fee");
        }

        // Проценты на остаток свыше 10 000 для Premium и VIP
        if ((Status == AccountStatus.Premium || Status == AccountStatus.VIP) && Balance > 10000m)
        {
            decimal excess = Balance - 10000m;
            decimal rate = Status == AccountStatus.Premium ? 0.01m : 0.03m;
            decimal interest = excess * rate;
            if (interest > 0)
            {
                MakeDeposit(interest, DateTime.UtcNow, "Interest on balance over 10000");
            }
        }

        // Кэшбэк 5% для VIP на все списания за последний месяц
        if (Status == AccountStatus.VIP)
        {
            decimal totalWithdrawals = 0;
            DateTime oneMonthAgo = DateTime.UtcNow.AddMonths(-1);

            foreach (var tx in _allTransactions)
            {
                if (tx.Amount < 0 && tx.Date >= oneMonthAgo && !tx.Note.Contains("overdraft fee") && !tx.Note.Contains("maintenance fee"))
                {
                    totalWithdrawals += Math.Abs(tx.Amount);
                }
            }

            decimal cashback = totalWithdrawals * 0.05m;
            if (cashback > 0)
            {
                MakeDeposit(cashback, DateTime.UtcNow, "VIP 5% Cashback");
            }
        }
    }

    /// <summary>
    /// Формирует сводную текстовую информацию о текущем состоянии и параметрах банковского счета.
    /// </summary>
    /// <returns>  </returns>
    public override string ToString()
    {
        return $"Type: {GetType().Name}\t" +
            $"Owner: {Owner}\t" +
            $"Number of account: {Number}\t" +
            $"Balance: {Balance}";
    }
}
