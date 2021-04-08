using InventoryManagementDB.Data.InventoryManagement;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace InventoryManagement.Data
{
    public class InventoryService
    {
        private readonly InventorymanagementdbContext _context;
        public InventoryService(InventorymanagementdbContext context)
        {
            _context = context;
        }
        public async Task<List<Inventory>>
            GetItemAsync(string strCurrentUser)
        {
            // Get Weather Forecasts  
            return await _context.Inventory
                 // Only get entries for the current logged in user
                 .Where(x => x.UserName == strCurrentUser)
                 // Use AsNoTracking to disable EF change tracking
                 // Use ToListAsync to avoid blocking a thread
                 .AsNoTracking().ToListAsync();
        }

        // CREATE
        public Task<Inventory>
           CreateItemAsync(Inventory objInventory)
        {
            _context.Inventory.Add(objInventory);
            _context.SaveChanges();
            return Task.FromResult(objInventory);
        }

        // EDIT
        public Task<bool>
            UpdateItemAsync(Inventory objInventory)
        {
            var ExistingInventory =
                _context.Inventory
                .Where(x => x.Id == objInventory.Id)
                .FirstOrDefault();
            if (ExistingInventory != null)
            {
                ExistingInventory.Object =
                    objInventory.Object;
                ExistingInventory.Location =
                    objInventory.Location;
                ExistingInventory.Amount =
                    objInventory.Amount;
                ExistingInventory.Comment =
                    objInventory.Comment;
                ExistingInventory.Date =
                    objInventory.Date;
                _context.SaveChanges();
            }
            else
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }

        // DELETE
        public Task<bool>
            DeleteItemAsync(Inventory objInventory)
        {
            var ExistingInventory =
                _context.Inventory
                .Where(x => x.Id == objInventory.Id)
                .FirstOrDefault();
            if (ExistingInventory != null)
            {
                _context.Inventory.Remove(ExistingInventory);
                _context.SaveChanges();
            }
            else
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }
    }
}
