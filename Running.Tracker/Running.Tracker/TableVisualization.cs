using Spectre.Console;

namespace Running.Tracker
{
    internal class TableVisualization
    {
        internal static void ShowTable<T>(List<T> tableData) where T : class
        {
            Console.WriteLine("\n");

            var table = new Table();

            table.AddColumn("ID");
            table.AddColumn("Date");
            table.AddColumn("Duration");
            table.AddColumn("Miles");
            table.AddColumn("Avg Pace");

            foreach (var item in tableData)
            {
                var idProperty = typeof(T).GetProperty("Id");
                var dateProperty = typeof(T).GetProperty("Date");
                var durationProperty = typeof(T).GetProperty("Duration");
                var milesProperty = typeof(T).GetProperty("Miles");
                

                table.AddRow(
                    idProperty?.GetValue(item)?.ToString() ?? "NULL",
                    dateProperty?.GetValue(item)?.ToString() ?? "NULL",
                    durationProperty?.GetValue(item)?.ToString() ?? "NULL",
                    milesProperty?.GetValue(item)?.ToString() ?? "NULL"
                    );
            }

            table.Border(TableBorder.Square);
            table.BorderColor(Color.Blue);

            AnsiConsole.Write(table);
            Console.WriteLine("\n");
        }
    }
}