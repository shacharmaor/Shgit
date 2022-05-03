using System.Globalization;

namespace ShgitObjects
{
    public class Graph
    {
        public string Path { get; set; } //location of graph
        public Graph(string path, string graphsLocation)
        {
            Path = path;

            string[] graphs = File.ReadAllLines(graphsLocation);

            if(!Directory.Exists(path)) { throw new FileNotFoundException("file not found. try again dumbass"); }
            else if(graphs.Contains(path)) { throw new FileNotFoundException("file already a graph. roy gilboa ass motherfucker"); }
            
            using var writer = new StreamWriter(graphsLocation, append: true);
            writer.WriteLine(Path);  //add graph to list
            writer.Close();

            string shgitPath = path + @"\.shgit";
            Directory.CreateDirectory(shgitPath); //shgit file for shgit stuff (versions)
            DirectoryInfo d = new DirectoryInfo(Path);
            FileInfo[] files = d.GetFiles("*", SearchOption.AllDirectories);
            string filePath = "";
            foreach (FileInfo file in files) 
            {
                filePath = shgitPath + @"\" + file.Name;
                Directory.CreateDirectory(filePath);
                File.Copy(file.FullName, filePath + @"\" + GetMetaData() + file.Extension);
            }
            
            Console.WriteLine("graph created! " + GetMetaData());
        }

        private string GetMetaData() //Commiter, commit time
        {
            return "user_" + DateTime.Now.ToString(new CultureInfo("de-DE")).Replace(' ', '_').Replace('.', '-').Replace(':', '-');
        }
    }
}