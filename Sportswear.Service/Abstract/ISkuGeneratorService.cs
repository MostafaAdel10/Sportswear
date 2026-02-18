namespace Sportswear.Service.Abstract
{
    public interface ISkuGeneratorService
    {
        public string Generate(string productCode, string colorName, string size);
    }
}
