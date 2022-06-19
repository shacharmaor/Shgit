using ShgitObjects;


string graphsLocation = @"C:\shgit\graphs.txt";

if(!File.Exists(graphsLocation)) { File.Create(graphsLocation); }//for first use on a computer

Client.ConnectToDatabase(); //login on start

Console.WriteLine("\nCommands:\n" +
    "1. '<full graph path> -u': upload changes to server\n" +
    "2. '<full graph path> -i': install changes from server\n" +
    "3. '<full graph path> -n': create new graph\n" +
    "4. <graph name> -j: join existing graph" +
    "5. '<graph name> -d': delete exiting graph\n" +
    "6. 'quit': quit the program\n");

bool running = true;
string input = ""; //user command
while (running)
{
    Console.WriteLine("Enter Command:\n");
    input = Console.ReadLine().ToLower();
    if (input == "quit") 
    { 
        running = false;
        Client.Quit();
    }
    else if (LegalCommand(input))
    {
        string[] Input = input.Split(' ');
        string graph = Input[0];
        string command = Input[1];
        switch (command)
        {
            case "-u":
                UploadGraph(graph);
                break;
            case "-i":
                Client.DownloadGraph(graph, GetGraphName(graph));
                break;
            case "-n":
                try { CreateGraph(graph, graphsLocation); }
                catch (FileNotFoundException e) { Console.WriteLine(e.Message); }
                break;
            case "-j":
                JoinGraph(graph);
                break;
            case "-d":
                DeleteGraph(graph);
                break;
        }
    }
    else { Console.WriteLine("illegal command"); }
}

Console.WriteLine("all rights reserved to roy gilboa's left testicle");


static void CreateGraph(string graph, string graphsLocation)
{
    string[] graphs = File.ReadAllLines(graphsLocation); //all existing graphs on the computer. Gotta connect it to the server!

    if (!Directory.Exists(graph)) { throw new FileNotFoundException("Directory not found. try again dumbass"); }
    else if (graphs.Contains(graph)) { throw new FileNotFoundException("Directory already a graph. roy gilboa ass motherfucker"); }

    var writer = new StreamWriter(graphsLocation, append: true);
    writer.WriteLine(graph);  //add graph to list
    writer.Close();
    
    UploadGraph(graph);

    Console.WriteLine("graph created!");
}


static string GetGraphName(string graph)
{
    string[] graphParts = graph.Split('\\');
    return graphParts[graphParts.Length - 1];
}

static void UploadGraph(string graph)
{
    string[] files = Directory.GetFiles(graph, "*", SearchOption.AllDirectories);
    string graphName = GetGraphName(graph);
    foreach (string file in files)
    {
        Client.UploadGraph(file, Path.GetRelativePath(new DirectoryInfo(graph).Parent.FullName, file), graphName);
    }

    Console.WriteLine("Graph uploaded successfully!");
}


static void JoinGraph(string graph)
{

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
    return false;
}
