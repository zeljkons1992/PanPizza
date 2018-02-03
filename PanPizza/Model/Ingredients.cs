using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanPizza.Model
{
    class Ingredients
    {
        private string _name;
        private double _price;
        bool isCheck;

        public bool IsChecked
        {
            get { return isCheck; }
            set { isCheck = value; }
        }

        public Ingredients(string name, double price)
        {
            this._name = name;
            this._price = price;
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public double Price
        {
            get
            {
                return _price;
            }
        }
    }
}
