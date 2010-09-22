using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace SARA.Utils
{
    /// <summary>
    /// Configure file, that contains parametrs for program.
    /// </summary>
    /// <remarks>
    /// Syntax of configure files:
    /// <list type="bullet">
    /// <item>comment lines starts with '<c>#</c>'.</item>
    /// <item>empty lines are ignored.</item>
    /// <item>other lines should have syntax: <c>parametr_name = value</c></item>
    /// <item>parametr names must start with letter or underscore ('<c>_</c>'), and can contains only letters, 
    /// digits and underscore.</item>
    /// <item>case of letters is significant.</item>
    /// <item>the same parametr can not be defined many times.</item>
    /// </list>
    /// 
    /// <example>
    /// <code>
    /// # This is comment.
    /// #This is also valid comment.
    /// 
    /// # example integer value:
    /// int_value    = 7
    /// # example float value:
    /// float_value  = 1.5
    /// # example bool value
    /// bool_value   = True
    /// # exmaple string value:
    /// string_value = "Hello World!!!"
    /// 
    /// # path seqences have more complex syntax.
    /// # Possibile ways to describe sequences are:
    /// 
    /// # * Simple file name
    /// single_file = "foo.bar"
    /// 
    /// # * File name pattern
    /// sequence1 = "file*.txt"
    /// 
    /// # * Indexed sequence (for example a1.fit, a2.fit, ... , a523.fit)
    /// sequence2 = "a&lt;523&gt;.fit"
    /// 
    /// # * Paths stored in other file:
    /// sequence3 = @"files.txt"
    /// 
    /// </code>
    /// </example>
    /// </remarks>
    public class ConfigFile
    {
        private static NumberFormatInfo _formatInfo = new CultureInfo("en-US", false).NumberFormat;

        private Dictionary<string, string> _params = new Dictionary<string,string>();

        /// <summary>
        /// Open and parse specified configure file.
        /// </summary>
        /// <param name="path">
        /// Path to configure file.
        /// </param>
        public ConfigFile(string path)
        {
            string[] lines = File.ReadAllLines(path);

            char[] sepEq = {'='};

            foreach (var line in lines)
            {
                if (line.StartsWith("#") || line.Trim() == "")
                    continue;
                
                string[] parts = line.Split(sepEq);
                string paramName = parts[0].Trim();

                if (parts.Length != 2)
                    throw new FormatException("Syntax error in " + path);
                
                foreach (var c in paramName)
                    if (!Char.IsLetterOrDigit(c) && !(c == '_'))
                        throw new FormatException("Parametr name must contain only letters, digits and underscore '_'.");

                if (_params.ContainsKey(paramName))
                    throw new FormatException("Syntax error in " + path + ": duplicate definition of " + paramName);

                _params.Add(paramName, parts[1]);
            }
        }

        /// <summary>
        /// Get integer value of parametr.
        /// </summary>
        /// <remarks>
        /// Syntax of integer value:
        /// <code>
        /// intValue ::= [sign] {digit}
        /// </code>
        /// </remarks>
        /// <param name="paramName">
        /// Name of parametr.
        /// </param>
        /// <returns>
        /// Integer value of specified parametr.
        /// </returns>
        public int GetIntParam(string paramName)
        {
            return Int32.Parse(_params[paramName]);
        }

        /// <summary>
        /// Get bool value of parametr.
        /// </summary>
        /// <remarks>
        /// Syntax of bool value:
        /// <code>
        /// boolValue ::= "True" | "False"
        /// </code>
        /// </remarks>
        /// <param name="paramName">
        /// Name of parametr.
        /// </param>
        /// <returns>
        /// Bool value of specified parametr.
        /// </returns>
        public bool GetBoolParam(string paramName)
        {
            string tmpParam = _params[paramName].Trim();
            if (tmpParam == "True")
                return true;
            else if (tmpParam == "False")
                return false;
            else
                throw new FormatException("Bool value of parametr have syntax : boolValue ::= \"True\" | \"False\".");
        }

        /// <summary>
        /// Get floating point value of parametr.
        /// </summary>
        /// <param name="paramName">
        /// Name of parametr.
        /// </param>
        /// <remarks>
        /// Syntax of floating point value:
        /// <code>
        /// floatValue ::= [sign] {digit} [ '.' [{ digit }] ] [ ('e' | 'E') [sign] {digit} ]
        /// </code>
        /// </remarks>
        /// <returns>
        /// Floating point value of specified parametr.
        /// </returns>
        public float GetFloatParam(string paramName)
        {
            return Single.Parse(_params[paramName], _formatInfo);
        }

        /// <summary>
        /// Get string value of parametr.
        /// </summary>
        /// <remarks>
        /// If value of parametrs contains character '<c>"</c>', function returns parsed string between first and 
        /// last '<c>"</c>' character (when string contains only one '<c>"</c>' character, function returns empty string).
        /// Otherwise function returns all string.
        /// 
        /// String parser repeace folowing sequences:
        /// <list type="bullet">
        /// <item>"<c>\\</c>" by "<c>\</c>".</item>
        /// <item>"<c>\n</c>" by new line.</item>
        /// <item>"<c>\r</c>" by carrige return.</item>
        /// <item>"<c>\t</c>" by horizontal tab.</item>
        /// </list>
        /// </remarks>
        /// <param name="paramName">
        /// Name of parametr.
        /// </param>
        /// <returns>
        /// String value of specified parametr.
        /// </returns>
        public string GetStringParam(string paramName)
        {
            string result = _params[paramName];
            if (result.Contains("\""))
            {
                int start = result.IndexOf('"');
                int end = result.LastIndexOf('"');
                if (start == end)
                    return "";

                result = result.Substring(start + 1, end - start - 1);
                result = result.Replace("\\n", "\n");
                result = result.Replace("\\r", "\r");
                result = result.Replace("\\t", "\t");
                result = result.Replace("\\\\", "\\");

                return result;
            }
            else
                return result;
        }

        /// <summary>
        /// Get path sequence value of parametr.
        /// </summary>
        /// <remarks>
        /// TODO: describe syntax.
        /// </remarks>
        /// <param name="paramName">
        /// Name of parametr.
        /// </param>
        /// <returns>
        /// Path sequence value of specified parametr.
        /// </returns>
        public string[] GetPathsParam(string paramName)
        {
            List<string> result = new List<string>();
            ParsePathSequence(_params[paramName], result, Environment.CurrentDirectory);
            return result.ToArray();
        }

        private void ParsePathSequence(string input, List<string> output, string currentDir)
        {
            string trimInput = input.Trim();
            if (trimInput.StartsWith("@"))
            {
                List<string> cointainer = new List<string>();
                ParsePathSequence(trimInput.Substring(1), cointainer, currentDir);
                foreach (string path in cointainer)
                {
                    // TODO: calc new current dir.
                    foreach (string pattern in File.ReadAllLines(path))
                    {
                        if (pattern.Trim() != "")
                            ParsePathSequence(pattern, output, currentDir);
                    }
                }
            }
            else
            {
                string input2;
                if (trimInput.Contains("\""))
                {
                    int start = trimInput.IndexOf('"');
                    int end = trimInput.LastIndexOf('"');

                    if (start == end)
                        throw new FormatException("Invalid sequence pattern : " + trimInput);

                    input2 = trimInput.Substring(start + 1, end - start - 1);
                }
                else
                    input2 = trimInput;

                if (input2.Contains("<") && input2.Contains(">"))
                {
                    int pref = input2.IndexOf('<');
                    int suff = input2.LastIndexOf('>');

                    if (pref > suff)
                        throw new FormatException("Invalid sequence pattern : " + trimInput);

                    string prefix = input2.Substring(0, pref);
                    string suffix = input2.Substring(suff + 1);
                    int count = Int32.Parse( input2.Substring(pref+1, suff - pref - 1) );

                    for (int n = 1; n <= count; n++)
                        output.Add(prefix + n.ToString() + suffix);
                }
                else if (input2[1] == ':')
                    output.AddRange(Directory.GetFiles(input2.Substring(0,3), input2.Substring(3), SearchOption.AllDirectories));
                else
                    output.AddRange(Directory.GetFiles(currentDir, input2, SearchOption.AllDirectories));
            }
        }
    }
}
