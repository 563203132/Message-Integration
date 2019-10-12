using System;

namespace Message.Integration.Common.Helper
{
    public class RandomCodeHelper
    {
        public static string Generate()
        {
            var generator = new Random();

            return generator.Next(0, 999999).ToString("D6");
        }
    }
}
