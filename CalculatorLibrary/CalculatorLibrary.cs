// OK! Create a functionality that will count the amount of times the calculator was used.
// Ok! Store a list with the latest calculations. And give the users the ability to delete that list.
// OK! Allow the users to use the Results in the list above to perform new calculations.
// OK! Add extra calculations: Square Root, Taking the Power, 10x, Trigonometry functions.

using System.Text.Json;
using System.IO;

namespace CalculatorLibrary
{
    public class Calculator
    {
        const string logPath = "../../../calculatorlog.json";
        private CalculatorLog log;

        public Calculator()
        {
            string jsonString = File.Exists(logPath) ? File.ReadAllText(logPath) : "{}";
            log = JsonSerializer.Deserialize<CalculatorLog>(jsonString) ?? new CalculatorLog();
            log.OpenedCount++;
            WriteLog();
        }

        public int getOpenedCount()
        {
            return log.OpenedCount;
        }

        public List<Operation> GetHistory()
        {
            return log.History;
        }

        void WriteLog()
        {
            int len = log.History.Count;
            // Only write the last 5 elements, both to avoid cluttering the screen and logs too large
            if (len > 5)
                log.History = log.History.GetRange(len - 5, 5);
            File.WriteAllText(logPath, JsonSerializer.Serialize<CalculatorLog>(log));
        }

        public double DoOperation(Operation operand)
        {
            operand.Result = double.NaN;
            log.History.Add(operand);

            // Use a switch statement to do the math.
            switch (operand.Operand)
            {
                case "a":
                    operand.Result = operand.Num1 + operand.Num2;
                    operand.Operand = "Add";
                    break;
                case "s":
                    operand.Result = operand.Num1 - operand.Num2;
                    operand.Operand = "Subtract";
                    break;
                case "m":
                    operand.Result = operand.Num1 * operand.Num2;
                    operand.Operand = "Multiply";
                    break;
                case "d":
                    // Ask the user to enter a non-zero divisor.
                    if (operand.Num2 != 0)
                    {
                        operand.Result = operand.Num1 / operand.Num2;
                        operand.Operand = "Divide";
                    }
                    else
                    {
                        log.History.RemoveAt(log.History.Count - 1); // remove from History
                    }
                    break;
                case "p":
                    operand.Result = Math.Pow(operand.Num1, operand.Num2);
                    if(double.IsNaN( operand.Result))
                    {
                        log.History.RemoveAt(log.History.Count - 1);
                    }
                    operand.Operand = "Power";
                    break;
                case "r":
                    if (operand.Num1 > 0)
                    {
                        operand.Result = Math.Sqrt(operand.Num1);
                        operand.Operand = "Square Root";
                    }
                    else
                    {
                        log.History.RemoveAt(log.History.Count - 1);
                    }
                    break;
                case "c":
                    operand.Result = Math.Cos(operand.Num1);
                    operand.Operand = "Cosine";
                    break;
                case "i":
                    operand.Result = Math.Sin(operand.Num1);
                    operand.Operand = "Sine";
                    break;
                // Return text for an incorrect option entry.
                default:
                    throw new Exception("Invalid Operation.");
            }
            WriteLog();

            return operand.Result;
        }
    }
    public class Operation
    {
        public double Num1 { get; set; }
        public double Num2 { get; set; }
        public string? Operand { get; set; }
        public double Result { get; set; }
    }
    public class CalculatorLog
    {
        public int OpenedCount { get; set; }
        public List<Operation> History { get; set; } = [];
    }
}