namespace DictionaryDistance
{
    class Word
    {
        public string Value { get; init; }

        public int Depth { get; init; }

        public Word Root { get; set; }

        public override string ToString() => $"{Depth}:{Value}";

        protected bool Equals(Word other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Word) obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(Word left, Word right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Word left, Word right)
        {
            return !Equals(left, right);
        }
    }
}