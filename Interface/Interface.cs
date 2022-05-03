using ShgitObjects;
using System.Globalization;

string graphsLocation = @"C:\shgit\graphs.txt";

if(!File.Exists(graphsLocation)) { File.Create(graphsLocation); }//for first use on a computer

Client.ConnectToDatabase();

Console.WriteLine("\nCommands:\n" +
    "1. '<graph location> -u': upload changes to server\n" +
    "2.'<graph location> -n': create new graph\n" +
    "3. '<graph name> -d': delete exiting graph\n" +
    "4. 'quit': quit the program\n");

bool running = true;
string input = "";
while (running)
{
    Console.WriteLine("Enter Command:\n");
    input = Console.ReadLine().ToLower();
    if (input == "quit") { running = false; }
    else if (LegalCommand(input))
    {
        string[] Input = input.Split(' ');
        string graph = Input[0];
        string command = Input[1];
        switch (command)
        {
            case "-u":
                UploadGraph(graph, Client.user);
                break;
            case "-n":
                try { CreateGraph(graph, graphsLocation, Client.user); }
                catch (FileNotFoundException e) { Console.WriteLine(e.Message); }
                break;
            case "-d":
                DeleteGraph(graph);
                break;
            case "-ad":
                File.WriteAllText(graphsLocation, String.Empty);
                break;
        }
    }
    else { Console.WriteLine("illegal command"); }
}

Console.WriteLine("all rights reserved to roy gilboa's left testicle");


static void CreateGraph(string graph, string graphsLocation, string name)
{
    string[] graphs = File.ReadAllLines(graphsLocation);

    if (!Directory.Exists(graph)) { throw new FileNotFoundException("file not found. try again dumbass"); }
    else if (graphs.Contains(graph)) { throw new FileNotFoundException("file already a graph. roy gilboa ass motherfucker"); }

    using var writer = new StreamWriter(graphsLocation, append: true);
    writer.WriteLine(graph);  //add graph to list
    writer.Close();

    string shgitPath = graph + @"\.shgit";
    Directory.CreateDirectory(shgitPath); //shgit file for shgit stuff (versions)
    
    UploadGraph(graph, name);

    Console.WriteLine("graph created! " + GetMetaData(name));
}

static string GetMetaData(string name) //Commiter, commit time
{
    return name + "_" + DateTime.Now.ToString(new CultureInfo("de-DE")).Replace(' ', '_').Replace('.', '-').Replace(':', '-');
}

static void UploadGraph(string graph, string name)
{
    string shgitPath = graph + @"\.shgit";
    DirectoryInfo d = new DirectoryInfo(graph);
    FileInfo[] files = d.GetFiles("*", SearchOption.AllDirectories);
    string filePath = "";
    string nodePath = ""; //location of shgit node
    foreach (FileInfo file in files)
    {
        if (!file.FullName.Contains(".shgit"))
        {
            filePath = shgitPath + @"\" + file.Name;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            nodePath = filePath + @"\" + GetMetaData(name) + file.Extension;
            File.Copy(file.FullName, nodePath);
            Client.Commit(file.Name + file.Extension, nodePath);
        }
    }

    Console.WriteLine("Graph uploaded successfully!");
}

static void DeleteGraph(string graph)
{
    Console.WriteLine("Are you sure you want to delete " + graph + "?\nThis cannot be undone [Y/N]");
    string ans = Console.ReadLine().ToUpper();
    if (ans != "Y" || ans!= "N") { Console.WriteLine("Illegal input - either Y (yes) or N (no)"); }
    else 
    {
        if (ans == "Y") 
        {
            Directory.Delete(graph + @"\.shgit");
        }
        else { Console.WriteLine("Word brotha"); }
    }
}

static bool LegalCommand(string input)
{
    if(input.Contains(' '))
    {
        string[] Input = input.Split(' ');
        string graph = Input[0];
        return Directory.Exists(graph);
    }
    else { return false; }
}

static string GetName()
{
    return Console.ReadLine();
}
