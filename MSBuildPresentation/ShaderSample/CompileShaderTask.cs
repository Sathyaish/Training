using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.IO;

namespace ShaderTask
{
    public class CompileShader : Task
    {
        [Required]
        public ITaskItem Shader { get; set; }

        [Required]
        public string OutputFolder { get; set; }

        [Required]
        public string CompilerLocation { get; set; }

        public override bool Execute()
        {
            if (Shader == null)
            {
                Log.LogError("An error occured while compiling shaders.");
                return false;
            }

            string fullPath = Shader.GetMetadata("FullPath");
            string filename = Shader.GetMetadata("Filename");
            string extension = Shader.GetMetadata("Extension");
            if (string.IsNullOrEmpty(fullPath))
            {
                Log.LogError("An error occured while compiling shaders.");
                return false;
            }

            if (File.Exists(fullPath) == false)
            {
                Log.LogError("Could not find shader located at '{0}'", fullPath);
                return false;
            }

            if (Directory.Exists(OutputFolder) == false)
            {
                try
                {
                    Directory.CreateDirectory(OutputFolder);
                }
                catch (Exception)
                {
                    Log.LogError("Compile Shader task failed to create directory at '{0}', please run build with higher privileges or create directory manually", OutputFolder);
                    return false;
                }
            }

            Log.LogMessage("Compiling shader: '{0}{1}'", filename, extension); 

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = CompilerLocation + "\\fxc.exe";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.Arguments = "/Fo " + OutputFolder + "\\" + filename + ".o /LD /Tfx_2_0 " + fullPath;
            process.StartInfo = startInfo;
            process.Start();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if(string.IsNullOrEmpty(error) == false)
            {
                try
                {
                    error = error.Replace(fullPath, "");
                    string[] pieces = error.Split(':');
                    string line = pieces[0].Replace("(", "").Replace(")", "");
                    int lineNum = int.Parse(line);
                    string errorCode = pieces[1].Replace("error ", "");
                    string errorString = pieces[2] + " " + pieces[3];
                    errorString = errorString.Replace("compilation failed; no code produced", "");
                    errorString = errorString.Replace("\r\n", "");
                    errorString = errorString.Replace("\n", "");

                    Log.LogError("shader compiler", errorCode, "", fullPath, lineNum, 0, 0, 0, errorString);
                    return false;
                }
                // We messed up parsing, just spit out the error as it was from the compiler
                catch (Exception)
                {
                    Log.LogMessage(error);
                    return false;
                }
            }

            return true;
        }
    }
}
