using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CornerShop.Models
{
    public class CartItem: INotifyPropertyChanged
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public ProductCategory Category { get; set; }
        public decimal UnitPrice { get; set; }
        private decimal _quantity;

        public decimal Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                    OnPropertyChanged(nameof(TotalPrice));
                }
            }
        }
        [BsonIgnore]
        public string Unit => Category == ProductCategory.Beverage ? "pcs" : "kg";
        [BsonIgnore]
        public decimal TotalPrice => UnitPrice * Quantity;
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
