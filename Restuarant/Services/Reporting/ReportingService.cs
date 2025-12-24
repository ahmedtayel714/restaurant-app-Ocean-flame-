using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantApp;
using Restuarant.Data;
using Restuarant.Core.Interfaces;

namespace RestaurantApp.Services.Reporting
{
    public class ReportingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReportingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // LINQ: Aggregate Function (Sum) & Filtering (Where)
        public async Task<decimal> GetDailySalesAsync(DateTime date)
        {
            var orders = await _unitOfWork.Orders.GetAllAsync(o => o.CreatedAt.Date == date.Date && o.Status == "Completed");
            return orders.Sum(o => o.TotalPrice);
        }

        // LINQ: Grouping, Ordering, and Projections
        public async Task<IEnumerable<TopItemDto>> GetTopSellingItemsAsync(int count = 5)
        {
            var orders = await _unitOfWork.Orders.GetAllAsync(o => o.Status == "Completed", includeProperties: "Items");
            
            var topItems = orders
                .SelectMany(o => o.Items)
                .GroupBy(i => i.Name)
                .Select(g => new TopItemDto
                { 
                    ItemName = g.Key, 
                    TotalQuantity = g.Sum(i => i.Quantity),
                    TotalRevenue = g.Sum(i => i.Total)
                })
                .OrderByDescending(x => x.TotalQuantity)
                .Take(count)
                .ToList();

            return topItems;
        }

        // LINQ: Filtering & Sorting
        public async Task<IEnumerable<Reservation>> GetPendingReservationsAsync()
        {
            var reservations = await _unitOfWork.Reservations.GetAllAsync(r => r.Status == "Pending");
            return reservations.OrderBy(r => r.CreatedAt);
        }
    }

    public class TopItemDto
    {
        public string ItemName { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}