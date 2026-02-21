using System;
using System.Collections.Generic;

namespace CardMatch.Infrastructure.Persistence
{
    [Serializable]
    public sealed class SaveDataDTO
    {
        public int Rows;
        public int Columns;
        public int Score;

        public List<CardDTO> Cards = new();
    }

    [Serializable]
    public sealed class CardDTO
    {
        public int Id;
        public int MatchKey;
        public int State;
    }
}