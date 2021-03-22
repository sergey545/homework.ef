﻿namespace Otus.Teaching.PromoCodeFactory.DataAccess.Data
{
    public class MyDbInitializer : IMyDbInitializer
    {
        private readonly Context _context;

        public MyDbInitializer(Context context)
        {
            _context = context;
        }
        
        public void Init()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            
            _context.AddRange(FakeDataFactory.Employees);
            _context.SaveChanges();
            
            _context.AddRange(FakeDataFactory.Preferences);
            _context.SaveChanges();
            
            _context.AddRange(FakeDataFactory.Customers);
            _context.SaveChanges();
        }
    }
}