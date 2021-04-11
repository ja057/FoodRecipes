using System;

namespace FoodRecipes.Models
{
    public class ErrorViewModel
                        
    {                       //Request Id used for View the tables
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
