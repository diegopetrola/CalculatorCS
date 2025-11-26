using CalculatorLibrary;
using System.Text.RegularExpressions;

namespace CalculatorProgram;

class Program
{
    public Calculator calculator;
    Program()
    {
        calculator = new();
    }

    void printHistory()
    {
        var history = calculator.GetHistory();
        for (int i = 0; i < history.Count; i++)
        {
            Operation op = history.ElementAt(i);
            // Not a big fan of this ngl
            bool showN2 = Regex.IsMatch(op.Operand ?? "", "[Add|Subtract|Power|Multiply|Divide]");
            Console.WriteLine($"[{i}]- {op.Num1} {op.Operand} {(showN2 ? op.Num2 : "")} = {op.Result}");
        }

        Console.WriteLine("\n");
    }

    bool ParseInput(out double value)
    {
        value = 0;
        string input = Console.ReadLine() ?? "";
        if (input.Length == 0) return false;
        if (input[0] == 'h')
        {
            printHistory();
            return false;
        }
        else if (input[0] == 'u')
        {
            if (int.TryParse(input.Substring(1, 1), out int index))
            {
                var history = calculator.GetHistory();
                if (index < history.Count)
                {
                    value = calculator.GetHistory().ElementAt(index).Result;
                    Console.WriteLine($"...used value {value} from history.");
                    return true;
                }
                else return false;
            }
        }
        else if (double.TryParse(input, out value))
        {
            return true;
        }
        else if (input[0] == 'q')
        {
            Environment.Exit(0);
        }
        return false;
    }

    public bool ParseOperation(out string operation)
    {
        operation = Console.ReadLine() ?? "";
        if (operation.Length == 0) return false;

        if (Regex.IsMatch(operation, "[a|s|m|d|p|r|c|i]"))
        {
            return true;
        }
        if (operation[0] == 'q') Environment.Exit(0);
        if (operation[0] == 'h')
            printHistory();


        return false;
    }

    static public void Main(string[] args)
    {
        Program app = new();

        Console.WriteLine("Console Calculator in C#\r");
        Console.WriteLine($"Calculator has been used {app.calculator.getOpenedCount()} times");
        Console.WriteLine("------------------------\n");

        while (true)
        {
            double result = 0;
            Operation operation = new();

            // Ask the user to choose an operator.
            Console.WriteLine("Choose an operator from the following list:");
            Console.WriteLine("\ta - Add");
            Console.WriteLine("\ts - Subtract");
            Console.WriteLine("\tm - Multiply");
            Console.WriteLine("\td - Divide");
            Console.WriteLine("\tp - Power");
            Console.WriteLine("\tr - Square root");
            Console.WriteLine("\tc - Cosine");
            Console.WriteLine("\ti - Sine");
            Console.WriteLine("\th - See history");
            Console.WriteLine("\tq - Quit");
            Console.Write("Your option? ");
            string op = "?";
            while (!app.ParseOperation(out op))
            {
                Console.Write("Please choose an operation: ");
            }

            // Ask the user to type the first number.
            Console.Write("Type a number, type 'h' to see history, type uN tu use index N of history: ");
            double cleanNum1 = 0;
            while (!app.ParseInput(out cleanNum1))
            {
                Console.Write("Please enter an integer value: ");
            }


            double cleanNum2 = 0;
            if (op != null && Regex.IsMatch(op, "[a|s|m|d|p]"))
            {
                // Ask the user to type the second number.
                Console.Write("Type a number, type 'h' to see history, type uN tu use index N of history: ");
                while (!app.ParseInput(out cleanNum2))
                {
                    Console.Write("Please enter a valid option (number, 'h' or 'uN'): ");
                }
            }

            operation.Num1 = cleanNum1;
            operation.Num2 = cleanNum2;
            operation.Operand = op;

            Console.WriteLine("\n------------------------");
            try
            {
                result = app.calculator.DoOperation(operation);
                if (double.IsNaN(result))
                {
                    Console.Write("This operation will result in a mathematical error.\n");
                }
                else Console.Write("Your result: {0:0.##}\n", result);
            }
            catch (Exception e)
            {
                Console.WriteLine("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
            }

            Console.WriteLine("------------------------\n");
            Console.WriteLine("\n"); // Friendly linespacing.
        }
    }
}