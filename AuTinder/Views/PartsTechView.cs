using AuTinder.Models;

namespace AuTinder.Views
{
    public class PartsTechView
    {
        private readonly List<PartOffer> _partsList = new List<PartOffer>
    {
        new PartOffer { PartName = "Brake Pad", Price = 50, Supplier = "Speedway Motors", CategoryID = 1, Make = "test", Model = "test"},
        new PartOffer { PartName = "Brake Pad2", Price = 45, Supplier = "Speedway Motors", CategoryID = 1, Make = "test", Model = "test"},
        new PartOffer { PartName = "Oil Filter", Price = 10, Supplier = "Speedway Motors", CategoryID = 2, Make = "test", Model = "test"},
        new PartOffer { PartName = "Oil Filter2", Price = 15, Supplier = "Speedway Motors", CategoryID = 2, Make = "test", Model = "test"},
        new PartOffer { PartName = "Oil Filter3", Price = 7, Supplier = "Speedway Motors", CategoryID = 2, Make = "test", Model = "test"},
        new PartOffer { PartName = "Air Filter", Price = 15, Supplier = "Rock Auto", CategoryID = 1, Make = "Honda", Model = "Civic"},
        new PartOffer { PartName = "Air Filter2", Price = 7, Supplier = "Rock Auto", CategoryID = 1, Make = "Honda", Model = "Civic"},
        new PartOffer { PartName = "Engine", Price = 50, Supplier = "Rock Auto", CategoryID = 2, Make = "Honda", Model = "Civic"},
    };

        public async Task<List<PartOffer>> MakeApiRequestForParts(string make, string model)
        {
            // Simulate an asynchronous operation
            await Task.Delay(100);

            // Filter the parts list based on the provided make, model, and year
            var filteredParts = _partsList
                .Where(p => p.Make.Equals(make, StringComparison.OrdinalIgnoreCase) &&
                            p.Model.Equals(model, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return filteredParts;
        }
    }
}
