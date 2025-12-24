using System;
using RestaurantApp;
using Restuarant.Core.Interfaces;
using Restuarant.Data.Repositories;

namespace Restuarant.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RestaurantContext _context;
        private IGenericRepository<MenuItem> _menuItems;
        private IGenericRepository<Order> _orders;
        private IGenericRepository<Reservation> _reservations;
        private IGenericRepository<SystemSettings> _settings;
        
        public UnitOfWork(RestaurantContext context)
        {
            _context = context;
        }

        public IGenericRepository<SystemSettings> Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = new GenericRepository<SystemSettings>(_context);
                }
                return _settings;
            }
        }

        public IGenericRepository<Reservation> Reservations
        {
            get
            {
                if (_reservations == null)
                {
                    _reservations = new GenericRepository<Reservation>(_context);
                }
                return _reservations;
            }
        }

        public IGenericRepository<MenuItem> MenuItems
        {
            get
            {
                if (_menuItems == null)
                {
                    _menuItems = new GenericRepository<MenuItem>(_context);
                }
                return _menuItems;
            }
        }

        public IGenericRepository<Order> Orders
        {
            get
            {
                if (_orders == null)
                {
                    _orders = new GenericRepository<Order>(_context);
                }
                return _orders;
            }
        }



        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
