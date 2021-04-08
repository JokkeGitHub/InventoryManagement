using InventoryManagementDB.Data.InventoryManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace InventoryManagement.Pages
{
    public partial class FetchInventory
    {
        // AuthenticationState is available as a CascadingParameter
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        List<Inventory> items;
        protected override async Task OnInitializedAsync()
        {
            // Get the current user
            var user = (await authenticationStateTask).User;
            // Get the forecasts for the current user
            // We access WeatherForecastService using @Service
            items = await Service.GetItemAsync(user.Identity.Name);
            //@Service.GetItemAsync(user.Identity.Name);
        }
        Inventory objInventory = new Inventory();
        bool ShowPopup = false;
        void ClosePopup()
        {
            // Close the Popup
            ShowPopup = false;
        }
        void AddNewItem()
        {
            // Make new forecast
            objInventory = new Inventory();
            // Set Id to 0 so we know it is a new record
            objInventory.Id = 0;
            // Open the Popup
            ShowPopup = true;
        }
        async Task SaveItem()
        {
            // Close the Popup
            ShowPopup = false;
            // Get the current user
            var user = (await authenticationStateTask).User;
            // A new forecast will have the Id set to 0
            if (objInventory.Id == 0)
            {
                // Create new forecast
                Inventory objNewInventory = new Inventory();
                objNewInventory.Object = objInventory.Object;
                objNewInventory.Location = objInventory.Location;
                objNewInventory.Amount = Convert.ToInt32(objInventory.Amount);
                objNewInventory.Comment = objInventory.Comment;
                objNewInventory.Date = System.DateTime.Now;
                objNewInventory.UserName = user.Identity.Name;
                // Save the result
                var result =
                @Service.CreateItemAsync(objNewInventory);
            }
            else
            {
                // This is an update
                var result =
                @Service.UpdateItemAsync(objInventory);
            }
            // Get the forecasts for the current user
            items =
            await @Service.GetItemAsync(user.Identity.Name);
        }
        void EditItem(Inventory Inventory)
        {
            // Set the selected forecast
            // as the current forecast
            objInventory = Inventory;
            // Open the Popup
            ShowPopup = true;
        }
        async Task DeleteItem()
        {
            // Close the Popup
            ShowPopup = false;
            // Get the current user
            var user = (await authenticationStateTask).User;
            // Delete the forecast
            var result = @Service.DeleteItemAsync(objInventory);
            // Get the forecasts for the current user
            items =
            await @Service.GetItemAsync(user.Identity.Name);
        }
    }
}
