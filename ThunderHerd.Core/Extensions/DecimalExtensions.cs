namespace ThunderHerd.Core.Extensions
{
    public static class DecimalExtensions
    {
        public static double Round(this double src, int precision = 4)
        {
            return Math.Round(src, precision, MidpointRounding.AwayFromZero);
        }
        public static decimal Round(this decimal src, int precision = 4)
        {
            return Math.Round(src, precision, MidpointRounding.AwayFromZero);
        }
    }
}
