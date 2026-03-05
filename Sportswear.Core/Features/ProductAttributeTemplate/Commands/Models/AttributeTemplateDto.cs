using Sportswear.DataAccess.Enums;

namespace Sportswear.Core.Features.ProductAttributeTemplate.Commands.Models
{
    public class AttributeTemplateDto
    {
        public string KeyEn { get; set; }
        public string KeyAr { get; set; }
        public AttributeType Type { get; set; }
    }
}
