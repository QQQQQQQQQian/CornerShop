using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CornerShop.Models
{
    
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }

        public decimal Price { get; set; }

        [BsonRepresentation(BsonType.String)]
        public ProductCategory Category { get; set; }
        public decimal Stock { get; set; }
        [BsonIgnore]
        public string Unit =>
        Category == ProductCategory.Beverage ? "pcs" : "Kg";
       



    }

}
