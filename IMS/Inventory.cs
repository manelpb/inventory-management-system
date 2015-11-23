using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS
{
    class Inventory
    {
        private string name;
        private int code;
        private int rate;
        private int stock;
        private int lead;

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public int Code
        {
            get
            {
                return code;
            }

            set
            {
                code = value;
            }
        }

        public int Rate
        {
            get
            {
                return rate;
            }

            set
            {
                rate = value;
            }
        }

        public int Stock
        {
            get
            {
                return stock;
            }

            set
            {
                stock = value;
            }
        }

        public int Lead
        {
            get
            {
                return lead;
            }

            set
            {
                lead = value;
            }
        }

        public Inventory(string name, int code, int rate, int stock, int lead)
        {
            this.name = name;
            this.code = code;
            this.rate = rate;
            this.stock = stock;
            this.lead = lead;
        }
    }
}
