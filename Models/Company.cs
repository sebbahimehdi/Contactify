namespace ContactManagerPro.Models
{
    public class Company
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string Industry { get; set; } = "";

        public string Website { get; set; } = "";

        public string Address { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Contact> Contacts { get; set; }
            = new List<Contact>();
    }
}
