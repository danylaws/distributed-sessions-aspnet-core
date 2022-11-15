namespace DistributedSessions
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }

        public Book(int id, string title, double price)
        {
            Id = id;
            Title = title;
            Price = price;
        }
    }
}