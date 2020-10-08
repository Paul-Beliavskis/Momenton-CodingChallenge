namespace Momenton.CodingChallenge.Extensions
{
    public static class StringExtentions
    {
        public static string AddCompanyLevelSpaces(this string row, int companyLevel)
        {
            for (var i = 1; i < companyLevel; i++)
            {
                row += "   ";
            }

            return row;
        }
    }
}
