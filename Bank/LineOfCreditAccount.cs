using System;

namespace bank;

/// <summary>
/// Описывает кредитный банковский счет, предоставляющий доступ к заемным средствам 
/// в рамках установленного лимита и предусматривающий комиссию за овердрафт.
/// </summary>
public class LineOfCreditAccount : BankAccount
{
    private decimal _creditLimit;
    /// <summary>
    /// Создает новый счет с открытой кредитной линией, привязанный к пользователю, 
    /// с фиксацией собственных средств и доступного кредитного плеча.
    /// </summary>
    /// <param name="name">Имя владельца счета</param>
    /// <param name="initialBalance">Начальный баланс</param>
    /// <param name="creditLimit">Размер доступного кредитного лимита</param>
    public LineOfCreditAccount(string name, decimal initialBalance, decimal creditLimit)
        : base(name, initialBalance, -creditLimit)
    {
        _creditLimit = creditLimit;
    }
    /// <summary>
    /// Анализирует состояние баланса при списании и формирует 
    /// штрафную транзакцию в случае ухода в отрицательную зону.
    /// </summary>
    /// <param name="isOverdrawn">Флаг, указывающий на факт превышения доступных средств</param>
    /// <returns>Объект транзакции с удержанной комиссией или null, если лимит не нарушен</returns>
    protected override Transaction? CheckWithdrawalLimit(bool isOverdrawn)
    {
        if (!isOverdrawn) return null;

        return new Transaction(-20m, DateTime.UtcNow, "Apply overdraft fee");
    }

    // В данном классе метод PerformMonthAndTransactions отсутствовал, добавляем его ради вызова базовых статусов
    /// <summary>
    /// Выполняет ежемесячный расчет комиссий по статусу для кредитной линии.
    /// </summary>
    public override void PerformMonthAndTransactions()
    {
        base.PerformMonthAndTransactions();
    }
}
