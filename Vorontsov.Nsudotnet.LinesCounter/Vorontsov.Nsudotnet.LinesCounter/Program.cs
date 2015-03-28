using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vorontsov.Nsudotnet.LinesCounter
{
    class Program
    {
        static void Main(string[] args)
        {
            string filter, commentsOn, commentsOff, inlineComment;
            
            if (args.Length > 0)
                filter = args[0];
            else
                filter = "*.cs";

            if (args.Length > 1)
                commentsOn = args[1];
            else
                commentsOn = "/*";

            if(args.Length > 2)
                commentsOff = args[2];
            else
                commentsOff = "*/";

            if (args.Length > 3)
                inlineComment = args[3];
            else
                inlineComment = "//";

            var files = from file in Directory.EnumerateFiles(Directory.GetCurrentDirectory(), filter, SearchOption.AllDirectories)
                        select file;
            int countOfLines = 0;
            bool readingComments;

            foreach (var file in files)
            {
                using (StreamReader sr = File.OpenText(file))
                {
                    string l;
                    readingComments = false;

                    while ((l = sr.ReadLine()) != null)
                    {
                        if (l == "" || l.StartsWith(inlineComment))
                            continue;

                        int i = 0;
                        bool lineIsNotEmpty = false;
                        while(i < l.Length && i != -1)
                        {
                            int pos;
                            if (readingComments == false)
                            {
                                pos = l.IndexOf(commentsOn, i);

                                if (pos != -1)
                                    readingComments = true;
 
                                if (pos != i)
                                    lineIsNotEmpty = true;

                                i = (pos == -1) ? pos : pos + commentsOn.Length;
                            }
                            else
                            {
                                pos = l.IndexOf(commentsOff, i);
                                
                                if (pos != -1)
                                    readingComments = false;

                                i = (pos == -1) ? pos : pos + commentsOff.Length;
                            }
                        }

                        if (lineIsNotEmpty)
                        {
                            ++countOfLines;
                            Console.WriteLine(l);
                        }
                    }
                }
            }
            Console.WriteLine(countOfLines);
            Console.ReadLine();
/*            foreach (var s in files)
//                Console.WriteLine(s);
*/
        }
    }
}
