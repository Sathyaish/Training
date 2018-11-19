using System.Collections.Generic;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;

namespace TempReflectTask
{
    public class ReflectionParseTask : Task
    {
        [Required]
        public string ReflectionToken { get; set; }

        [Required]
        public string ReflectionFileSchema { get; set; }

        [Required]
        public string ReflectionLineSchema { get; set; }

        [Required]
        public string IntermediateFolder { get; set; }

        [Required]
        public string FileToWrite { get; set; }

        [Required]
        public ITaskItem[] Files { get; set; }

        public override bool Execute()
        {
            foreach (var file in this.Files)
            {
                ParseFile(file.GetMetadata("FullPath"));
            }

            CreateFile();

            return true;
        }

        private List<string> _functionsFound = new List<string>();

        private void ParseFile(string fileName)
        {
            Log.LogMessage("Reflecting {0} ...", Path.GetFileName(fileName));
            using (StreamReader stream = new StreamReader(fileName))
            {
                string line;

                while ((line = stream.ReadLine()) != null)
                {
                    if (line == this.ReflectionToken)
                    {
                        line = stream.ReadLine();
                        ParseLine(line);
                    }
                }
            }

            string intermediateFile = Path.Combine(this.IntermediateFolder, Path.GetFileNameWithoutExtension(fileName)) + ".reflect.obj";
            using (StreamWriter writer = new StreamWriter(intermediateFile))
            {
                foreach (var name in this._functionsFound)
                {
                    writer.WriteLine(string.Format(this.ReflectionLineSchema, name));
                }
            }
            this._functionsFound.Clear();
        }

        private void ParseLine(string line)
        {
            int openParen = -1;
            int nameStart = -1;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '(')
                {
                    openParen = i;
                    break;
                }
            }

            bool startedName = false;
            for (int i = openParen - 1; i >= 0; i--)
            {
                if (line[i] == ' ' && startedName == true)
                {
                    nameStart = i + 1;
                    break;
                }
                else if (line[i] == ' ' && startedName == false)
                {
                    startedName = true;
                }
                else
                {
                    startedName = true;
                }
            }

            string name = line.Substring(nameStart, openParen - nameStart).Replace(" ", "");
            this._functionsFound.Add(name);
        }

        private void CreateFile()
        {
            Log.LogMessage("Creating {0} ...", Path.GetFileName(this.FileToWrite));
            List<string> chunks = new List<string>();
            string[] files = Directory.GetFiles(this.IntermediateFolder, "*.reflect.obj");
            foreach (var file in files)
            {
                using (StreamReader stream = new StreamReader(file))
                {
                    chunks.Add(stream.ReadToEnd());
                }
            }

            using (StreamWriter writer = new StreamWriter(this.FileToWrite))
            {
                string chunk = string.Empty;
                foreach (var line in chunks)
                {
                    chunk += line + "\n";
                }
                string contents = string.Format(this.ReflectionFileSchema, chunk);
                writer.Write(contents);
            }
        }
    }
}
