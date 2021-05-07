using System;
using System.Collections.Generic;
using ATM_ActivityLibrary;

namespace Project_ATM
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, int> billRepository = new Dictionary<int, int>();
            billRepository.Add(1000, 2);
            billRepository.Add(500, 3);
            billRepository.Add(100, 5);
            ATM_Activity.AddBills(billRepository);

        retry:
            Console.WriteLine("Please enter valid option to continue. ");
            Console.WriteLine("     1. Auto Transaction - Assignment transactions ");
            Console.WriteLine("     2. Manual Transaction");

            if (!int.TryParse(Console.ReadLine(), out int option))
            {
                goto retry;
            }
            else if (option == 1 || option == 2)
            {
                if (option == 1)
                {
                    List<string> autoTransactions = new List<string> { "1500", "700", "400", "1100", "1000", "700", "300" };
                    foreach (var autoAmount in autoTransactions)
                    {
                        StartATM(false, autoAmount);
                    }
                }
                else
                    StartATM(true);
            }
            else
            {
                goto retry;
            }

            Console.ReadLine();
        }

        private static void StartATM(bool isManual, string autoAmount = null)
        {
            do
            {
                Console.ForegroundColor = ConsoleColor.White;

                int amount = ATM_Activity.CalculateTotalAmountAvailable();

                Console.WriteLine($"Total Amount Available in the ATM: {amount}");

                Console.WriteLine($"Total bills Available in the ATM: ");

                foreach (KeyValuePair<int, int> bill in ATM_Activity.billsAvailable)
                {
                    if (bill.Value > 0)
                        Console.WriteLine($"    {bill.Key} X {bill.Value} = {bill.Key * bill.Value}");
                }

                if (isManual)
                {
                    Console.WriteLine("Kindly enter the amount you wish to withdraw: ");

                    BeginTransaction(Console.ReadLine());
                }
                else
                    BeginTransaction(autoAmount);
            }
            while (isManual);
        }

        private static void BeginTransaction(string inputAmount)
        {
            Console.WriteLine($"Amount to withdraw - {inputAmount}");

            int? withdrawalAmount = ATM_Activity.ValidateWithdrawalAmount(inputAmount);
            if (withdrawalAmount != null)
            {
                if (ATM_Activity.InitiateTransaction(withdrawalAmount.Value))
                {
                    Console.WriteLine("Processing.....");
                    ProcessTransaction(withdrawalAmount.Value);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("*********************************************************");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Transaction failure!");
                    Console.WriteLine("Insufficient amount available, please try again tomorrow!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("*********************************************************");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Transaction failure!");
                Console.WriteLine("Please enter valid amount!");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("*********************************************************");
            }
        }

        private static void ProcessTransaction(int withdrawalAmount)
        {            
            var billsToWithdraw = ATM_Activity.CalculateBillsToWithdraw(withdrawalAmount, out bool isSuccess);

            if (isSuccess)
            {
                foreach (KeyValuePair<int, int> bill in billsToWithdraw)
                {
                    Console.WriteLine($"    {bill.Key} X {bill.Value} = {bill.Key * bill.Value}");
                }
                Console.WriteLine($"Please collect your cash ...");

                ATM_Activity.CommitTransaction(billsToWithdraw);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Transaction successful!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Transaction failure!");
                Console.WriteLine($"Unfortunately ATM can only disburse amount in denomination of 1000, 500 & 100");
            }
        }
    }
}
