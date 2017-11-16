namespace FactOrFictionUrlSuggestions
{
    public static class FinderFactory
    {
        public static IFinder CreateFinder()
        {
            return new BingV5Finder();
        }
    }
}
