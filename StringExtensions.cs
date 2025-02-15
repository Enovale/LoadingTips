using System;

namespace LoadingTips
{
    internal static class StringExtensions
    {
        private static bool IsInputStringShorter(string str, int length)
            => str.Length < length;
        
        public static ReadOnlySpan<char> UseAsSpan(this string str, int start, int length)
        {
            if (IsInputStringShorter(str, length - start))
                return str;
            return str.AsSpan(start, length);
        }
    }
}