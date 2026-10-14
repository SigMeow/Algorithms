namespace bank;
/// <summary>
/// Описывает накопительную модель банковского счета, предусматривающую 
/// регулярное процентное вознаграждение при сохранении минимального остатка.
/// </summary>
public class InterestEarningAccount : BankAccount
{
    /// <summary>
    /// Создает новый сберегательный счет с привязкой к имени клиента и стартовым капиталом.
    /// </summary>
    /// <param name="name">Имя владельца счета</param>
    /// <param name="initialBalance">Начальный баланс</param>
    public InterestEarningAccount(string name, decimal initialBalance)
         : base(name, initialBalance)
    {

    }
    // override позволяет в дочернем классе определить новую реализацию
    // метода PerformMonthAndTransactions
    /// <summary>
    /// Начисляет бонусные проценты на остаток средств, если 
    /// текущие накопления превышают установленный минимальный порог.
    /// </summary>
    public override void PerformMonthAndTransactions()
    {
        if (Balance > 500m)
        {
            decimal interest = Balance * 0.02m;
            MakeDeposit(interest, DateTime.UtcNow, "Apply month interest");
        }

    }
}
