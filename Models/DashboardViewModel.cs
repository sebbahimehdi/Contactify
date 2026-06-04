namespace ContactManagerPro.Models
{
    public class DashboardViewModel
    {
        public int TotalContacts { get; set; }
        public int FavoriteContacts { get; set; }
        public int CompaniesCount { get; set; }
        public int CategoriesCount { get; set; }
        public int NotesCount { get; set; }
        public int CitiesCount { get; set; }
        public List<Contact> RecentContacts { get; set; } = new List<Contact>();
    }
}
