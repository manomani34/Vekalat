using Vekalat.Application.Common.InfraServices;
using System;

namespace Vekalat.InfraStructure.Data.Repositories.InfraRepository
{
    public class RandomCodeGenerator : IRandomCodeGenerator
    {
        public string DigitCodeGenerator(int digit)
        {
            var generator = new Random();
            return generator.Next(100, 1000000).ToString($"D{digit}");
        }

        public string GuidGenerator()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
        public string CustomGuidGenerator(int take)
        {
            return Guid.NewGuid().ToString().Replace("-", "").Substring(0, take);
        }
    }
}