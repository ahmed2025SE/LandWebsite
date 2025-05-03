namespace LandWebsite.Data.Entites
{
    public class Owner
    {
        public Guid ownerId { get; set; }
        public string ownerName { get; set; }
        public string email { get; set; }
        public int phone{ get; set; }

        public List<Land> Lands { get; set; } = [];
 
    }
}
