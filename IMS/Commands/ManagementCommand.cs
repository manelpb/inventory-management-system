using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IMS.Commands
{
    class ManagementCommand
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

        public ManagementCommand()
        {

        }

        /*
        * print help menu
        */
        public void Help()
        {
            Console.WriteLine("Management Commands: ");
            Console.WriteLine("{0,-10}{1,-10}{2,-10}{3,-10}", "Cmd", "Command", "Params", "Description");
            Console.WriteLine("{0,-10}{1,-10}{2,-10}{3,-10}", "~~~", "~~~~~~~", "~~~~~~", "~~~~~~~~~~~");

            Console.WriteLine("{0,-10}{1,-10}{2,-10}{3,-10}", "?", "help", null, "Print this help menu");
            Console.WriteLine("{0,-10}{1,-10}{2,-10}{3,-10}", "lf", "lstfiles", null, "List the files in working directory");
            Console.WriteLine("{0,-10}{1,-10}{2,-10}{3,-10}", "ldb", "loaddb", "fname", "Loads the database file");
            Console.WriteLine("{0,-10}{1,-10}{2,-10}{3,-10}", "sdb", "savedb", "fname", "Saves current database in file");
            Console.WriteLine("{0,-10}{1,-10}{2,-10}{3,-10}", "q", "quit", null, "Ends the program and returns");

            Console.WriteLine();
        }

        /*
        * list files in a directory 
        */
        public void ListFiles(string directory)
        {
            Console.WriteLine("Files in directory {0}", directory);

            DirectoryInfo dir = new DirectoryInfo(directory);
            FileInfo[] files = dir.GetFiles();

            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine(files[i].Name);
            }
        }

        /*
        * load the database based on a filename
        */
        public void LoadDb(string fname)
        {
            try
            {
                Console.WriteLine("Loading File {0}", fname);

                StreamReader sr = new StreamReader(fname);
                int count = Convert.ToInt16(sr.ReadLine());
                inventories = new Inventory[count];
                
                for(int i = 0; i < count; i++)
                {
                    string[] line = sr.ReadLine().Split(',');
                    string name = line[0];
                    int code = Convert.ToInt32(line[1]);
                    int rate = Convert.ToInt32(line[2]);
                    int stock = Convert.ToInt32(line[3]);
                    int lead = Convert.ToInt32(line[4]);

                    inventories[i] = new Inventory(name, code, rate, stock, lead);
                }

                sr.Close();

                Console.WriteLine("{0} records loaded.", count);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Could not open file: ", exc.Message);
            }
        }

        /*
        * save the database based on a file name
        */
        public void SaveDb(string fname)
        {
            try
            {
                Console.WriteLine("Saving File {0}", fname);

                StreamWriter sw = new StreamWriter(fname);
                sw.WriteLine(inventories.Length);

                for (int i = 0; i < inventories.Length; i++)
                {
                    Inventory inventory = inventories[i];

                    sw.WriteLine("{0},{1},{2},{3},{4}", inventory.Name, inventory.Code, inventory.Rate, inventory.Stock, inventory.Lead);
                }

                sw.Close();

                Console.WriteLine("{0} records saved.", inventories.Length);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Could not save file: ", exc.Message);
            }
        }

        /*
        * show an exit message
        */
        public void Quit()
        {
            Console.WriteLine("Thank you for using IMS - Inventory Management System");
        }
    }
}
