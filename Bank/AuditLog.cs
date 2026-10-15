using System;

namespace bank;

/// <summary>
/// Предоставляет централизованный журнал для регистрации финансовых изменений по счетам.
/// </summary>
public static class AuditLog
{
    /// <summary>
    /// Записывает сведения о совершенной финансовой операции в консоль аудита.
    /// </summary>
    /// <param name="accountNumber">Уникальный номер банковского счета.</param>
    /// <param name="type">Категория выполняемой операции (Deposit / Withdrawal / Interest).</param>
    /// <param name="amount">Размер денежных средств, участвующих в транзакции.</param>
    /// <param name="date">Фактическое время проведения операции.</param>
    public static void Log(string accountNumber, string type, decimal amount, DateTime date)
    {
        Console.WriteLine($"[AUDIT] Счет: {accountNumber} | Тип: {type} | Сумма: {amount} | Дата: {date}");
    }
}
