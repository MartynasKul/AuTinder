using AuTinder.Models;
using AuTinder.Views;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace AuTinder.Controllers
{
    public class PartsController : Controller
    {
        private readonly PartsTechView _partsTechView;
        private readonly List<PartOffer> _optimalPartsList = new List<PartOffer>();

        public PartsController(PartsTechView partsTechView)
        {
            _partsTechView = partsTechView;
        }

        [HttpPost]
        public async Task<IActionResult> GetParts(string make, string model)
        {
            // Get parts from the PartsTechView
            var partOffers = await _partsTechView.MakeApiRequestForParts(make, model);

            bool good = Validate();
            if (good)
            {

                // Count the number of unique categories
                int categoryCount = CountNumberOfCategories(partOffers);

                // Iterate over each category
                for (int i = 0; i < categoryCount; i++)
                {
                    var categoryParts = GetPartsByCategory(partOffers, i + 1);
                    int partsInCategoryCount = CountNumberOfPartsInCategory(categoryParts);

                    for (int j = 0; j < partsInCategoryCount; j++)
                    {
                        ComparePricesOfPartsInCategory(categoryParts);
                        PartOffer lowestPricePart = categoryParts[0];
                        UpdatePriceAndSupplier(lowestPricePart);
                        SaveLowestPricePartToOptimalPartsList(lowestPricePart);
                    }
                }


                // Return the partial view with parts data
                return PartialView("~/Views/Route/_PartsListPartial.cshtml", _optimalPartsList);
            }

            else
            {
                // Return an error partial view
                return PartialView("~/Views/Route/_ErrorPartial.cshtml", "Error retrieving parts.");
            }

        }

        private bool Validate()
        {
            return true;
        }

        private int CountNumberOfCategories(List<PartOffer> partOffers)
        {
            return partOffers.Select(p => p.CategoryID).Distinct().Count();
        }

        private List<PartOffer> GetPartsByCategory(List<PartOffer> partOffers, int categoryId)
        {
            return partOffers.Where(p => p.CategoryID == categoryId).ToList();
        }

        private int CountNumberOfPartsInCategory(List<PartOffer> categoryParts)
        {
            return categoryParts.Count;
        }

        private void ComparePricesOfPartsInCategory(List<PartOffer> categoryParts)
        {
            categoryParts.Sort((x, y) => x.Price.CompareTo(y.Price));
        }

        private void UpdatePriceAndSupplier(PartOffer part)
        {
            var lowestPricePart = part;
            part.Price = lowestPricePart.Price;
            part.Supplier = lowestPricePart.Supplier;
        }

        private void SaveLowestPricePartToOptimalPartsList(PartOffer part)
        {
            if (_optimalPartsList.All(p => p.PartName != part.PartName))
            {
                _optimalPartsList.Add(part);
            }
        }
    }
}
