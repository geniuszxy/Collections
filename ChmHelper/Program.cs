using System.Text;
using ChmHelper;

// Check input

if (args.Length != 1)
{
    Console.WriteLine("""
        Usage

            ChmHelper <Folder>
        
        """);
    return;
}

var inputFolder = args[0];
if(!Directory.Exists(inputFolder))
{
    Console.WriteLine($"Input folder \"{inputFolder}\" doesn't exist");
    return;
}

// Encoding
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

// Process
var helper = new HhpHelper(inputFolder);
helper.Process();