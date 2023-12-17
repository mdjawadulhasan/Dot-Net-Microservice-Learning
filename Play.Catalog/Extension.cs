using Play.Catalog.Entities;
using static Play.Catalog.DTOs.DTOs;

namespace Play.Catalog
{
    public static class Extension
    {
        public static ItemDto AsDto(this Item item)
        {
            return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreateDate);
        }
    }
}
