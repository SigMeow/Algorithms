namespace bank;
/// <summary>
/// Описывает специализированный счет подарочной карты с поддержкой 
/// автоматических ежемесячных начислений фиксированной суммы.
/// </summary>
public class GiftCardAccount : BankAccount
{
	private readonly decimal _monthlyDeposit = 0m;

	// monthlyDeposit - параметр по умолчанию, по умолчанию принимает 0,
	// при создании new GiftCardAccount("Ulyana", 1000); - monthlyDeposit = 0
	// new GiftCardAccount("Ulyana", 1000, 5000) => monthlyDeposit = 5000
	/// <summary>
	/// Создает новый счет подарочной карты для указанного клиента 
	/// с возможностью настройки регулярного ежемесячного пополнения.
	/// </summary>
	/// <param name="name">Имя владельца карты</param>
	/// <param name="initialBalance">Стартовый баланс</param>
	/// <param name="monthlyDeposit">Величина регулярного ежемесячного взноса</param>
	public GiftCardAccount(string name, decimal initialBalance, decimal monthlyDeposit = 0)
		: base(name, initialBalance)
		=> _monthlyDeposit = monthlyDeposit;
	/// <summary>
	/// Выполняет плановое пополнение баланса карты в конце 
	/// расчетного периода, если установлена сумма ежемесячного взноса.
	/// </summary>
	public override void PerformMonthAndTransactions()
	{
		if (_monthlyDeposit != 0)
		{
			MakeDeposit(_monthlyDeposit, DateTime.UtcNow, "Add monthly deposit");
		}
	}
	/// <summary>
	/// Возвращает расширенную текстовую информацию о состоянии 
	/// подарочной карты, включая параметры ежемесячного пополнения.
	/// </summary>
	/// <returns>Строка с подробными характеристиками счета.</returns>
	public override string ToString() => base.ToString() + $"monthly deposit: {_monthlyDeposit}";
}
