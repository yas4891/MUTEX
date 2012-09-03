using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GSTAppLogic.templating
{
    /// <summary>
    /// the purpose of this whole class is to strip source files from the templates.
    /// This is needed if the professor provided a template to solve the programming problem.
    /// It will strip the provided template from the final source file 
    /// </summary>
    public static class TemplatingHelper
    {
        /// <summary>
        /// strips lines from 
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="templatePath"></param>
        /// <returns>path to the temporary stripped file</returns>
        public static string StripTemplateFromSourceFile(string sourcePath, string templatePath)
        {
            var source = File.ReadAllLines(sourcePath).Select(line => line.Trim()).ToList();
            var template = File.ReadAllLines(templatePath).Select(line => line.Trim()).ToArray();

            foreach(var tmpl in template)
            {
                source.Remove(tmpl);
            }

            var path = Path.GetTempFileName();
            using(var writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                foreach (var line in source)
                    writer.WriteLine(line);    
            }
            
            return path;
        }
    }
}
