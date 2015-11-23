using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Commands
{
    class InventoryCommand
    {
        private Inventory[] inventories;

        public Inventory[] Inventories
        {
            get
            {
                return inventories;
            }

            set
            {
                inventories = value;
            }
        }

        public InventoryCommand()
        {            
        }

        /*
        * print help menu
        */
        public void Help()
        {
            Console.WriteLine("Inventory Commands: ");
            Console.WriteLine("{0,-10}{1,-10}{2,-10}{3,-10}", "Cmd", "Command", "Params", "Description");
            Console.WriteLine("{0,-10}{1,-10}{2,-10}{3,-10}", "~~~", "~~~~~~~", "~~~~~~", "~~~~~~~~~~~");
            
            Console.WriteLine("{0,-10}{1,-10}{2,-10}{3,-10}", "li", "listitems", null, "List all items in inventory");
            Console.WriteLine("{0,-10}{1,-10}{2,-10}{3,-10}", "+", "inc", "code qty", "Increase an item stock in inventory");
            Console.WriteLine("{0,-10}{1,-10}{2,-10}{3,-10}", "-", "dec", "code qty", "Decrease an item stock in inventory");
            Console.WriteLine("{0,-10}{1,-10}{2,-10}{3,-10}", "lw", "low", null, "List items with low stock level");
            Console.WriteLine("{0,-10}{1,-10}{2,-10}{3,-10}", "pk", "prek", null, "List preorder quantities for k days");

            Console.WriteLine();
        }

        /*
        * list items
        * - only when a database is loaded
        */
        public void ListItems()
        {
            if (DataBaseLoaded())
            {
                Console.WriteLine("Inventory report:");

                PrintHeader();

                for (int i = 0; i < inventories.Length; i++)
                {
                    Inventory inventory = inventories[i];

                    Console.WriteLine("{0,-20}{1,5}{2,5}{3,6}{4,5}", inventory.Name, inventory.Code, inventory.Rate, inventory.Stock, inventory.Lead);
                }
            }
        }

        /*
        * increase a stock of a item by code
        * - only when a database is loaded
        */
        public void IncreaseStock(string codeInput, string qntInput)
        {
            if (DataBaseLoaded())
            {
                try
                {
                    int qnt = Convert.ToInt32(qntInput);
                    int itemIndex = FindItemIndex(Convert.ToInt32(codeInput));

                    if (qnt < 0)
                    {
                        throw new Exception("Amount should be positive. Given " + qnt + " value");
                    }

                    if (itemIndex != -1)
                    {
                        Inventory inventory = inventories[itemIndex];
                        inventory.Stock += qnt;
                        inventories[itemIndex] = inventory;

                        PrintItem(inventory);
                    }
                    else
                    {
                        Console.WriteLine("Code {0} not found.", codeInput);
                    }
                }
                catch (Exception exc)
                {
                    Console.WriteLine("Could not update item. {0}", exc.Message);
                }
            }
        }

        /*
        * decrease a stock of a item by code
        * - only when a database is loaded
        */
        public void DecreaseStock(string codeInput, string qntInput)
        {
            if (DataBaseLoaded())
            {
                try
                {
                    int qnt = Convert.ToInt32(qntInput);
                    int itemIndex = FindItemIndex(Convert.ToInt32(codeInput));

                    if(qnt < 0)
                    {
                        throw new Exception("Amount should be positive. Given " + qnt + " value");
                    }

                    if (itemIndex != -1)
                    {
                        Inventory inventory = inventories[itemIndex];

                        if ((inventory.Stock - qnt) < 0)
                        {
                            inventory.Stock = 0;
                        }
                        else
                        {
                            inventory.Stock -= qnt;
                        }

                        inventories[itemIndex] = inventory;

                        PrintItem(inventory);
                    }
                    else
                    {
                        Console.WriteLine("Code {0} not found.", codeInput);
                    }
                }
                catch (Exception exc)
                {
                    Console.WriteLine("Could not update item. {0}", exc.Message);
                }
            }
        }

        /*
        * show items with lower stock values
        * - only when a database is loaded
        */
        public void LowItems()
        {
            if (DataBaseLoaded())
            {
                bool printed = false;

                for (int i = 0; i < inventories.Length; i++)
                {
                    Inventory inventory = inventories[i];

                    if ((inventory.Rate * inventory.Lead) > inventory.Stock)
                    {
                        if (!printed)
                        {
                            PrintHeader();
                        }

                        printed = true;

                        Console.WriteLine("{0,-20}{1,5}{2,5}{3,6}{4,5}", inventory.Name, inventory.Code, inventory.Rate, inventory.Stock, inventory.Lead);
                    }
                }

                if (!printed)
                {
                    Console.WriteLine("No item is low in stock");
                }
            }
        }

        /*
        * show items that need to be ordered
        * - only when a database is loaded
        */
        public void PreOrderQnt(string kInput)
        {
            if (DataBaseLoaded())
            {
                try
                {
                    int k = Convert.ToInt32(kInput);
                    bool printed = false;

                    if (k < 0)
                    {
                        throw new Exception("Days must be positive. Given " + kInput + " days");
                    }

                    for (int i = 0; i < inventories.Length; i++)
                    {
                        Inventory inventory = inventories[i];

                        int preOrderQnt = 0;

                        if (inventory.Lead > k)
                        {
                            preOrderQnt = inventory.Rate * inventory.Lead - inventory.Stock;
                        }
                        else
                        {
                            preOrderQnt = inventory.Rate * k - inventory.Stock;
                        }

                        if (preOrderQnt > 0)
                        {
                            if (!printed)
                            {
                                Console.WriteLine("Pre order quantities for {0} days", kInput);
                                PrintHeaderExt();
                            }

                            Console.WriteLine("{0,-20}{1,5}{2,5}{3,6}{4,5}{5,6}", inventory.Name, inventory.Code, inventory.Rate, inventory.Stock, inventory.Lead, preOrderQnt);

                            printed = true;
                        }
                    }

                    if (!printed)
                    {
                        Console.WriteLine("No item needs to be preordered for {0} days", kInput);
                    }
                }
                catch (Exception excp)
                {
                    Console.WriteLine("Invalid Days. {0}", excp.Message);
                }
            }
        }

        /*
        * check if the database was loaded
        */
        private bool DataBaseLoaded()
        {
            if (inventories == null || inventories.Length == 0)
            {
                Console.WriteLine("Database not loaded. Please use ldb first");

                return false;
            }

            return true;
        }

        /*
        * find items by code and return its index
        */
        private int FindItemIndex(int itemCode)
        {
            for (int i = 0; i < inventories.Length; i++)
            {
                // find for item
                if (itemCode == inventories[i].Code)
                {
                    return i;
                }
            }

            return -1;
        }

        /*
        * show item based on parameter 
        */
        private void PrintItem(Inventory inventory)
        {
            Console.WriteLine("Updating inventory");

            Console.WriteLine("{0}-{1}", inventory.Code, inventory.Name);
            Console.WriteLine("Stock: {0}", inventory.Stock);
            Console.WriteLine("Consumption-rate: {0}", inventory.Rate);
            Console.WriteLine("Lead-time: {0}", inventory.Lead);
        }

        /*
        * print item columns header
        */
        private void PrintHeader()
        {
            Console.WriteLine("{0,-20}{1,5}{2,5}{3,6}{4,5}", "Name", "Code", "Rate", "Stock", "Lead");
            Console.WriteLine("{0,-20}{1,5}{2,5}{3,6}{4,5}", "~~~~", "~~~~", "~~~~", "~~~~~", "~~~~");
        }

        /*
        * print item columns header for 'PreOrder' operations
        */
        private void PrintHeaderExt()
        {
            Console.WriteLine("{0,-20}{1,5}{2,5}{3,6}{4,5}{5,6}", "Name", "Code", "Rate", "Stock", "Lead", "Units");
            Console.WriteLine("{0,-20}{1,5}{2,5}{3,6}{4,5}{5,6}", "~~~~", "~~~~", "~~~~", "~~~~~", "~~~~", "~~~~~");
        }
    }
}
