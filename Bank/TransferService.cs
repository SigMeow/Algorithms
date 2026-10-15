using System;

namespace bank;

/// <summary>
/// Сервисный класс, управляющий межбанковскими переводами между пользователями.
/// </summary>
public class TransferService
{
    /// <summary>
    /// Осуществляет перевод денежных средств со счета-отправителя на счет-получатель.
    /// </summary>
    /// <param name="source">Счет, с которого списываются средства.</param>
    /// <param name="destination">Счет, на который зачисляются средства.</param>
    /// <param name="amount">Сумма перевода.</param>
    public void Transfer(BankAccount source, BankAccount destination, decimal amount)
    {
        if (source.Number == destination.Number)
        {
            throw new ArgumentException("Cannot transfer money to the same account.");
        }

        // Списываем деньги со счета-отправителя
        source.MakeWithdrawal(amount, DateTime.UtcNow, $"Transfer to account {destination.Number}");

        try
        {
            // Пробуем зачислить на счет-получатель
            destination.MakeDeposit(amount, DateTime.UtcNow, $"Transfer from account {source.Number}");
        }
        catch (Exception)
        {
            // Откат операции: если зачисление не удалось, возвращаем деньги отправителю
            source.MakeDeposit(amount, DateTime.UtcNow, "Rollback: transfer failed");
            throw;
        }
    }
}
