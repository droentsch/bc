using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bc
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!checkArgs(args)) return;

            string expressionToParse = args[0];
            double result = 0.0;
            
            var calc = new Calculatrix();
            try
            {
                result = calc.Calculate(expressionToParse);
            }
            catch {
                Console.WriteLine("bc didn't like that expression for some reason.");
                return;
            }
            Console.WriteLine(result);
        }
        #region private methods
        private static bool checkArgs(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("The argument to bc must consist of one and only one string.  Did you forget quotation marks?");
                return false;
            }
            if (args[0].Contains(')') || args[0].Contains('('))
            {
                Console.WriteLine("Sorry, this version of bc doesn't like parens.");
                return false;
            }
            return true;
        }
        #endregion
    }
}
