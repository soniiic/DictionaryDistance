namespace DictionaryDistance
{
    internal class NewWordResult
    {
        public string Word { get; }

        public Word Root { get; }

        public NewWordResult(string word, Word root)
        {
            Word = word;
            Root = root;
        }
    }
}