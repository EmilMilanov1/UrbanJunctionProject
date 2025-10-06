namespace UrbanJunction.Web.Models.Topics
{
    public class AllHousesViewModel
    {
        public IEnumerable<HouseDetailsViewModel> Topics { get; set; }
            = new List<HouseDetailsViewModel>();
    }
}
