namespace AuTinder.Models
{
    public class MainViewModel
    {
        public List<Ad> Ads { get; set; }
        public List<Delivery> Deliveries { get; set; }

        #region Constructors
        public MainViewModel() { }

        public MainViewModel(List<Ad> ads)
        {
            Ads = ads;
        }

        public MainViewModel(List<Delivery> deliveries)
        {
            Deliveries = deliveries;
        }

        public MainViewModel(List<Ad> ads, List<Delivery> deliveries)
        {
            Ads = ads;
            Deliveries = deliveries;
        }
        #endregion
    }
}
