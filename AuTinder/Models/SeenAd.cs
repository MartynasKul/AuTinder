namespace AuTinder.Models
{
    public class SeenAd
    {
        public int AdId { get; set; }
        public int UserId { get; set; }
        public bool liked { get; set; }

        public Ad ad { get; set; }

        public SeenAd() { }

        public SeenAd(int adid, int userid, bool like) { 
            AdId = adid;
            UserId = userid;
            liked = like;
        }

        public SeenAd(int adid, int userid, bool like, Ad ad)
        {
            AdId = adid;
            UserId = userid;
            liked = like;
            this.ad = ad;
        }
    }
}
