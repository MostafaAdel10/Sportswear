using Sportswear.Service.Abstract;

namespace Sportswear.Service.Implementations
{
    public class SkuGeneratorService : ISkuGeneratorService
    {
        public string Generate(string productCode, List<string> attributeValues)
        {
            var cleanCode = productCode
                .ToUpper()
                .Replace(" ", "-");

            var parts = attributeValues.Select(v =>
                v.ToUpper()
                 .Replace(" ", "")
                 .Substring(0, Math.Min(v.Length, 5)));

            return $"{cleanCode}-{string.Join("-", parts)}";
        }
    }
}
