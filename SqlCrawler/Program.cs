using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;

namespace SqlCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Must input database name and fill the configs!");
                Console.Read();
                return;
            }

            var database = args[0];
            var dc = new DataCenter(database);
            dc.Produce().MergeToObjects();

            foreach (var obj in dc.Objects)
            {
                var markdown = new MarkdownFormatter(database, obj);

                var folder = Path.Combine(Directory.GetCurrentDirectory(), database, obj.Name.Substring(0, 4), obj.Name);
                Directory.CreateDirectory(folder);
                File.WriteAllText(Path.Combine(folder, obj.Name + ".md"), markdown.ToString());

                if (!string.IsNullOrWhiteSpace(obj.Definition))
                {
                    File.WriteAllText(Path.Combine(folder, obj.Name + ".sql"), obj.Definition);   
                }
            }
        }
    }
}