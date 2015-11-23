using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Commands;

namespace IMS
{
    class InventorySystem
    {
        private InventoryCommand inventoryCmd;
        private ManagementCommand managementCmd;
        private string[] parameters;
        
        /*
        * start the program
        */
        public void Run()
        {
            // instance program classes
            inventoryCmd = new InventoryCommand();
            managementCmd = new ManagementCommand();

            // print welcome msgs and request commands
            PrintWelcome();
            RequestCommand();
        }

        /*
        * print welcome messsage
        */
        private void PrintWelcome()
        {
            Console.WriteLine("Welcome to IMS - Inventory Management System");
            Console.WriteLine("Type ? for help");
        }

        /*
        * execute commands according with user 
        */
        private void RequestCommand()
        {
            Console.Write("cmd> ");
            parameters = Console.ReadLine().ToLower().Split(' ');

            bool quit = false;

            // send the inventories array from management to inventory class
            inventoryCmd.Inventories = managementCmd.Inventories;
            
            // perform each command based on user input
            switch (parameters[0])
            {
                case "lf":
                case "lstfiles":
                    managementCmd.ListFiles(System.IO.Directory.GetCurrentDirectory()); //AppDomain.CurrentDomain.BaseDirectory
                    break;
                case "ldb":
                case "loaddb":
                    if (IsValidPameter(1))
                    {
                        managementCmd.LoadDb(parameters[1]);
                        inventoryCmd.Inventories = managementCmd.Inventories;
                    }
                    break;
                case "sdb":
                case "savedb":
                    if (IsValidPameter(1))
                    {
                        managementCmd.SaveDb(parameters[1]);
                    }
                    break;
                case "li":
                case "listitems":
                    inventoryCmd.ListItems();
                    break;
                case "+":
                case "inc":
                    if (IsValidPameter(1) && IsValidPameter(2))
                    {
                        inventoryCmd.IncreaseStock(parameters[1], parameters[2]);
                    }
                    break;
                case "-":
                case "dec":
                    if (IsValidPameter(1) && IsValidPameter(2))
                    {
                        inventoryCmd.DecreaseStock(parameters[1], parameters[2]);
                    }
                    break;
                case "lw":
                case "low":
                    inventoryCmd.LowItems();
                    break;
                case "pk":
                case "prek":
                    if (IsValidPameter(1))
                    {
                        inventoryCmd.PreOrderQnt(parameters[1]);
                    }
                    break;
                case "q":
                case "quit":
                    managementCmd.Quit();
                    quit = true;
                    break;
                case "?":
                case "help":
                    inventoryCmd.Help();
                    managementCmd.Help();
                    break;
                default:
                    Console.WriteLine("Command {0} is not recongnized. Please try again.", parameters[0]);
                    break;
            }

            // send back inventory array from inventory class to management class
            managementCmd.Inventories = inventoryCmd.Inventories;

            if (!quit)
            {
                RequestCommand();
            }
        }
        
        /*
        * check if a parameter is valid
        */
        private bool IsValidPameter(int paramNumber)
        {
            try
            {
                if (parameters[paramNumber] != "")
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Invalid no of arguments. Index was outside the bounds of the array");
                }
            } catch
            {
                Console.WriteLine("Invalid no of arguments. Index was outside the bounds of the array");
            }

            return false;
        }
    }
}
