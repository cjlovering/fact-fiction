namespace FactOrFictionUrlSuggestions
{
    public class FinderFactory
    {
        private string SubscriptionKey;
        public FinderFactory(string SubscriptionKey)
        {
            this.SubscriptionKey = SubscriptionKey;
        }
        public IFinder CreateFinder()
        {
            return new BingV7Finder(SubscriptionKey);
        }
    }
}
