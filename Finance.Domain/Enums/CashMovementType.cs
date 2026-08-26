namespace Finance.Domain.Enums;

public enum CashMovementType
{
    OpeningFloat = 1,
    Deposit = 2,
    Withdrawal = 3,
    SaleCash = 4,
    CustomerPayment = 5,
    ExpensePayment = 6,
    ObligationSettlement = 7,
	Inbound=8,
	Outbound=9
}
