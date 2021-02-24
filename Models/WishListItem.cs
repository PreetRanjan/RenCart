namespace RenCart.API.Models
{
    public class WishListItem
    {
        public long Id { get; set; }
        public string WishListId { get; set; }
        public virtual WishList WishList { get; set; }
        public long BookId { get; set; }
        public virtual Book Book { get; set; }
    }
}