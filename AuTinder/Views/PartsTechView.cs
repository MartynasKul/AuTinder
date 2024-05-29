using AuTinder.Models;

namespace AuTinder.Views
{
    public class PartsTechView
    {
        private readonly List<PartOffer> _partsList = new List<PartOffer>
    {
        new PartOffer { PartName = "Brake Disc", Price = 60, Supplier = "Speedway Motors", CategoryID = 1, Make = "test", Model = "test"},
        new PartOffer { PartName = "Brake Disc2", Price = 55, Supplier = "Speedway Motors", CategoryID = 1, Make = "test", Model = "test"},
        new PartOffer { PartName = "Fuel Filter", Price = 20, Supplier = "Speedway Motors", CategoryID = 2, Make = "test", Model = "test"},
        new PartOffer { PartName = "Fuel Filter2", Price = 18, Supplier = "Speedway Motors", CategoryID = 2, Make = "test", Model = "test"},
        new PartOffer { PartName = "Spark Plug", Price = 5, Supplier = "Speedway Motors", CategoryID = 3, Make = "test", Model = "test"},
        new PartOffer { PartName = "Spark Plug2", Price = 6, Supplier = "Speedway Motors", CategoryID = 3, Make = "test", Model = "test"},
        new PartOffer { PartName = "Radiator", Price = 100, Supplier = "Rock Auto", CategoryID = 1, Make = "Honda", Model = "Civic"},
        new PartOffer { PartName = "Radiator2", Price = 95, Supplier = "Rock Auto", CategoryID = 1, Make = "Honda", Model = "Civic"},
        new PartOffer { PartName = "Alternator", Price = 150, Supplier = "Rock Auto", CategoryID = 2, Make = "Honda", Model = "Civic"},
        new PartOffer { PartName = "Alternator2", Price = 140, Supplier = "Rock Auto", CategoryID = 2, Make = "Honda", Model = "Civic"},
        new PartOffer { PartName = "Starter Motor", Price = 120, Supplier = "Speedway Motors", CategoryID = 4, Make = "test", Model = "test"},
        new PartOffer { PartName = "Starter Motor2", Price = 115, Supplier = "Speedway Motors", CategoryID = 4, Make = "test", Model = "test"},
        new PartOffer { PartName = "Clutch Kit", Price = 200, Supplier = "Speedway Motors", CategoryID = 5, Make = "test", Model = "test"},
        new PartOffer { PartName = "Clutch Kit2", Price = 190, Supplier = "Speedway Motors", CategoryID = 5, Make = "test", Model = "test"},
        new PartOffer { PartName = "Shock Absorber", Price = 75, Supplier = "Rock Auto", CategoryID = 3, Make = "Honda", Model = "Civic"},
        new PartOffer { PartName = "Shock Absorber2", Price = 70, Supplier = "Rock Auto", CategoryID = 3, Make = "Honda", Model = "Civic"},
        new PartOffer { PartName = "Timing Belt", Price = 35, Supplier = "Rock Auto", CategoryID = 4, Make = "Honda", Model = "Civic"},
        new PartOffer { PartName = "Timing Belt2", Price = 40, Supplier = "Rock Auto", CategoryID = 4, Make = "Honda", Model = "Civic"},
        new PartOffer { PartName = "Water Pump", Price = 85, Supplier = "Speedway Motors", CategoryID = 6, Make = "test", Model = "test"},
        new PartOffer { PartName = "Water Pump2", Price = 80, Supplier = "Speedway Motors", CategoryID = 6, Make = "test", Model = "test"},
        new PartOffer { PartName = "Headlight", Price = 50, Supplier = "Speedway Motors", CategoryID = 7, Make = "test", Model = "test"},
        new PartOffer { PartName = "Headlight2", Price = 45, Supplier = "Speedway Motors", CategoryID = 7, Make = "test", Model = "test"},
        new PartOffer { PartName = "Tail Light", Price = 35, Supplier = "Speedway Motors", CategoryID = 8, Make = "test", Model = "test"},
        new PartOffer { PartName = "Tail Light2", Price = 45, Supplier = "Speedway Motors", CategoryID = 8, Make = "test", Model = "test"},
        new PartOffer { PartName = "Headlight", Price = 35, Supplier = "Auto Parts", CategoryID = 1, Make = "pref1", Model = "pref1"},
        new PartOffer { PartName = "Turbine", Price = 45, Supplier = "Auto Parts", CategoryID = 2, Make = "pref1", Model = "pref1"},
        new PartOffer { PartName = "Brake disc", Price = 15, Supplier = "Auto Parts", CategoryID = 3, Make = "pref1", Model = "pref1"},
        new PartOffer { PartName = "Fuel filter", Price = 20, Supplier = "Auto Parts", CategoryID = 4, Make = "pref1", Model = "pref1"},

        new PartOffer { PartName = "Hydraulic oil", Price = 20, Supplier = "Tractor Parts", CategoryID = 1, Make = "Tractor", Model = "Tractor"},
        new PartOffer { PartName = "Hydraulic oil2", Price = 30, Supplier = "Tractor Parts", CategoryID = 1, Make = "Tractor", Model = "Tractor"},

        new PartOffer { PartName = "Fuel filter", Price = 50, Supplier = "Auto Parts", CategoryID = 1, Make = "Mazda", Model = "Miata"},
        new PartOffer { PartName = "Oil Filter", Price = 20, Supplier = "Auto Parts", CategoryID = 2, Make = "Mazda", Model = "Miata"},

        new PartOffer { PartName = "Bumper", Price = 10, Supplier = "Auto Parts", CategoryID = 1, Make = "Mazda", Model = "Leon"},
        new PartOffer { PartName = "Brake pump", Price = 25, Supplier = "Auto Parts", CategoryID = 2, Make = "Mazda", Model = "Leon"},
        new PartOffer { PartName = "Brake pump", Price = 50, Supplier = "Auto Parts", CategoryID = 2, Make = "Mazda", Model = "Leon"},

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
