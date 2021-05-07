using Microsoft.VisualStudio.TestTools.UnitTesting;
using ATM_ActivityLibrary;
using System.Collections.Generic;

namespace ATM_UnitTestProject
{
    [TestClass]
    public class ATMTests
    {
        [TestMethod]
        public void Validate_BillsRepositoryBalance_returnTrue()
        {
            Dictionary<int, int> billRepository = new Dictionary<int, int>();
            billRepository.Add(1000, 2);
            billRepository.Add(500, 3);
            billRepository.Add(100, 5);

            ATM_Activity.AddBills(billRepository);

            Assert.AreEqual(4000, ATM_Activity.CalculateTotalAmountAvailable());
        }

        [TestMethod]
        public void UserInputValidation_WrongInput_ReturnsNull()
        {
            int? amount = ATM_Activity.ValidateWithdrawalAmount("TEST");

            Assert.IsNull(amount);
        }

        [TestMethod]
        public void UserInputValidation_ValidInput_ReturnsInt()
        {
            int? amount = ATM_Activity.ValidateWithdrawalAmount("15000");

            Assert.IsNotNull(amount);
            Assert.AreEqual(amount, 15000);
        }

        [TestMethod]
        public void CheckSufficientBalanceInATM_ExceedBalance_ReturnsFalse()
        {
            Dictionary<int, int> billRepository = new Dictionary<int, int>();
            billRepository.Add(1000, 2);
            billRepository.Add(500, 3);
            billRepository.Add(100, 5);

            ATM_Activity.AddBills(billRepository);

            Assert.IsFalse(ATM_Activity.InitiateTransaction(15000));
        }

        [TestMethod]
        public void CheckSufficientBalanceInATM_ExactBalance_ReturnsTrue()
        {
            Dictionary<int, int> billRepository = new Dictionary<int, int>();
            billRepository.Add(1000, 2);
            billRepository.Add(500, 3);
            billRepository.Add(100, 5);

            ATM_Activity.AddBills(billRepository);

            Assert.IsTrue(ATM_Activity.InitiateTransaction(4000));
        }

        [TestMethod]
        public void CheckSufficientBalanceInATM_ValidBalance_ReturnsTrue()
        {
            Dictionary<int, int> billRepository = new Dictionary<int, int>();
            billRepository.Add(1000, 2);
            billRepository.Add(500, 3);
            billRepository.Add(100, 5);

            ATM_Activity.AddBills(billRepository);

            Assert.IsTrue(ATM_Activity.InitiateTransaction(1000));
        }

        [TestMethod]
        public void CalculateBillsToWithdraw_ValidTransaction_ReturnsBills()
        {
            Dictionary<int, int> billRepository = new Dictionary<int, int>();
            billRepository.Add(1000, 2);
            billRepository.Add(500, 3);
            billRepository.Add(100, 5);

            ATM_Activity.AddBills(billRepository);

            var bills = ATM_Activity.CalculateBillsToWithdraw(1600, out bool isSuccess);
            Assert.IsTrue(isSuccess);
            Assert.AreEqual(bills[1000], 1);
            Assert.AreEqual(bills[500], 1);
            Assert.AreEqual(bills[100], 1);
        }

        [TestMethod]
        public void CalculateBillsToWithdraw_InValidTransaction_ReturnsBills()
        {
            Dictionary<int, int> billRepository = new Dictionary<int, int>();
            billRepository.Add(1000, 2);
            billRepository.Add(500, 3);
            billRepository.Add(100, 5);

            ATM_Activity.AddBills(billRepository);

            var bills = ATM_Activity.CalculateBillsToWithdraw(50, out bool isSuccess);
            Assert.IsFalse(isSuccess);
        }

        [TestMethod]
        public void CommitTransaction_TotalBalanceAmountChanges_returnsTrue()
        {
            Dictionary<int, int> billRepository = new Dictionary<int, int>();
            billRepository.Add(1000, 2);
            billRepository.Add(500, 3);
            billRepository.Add(100, 5);

            ATM_Activity.AddBills(billRepository);

            var bills = ATM_Activity.CalculateBillsToWithdraw(1600, out bool isSuccess);
            Assert.IsTrue(isSuccess);
            Assert.AreEqual(bills[1000], 1);
            Assert.AreEqual(bills[500], 1);
            Assert.AreEqual(bills[100], 1);

            ATM_Activity.CommitTransaction(bills);

            Assert.AreEqual(2400, ATM_Activity.CalculateTotalAmountAvailable());
            Assert.AreEqual(ATM_Activity.billsAvailable[1000], 1);
            Assert.AreEqual(ATM_Activity.billsAvailable[500], 2);
            Assert.AreEqual(ATM_Activity.billsAvailable[100], 4);
        }
    }
}
