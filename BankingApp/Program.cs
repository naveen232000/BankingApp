using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp
{
   // Data Source = LAPTOP - SG7H0DO8; Initial Catalog = Banking; Integrated Security = True; Encrypt=False
    public  class Program
    {
        static void Main(string[] args)
        {
            Banking banking = new Banking();
            while (true)
            {
                Console.WriteLine("\nEnter your Banking\n1)Add New Account\n2)Deposit Ammount\n3)Withdral Amount\n4)Exit");
                int n =int.Parse(Console.ReadLine());
                switch(n)
                {
                    case 1:
                        {
                            banking.AddAccount(); break;
                        }
                        case 2:
                        {
                            banking.Deposit();  break;
                        }
                        case 3:
                        {
                            banking.Withdral(); break;
                        }
                        case 4:
                        {
                            Environment.Exit(0);
                            break;
                        }
                    default: { Console.WriteLine("Enter Correct Choice"); break; }
                }

            }
     
        }
    }
}
