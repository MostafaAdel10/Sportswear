using Sportswear.Service.Abstract;

namespace Sportswear.Service.Implementations
{
    public class SkuGeneratorService : ISkuGeneratorService
    {
        public string Generate(string productCode, string colorName, string size)
        {
            return $"{productCode}-{colorName}-{size}".ToUpper();
        }
    }
}
