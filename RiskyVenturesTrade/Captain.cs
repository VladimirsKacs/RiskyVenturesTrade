namespace RiskyVenturesTrade
{
    internal class Captain
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int Xp { get; set; }
        public CaptainSpec CaptainSpec { get; set; }
    }

    public enum CaptainSpec
    {
        Trade = 1,
        Exploration = 2,
    }

    public static class CaptainExpertise
    {
        public static Dictionary<int, int> Exp { get; } =
            new()
            {
                { 0, 0 },
                { 1, 10 },
                { 2, 30 },
                { 3, 60 },
                { 4, 100 },
                { 5, 150 },
                { 6, 999999 }
            };
        public static Dictionary<int, double> ProblemAversion { get; } =
            new()
            {
                {0, 1.25},
                { 1, 1 },
                { 2, 0.9 },
                { 3, 0.8 },
                { 4, 0.7 },
                { 5, 0.6 },
                { 6, 0.5 }
            };
    }
}
