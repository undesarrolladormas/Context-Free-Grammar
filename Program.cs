using Cocona;
using Grammar.Commands;

Console.OutputEncoding = System.Text.Encoding.UTF8;
var app = CoconaApp.Create();

app.AddCommand(([Argument(Description = "File path")]string? path )=>{
    Console.WriteLine();
    if (path is null)
    {
        InteractiveMode.Interactive();
    }
    else
    {
        ReadFile.Read(path);
    }
    Console.WriteLine();
});

app.Run();