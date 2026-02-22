namespace CardMatch.Core.Events
{
    public sealed class ComboChanged
    {
        public int Combo { get; }

        public ComboChanged(int combo)
        {
            Combo = combo;
        }
    }
}