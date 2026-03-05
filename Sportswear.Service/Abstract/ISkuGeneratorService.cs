namespace Sportswear.Service.Abstract
{
    public interface ISkuGeneratorService
    {
        public string Generate(string productCode, List<string> attributeValues);
    }
}
