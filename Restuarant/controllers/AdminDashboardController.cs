using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Services.Configuration;
using RestaurantApp.Services.Factory;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.Controllers
{
    public class AdminDashboardController : Controller
    {
        private readonly RestaurantApp.Services.Reporting.ReportingService _reportingService;
        private readonly Restuarant.Core.Interfaces.IUnitOfWork _unitOfWork;

        public AdminDashboardController(
            RestaurantApp.Services.Reporting.ReportingService reportingService,
            Restuarant.Core.Interfaces.IUnitOfWork unitOfWork)
        {
            _reportingService = reportingService;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            // Auth Check
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
            {
                return Redirect("/Admin/Login");
            }

            // Sync Singleton with DB
            var dbSettings = (await _unitOfWork.Settings.GetAllAsync()).FirstOrDefault();
            var config = RestaurantConfig.Instance;
            if (dbSettings != null)
            {
                config.TaxRate = dbSettings.TaxRate;
                config.Currency = dbSettings.Currency;
                config.IsOpen = dbSettings.IsOpen;
            }

            ViewBag.TaxRate = config.TaxRate;
            ViewBag.Currency = config.Currency;
            ViewBag.IsOpen = config.IsOpen;

            // LINQ Usage (Reporting)
            ViewBag.DailySales = await _reportingService.GetDailySalesAsync(System.DateTime.Now);
            ViewBag.TopItems = await _reportingService.GetTopSellingItemsAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSettings(decimal taxRate, string currency, bool isOpen)
        {
            // Update DB
            var dbSettings = (await _unitOfWork.Settings.GetAllAsync()).FirstOrDefault();
            if (dbSettings != null)
            {
                dbSettings.TaxRate = taxRate;
                dbSettings.Currency = currency;
                dbSettings.IsOpen = isOpen;
                _unitOfWork.Settings.Update(dbSettings);
                _unitOfWork.Save();
            }

            // Update Singleton (for current session)
            var config = RestaurantConfig.Instance;
            config.TaxRate = taxRate;
            config.Currency = currency;
            config.IsOpen = isOpen;

            TempData["Message"] = "System settings updated and persisted successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult TestPayment(string type, decimal amount)
        {
            try
            {
                // Factory Usage
                var processor = PaymentFactory.CreatePaymentProcessor(type, "test_user");
                bool success = processor.ProcessPayment(amount);

                TempData["Message"] = $"Payment processed via {type}: {success}";
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
