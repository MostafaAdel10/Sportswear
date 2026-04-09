using Sportswear.Service.Abstract;

namespace Sportswear.Service.Implementations
{
    public class SkuGeneratorService : ISkuGeneratorService
    {
        public string Generate(string productCode, List<string> attributeValues)
        {
            if (!attributeValues.Any())
                return productCode.ToUpper().Replace(" ", "-");

            var parts = attributeValues
                .Where(v => !string.IsNullOrEmpty(v))
                .Select(v => v.ToUpper().Replace(" ", "").Substring(0, Math.Min(v.Length, 5)));

            return $"{productCode.ToUpper().Replace(" ", "-")}-{string.Join("-", parts)}";
        }
        // مثال: "NIKE-TSHIRT" + ["XL", "Red"] → "NIKE-TSHIRT-XL-RED"
    }
}
