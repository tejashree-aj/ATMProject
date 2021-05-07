using System;
using System.Collections.Generic;
using System.Linq;

namespace ATM_ActivityLibrary
{
    public static class ATM_Activity
    {
        private static int amount = 0;
        public static Dictionary<int, int> billsAvailable;

        /// <summary>
        /// Creating cash available using Dictionary
        /// </summary>
        public static void AddBills(Dictionary<int, int> _billsAvailable)
        {
            billsAvailable = _billsAvailable;

            CalculateTotalAmountAvailable();
        }

        /// <summary>
        /// Calculate total amount available
        /// </summary>
        public static int CalculateTotalAmountAvailable()
        {
            amount = 0;
            foreach (KeyValuePair<int, int> item in billsAvailable)
            {
                amount += item.Key * item.Value;
            }
            return amount;
        }

        /// <summary>
        /// Validate user input - If entered correct amount
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        public static int? ValidateWithdrawalAmount(string userInput)
        {
            if (int.TryParse(userInput, out int withdrawalAmount))
            {
                return withdrawalAmount;
            }
            else
                return null;
        }

        /// <summary>
        /// check if sufficient balance available in the ATM
        /// </summary>
        /// <param name="withdrawalAmount"></param>
        /// <returns></returns>
        public static bool InitiateTransaction(int withdrawalAmount)
        {
            //If input amount less the the available balance then InitiateTransaction.
            if (withdrawalAmount <= amount)
            {
                return true;
            }
            //If input amount greater then the available balance then end transaction
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Calculate bills available to perform transaction
        /// </summary>
        /// <param name="withdrawalAmount"></param>
        /// <param name="isSuccess"></param>
        /// <returns></returns>
        public static Dictionary<int, int> CalculateBillsToWithdraw(int withdrawalAmount, out bool isSuccess)
        {
            isSuccess = false;
            int calculatedWithdrawalAmount = 0;
            Dictionary<int, int> billsToWithdraw = new Dictionary<int, int>();
            foreach (var currency in billsAvailable.Where(x => x.Value > 0).OrderByDescending(x => x.Key))
            {
                //if available cash 
                if (currency.Key <= withdrawalAmount - calculatedWithdrawalAmount)
                {
                    int currencyRequired = (withdrawalAmount - calculatedWithdrawalAmount) / currency.Key;
                    if (currencyRequired <= currency.Value)
                    {
                        calculatedWithdrawalAmount += currencyRequired * currency.Key;
                        billsToWithdraw.Add(currency.Key, currencyRequired);
                    }
                    else
                    {
                        calculatedWithdrawalAmount += currency.Value * currency.Key;
                        billsToWithdraw.Add(currency.Key, currencyRequired);
                    }
                }
            }
            if (withdrawalAmount == calculatedWithdrawalAmount)
                isSuccess = true;

            return billsToWithdraw;
        }

        /// <summary>
        /// Finalize the transaction
        /// </summary>
        /// <param name="billsToWithdraw"></param>
        public static void CommitTransaction(Dictionary<int, int> billsToWithdraw)
        {
            foreach (KeyValuePair<int, int> bill in billsToWithdraw)
            {
                billsAvailable[bill.Key] = billsAvailable[bill.Key] - bill.Value;
            }
        }
    }
}
