using System.Text;

namespace Demo.Library.Algorithms.Strings
{
    public static partial class ComparisonMetrics
    {
        public static string LongestCommonSubstring(this string source, string target)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(target)) { return null; }

            int[,] l = new int[source.Length, target.Length];
            int maximumLength = 0;
            int lastSubsBegin = 0;
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < source.Length; i++)
            {
                for (int j = 0; j < target.Length; j++)
                {
                    if (source[i] != target[j])
                    {
                        l[i, j] = 0;
                    }
                    else
                    {
                        if ((i == 0) || (j == 0))
                            l[i, j] = 1;
                        else
                            l[i, j] = 1 + l[i - 1, j - 1];

                        if (l[i, j] > maximumLength)
                        {
                            maximumLength = l[i, j];
                            int thisSubsBegin = i - l[i, j] + 1;
                            if (lastSubsBegin == thisSubsBegin)
                            {//if the current LCS is the same as the last time this block ran
                                stringBuilder.Append(source[i]);
                            }
                            else //this block resets the string builder if a different LCS is found
                            {
                                lastSubsBegin = thisSubsBegin;
                                stringBuilder.Length = 0; //clear it
                                stringBuilder.Append(source.Substring(lastSubsBegin, (i + 1) - lastSubsBegin));
                            }
                        }
                    }
                }
            }

            return stringBuilder.ToString();
        }

    }
}
