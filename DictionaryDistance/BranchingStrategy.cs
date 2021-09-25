using System.Collections.Generic;
using System.Linq;

namespace DictionaryDistance
{
    internal class BranchingStrategy : IStrategy
    {
        private readonly List<Word> sourceBranch;
        private readonly List<Word> destinationBranch;
        private int depth;
        private readonly IReadOnlyCollection<string> words;

        public BranchingStrategy(IReadOnlyCollection<string> words, string source, string destination)
        {
            this.words = words;
            sourceBranch = new List<Word> { new() { Value = source, Depth = 0 } };
            destinationBranch = new List<Word> { new() { Value = destination, Depth = 0 } };
            depth = 0;
        }

        public DistanceResult FindDistance()
        {
            while (true)
            {
                //try to find a match between the two branches
                var tryMatch = sourceBranch.Where(s => s.Depth == depth)
                    .Join(destinationBranch.Where(d => d.Depth >= depth - 1), s=> s, d => d, (s, d) => (s, d))
                    .OrderBy((s)=> s.d.Depth)
                    .FirstOrDefault();

                if (tryMatch.d != null)
                {
                    return new DistanceResult(depth + tryMatch.d.Depth, GetRoots(tryMatch.s).Reverse().Concat(GetRoots(tryMatch.d)).Distinct());
                }

                // Add a new layer on to each branch
                var foundNewSourceWords = NewLayer(words, sourceBranch, depth);
                var foundNewDestWords = NewLayer(words, destinationBranch, depth);

                if (!foundNewSourceWords || !foundNewDestWords)
                {
                    return null;
                }

                depth++;
            }
        }

        private static IEnumerable<string> GetRoots(Word word)
        {
            var output = new List<string> {word.Value};

            if (word.Root != null)
            {
                output.AddRange(GetRoots(word.Root));
            }

            return output;
        }

        private static bool NewLayer(IReadOnlyCollection<string> allWords, List<Word> branch, int depth)
        {
            var foundWords = FindNewWords(allWords, branch.Where(s => s.Depth == depth)).ToList();
            var newWords = foundWords.Where(f => branch.Select(w => w.Value).All(_ => _ != f.Word)).ToList();
            branch.AddRange(newWords.Select(w => new Word { Value = w.Word, Depth = depth + 1, Root = w.Root}));
            return newWords.Any();
        }

        private static IEnumerable<NewWordResult> FindNewWords(IReadOnlyCollection<string> allWords, IEnumerable<Word> wordsToIterate)
        {
            foreach (var word in wordsToIterate)
            {
                for (var i = 0; i < word.Value.Length; i++)
                {
                    // loop through all the chars of the word to find words in the dict that match without the ith char
                    var toFind = word.Value.Remove(i, 1);

                    foreach (var match in allWords.Where(w => w.Remove(i, 1) == toFind))
                    {
                        yield return new NewWordResult(match, word);
                    }
                }
            }
        }
    }
}