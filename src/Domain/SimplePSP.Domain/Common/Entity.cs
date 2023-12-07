namespace SimplePSP.Domain.Common
{
    public abstract class Entity
    {
        public string Id { get; set; }

        protected Entity(string id)
        {
            Id = id;
        }
    }
}