using System.Collections.Generic;

namespace UnitTests
{
    class TestData
    {
        public const int NumberOfCategories = 10;

        public static readonly IDictionary<int, int> BoardsPerCategory = new Dictionary<int, int>
        {
            {6, 4},
            {7, 7},
            {20, 7},
            {9, 2},
            {23, 3},
            {21, 8},
            {12, 18},
            {13, 2},
            {24, 0},
            {26, 2}
        };

        public const int BoardId = 14;
        public const int ThreadId = 214777;

        public const string Username = "";
        public const string Password = "";
    }
}
