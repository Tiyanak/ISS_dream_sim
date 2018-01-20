using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Monetary_server
{
    class Writer
    {

        public StreamWriter fileWriter;
        
        public Writer()
        {
            
            string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Desktop\\monetary_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
            
            openFile(path);
        }

        public Writer(string path)
        {
            openFile(path);
        }

        public void openFile(string filename)
        {

            string resultDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + Path.DirectorySeparatorChar + "monetary_results";
            string filePath = resultDir + Path.DirectorySeparatorChar + filename;
            if (File.Exists(filePath))
            {
                this.fileWriter = new StreamWriter(filePath, true);
            } else
            {
                if (!Directory.Exists(resultDir)) {
                    Directory.CreateDirectory(resultDir);
                }
                this.fileWriter = new StreamWriter(File.Create(filePath));
            }   
        }

        public void writeLine(string line)
        {
            try
            {
                this.fileWriter.WriteLine(line);
            } catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public void writeLines(List<string> lines)
        {
            try
            {
                foreach (string line in lines)
                {
                    this.fileWriter.WriteLine(line);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public void closeFile()
        {
            this.fileWriter.Close();
        }

    }

}
