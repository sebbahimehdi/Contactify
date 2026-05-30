namespace ContactManagerPro.Models
{
    public class ContactIndexViewModel
    {
        public List<Contact> Contacts { get; set; } = new List<Contact>();
        public string SearchString { get; set; }
        public string SearchField { get; set; } = "all";
        public bool IsFavoritesFilter { get; set; }
        public string SortBy { get; set; } = "name";
        public int TotalContacts { get; set; }
        public int FavoriteCount { get; set; }
    }
}
