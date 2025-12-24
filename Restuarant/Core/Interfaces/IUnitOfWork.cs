using System;
using RestaurantApp;

namespace Restuarant.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<MenuItem> MenuItems { get; }
        IGenericRepository<Order> Orders { get; }
        IGenericRepository<Reservation> Reservations { get; }
        IGenericRepository<SystemSettings> Settings { get; }
        
        void Save();
    }
}
