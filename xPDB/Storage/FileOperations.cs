using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xPDB.Storage
{
    public class FileOperations
    {
        public static int countFileLines(string path)
        {
            try
            {
                var lineCount = 0;
                using (var reader = File.OpenText(path))
                {
                    while (reader.ReadLine() != null)
                    {
                        lineCount++;
                    }
                }
                return lineCount;
            }
            catch
            {
                return -255;
            }
        }

        public static string readAllText(string path)
        {
            return System.IO.File.ReadAllText(path);
        }
        public static byte[] readAllBytes(string path)
        {
            return System.IO.File.ReadAllBytes(path);
        }
        public static string[] readAllLines(string path)
        {
            return System.IO.File.ReadAllLines(path);
        }
        public static string readViaStreamReader(string path)
        {
            string data;
            using (StreamReader sr = new StreamReader(path))
            {
                data = sr.ReadToEnd();
            }
            return data;
        }
        public static string readSkipLine(string path, int line)
        {
            return File.ReadLines(path).Skip(line).Take(1).First();
        }
        public static string readStreamLine(string path, int line)
        {
            string data = null;
            try
            {
                using (Stream stream = File.Open(path, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        for (int i = 0; i < line; ++i)
                        {
                            data = reader.ReadLine();
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
            return data;
        }

        public static byte[] readBinaryPosition(string path, int pos, int required)
        {
            using (BinaryReader b = new BinaryReader(File.Open(path,
                               FileMode.Open)))
            {
                b.BaseStream.Seek(pos, SeekOrigin.Begin);
                byte[] by = b.ReadBytes(required);
                return by;
            }

        }
        public static string readIterativeLine(string path, int line)
        {
            IEnumerable<string> lines = File.ReadLines(path);
            return lines.Skip(line - 1).First();
        }

        public static bool writeViaStream(string path, string content)
        {
            try
            {
                using (StreamWriter writetext = new StreamWriter(path))
                {
                    writetext.Write(content);
                }
                return true;
            }
            catch
            {
                //GlobalErrorHandler.logError(ex);
                return false;
            }
        }
        public static bool writeToLine(string path, string content, int line)
        {
            try
            {
                string[] arrLine = File.ReadAllLines(path);
                arrLine[line - 1] = content;
                WriteAllLinesBetter(path, arrLine);
                return true;
            }
            catch
            {
                //GlobalErrorHandler.logError(ex);
                return false;
            }
        }
        public static void WriteAllLinesBetter(string path, params string[] lines)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            if (lines == null)
                throw new ArgumentNullException("lines");
            System.IO.File.WriteAllText(path, string.Empty);
            using (var stream = File.OpenWrite(path))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                if (lines.Length > 0)
                {
                    for (int i = 0; i < lines.Length; i++)
                    {
                        writer.Write(lines[i] + "\n");
                    }
                }
            }
        }
        public static bool writeAppend(string path, string content)
        {
            try
            {
                File.AppendAllText(path, content);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool writeByteArrayViaStream(string path, byte[] data)
        {
            try
            {
                // Open file for reading
                System.IO.FileStream _FileStream =
                   new System.IO.FileStream(path, System.IO.FileMode.Create,
                                            System.IO.FileAccess.Write);
                // Writes a block of bytes to this stream using data from
                // a byte array.
                _FileStream.Write(data, 0, data.Length);

                // close file stream
                _FileStream.Close();

                return true;
            }
            catch (Exception _Exception)
            {
                Utility.UISnippets.messageBoxWarning(_Exception.ToString()
                                  , "Exception");
            }

            // error occured, return false
            return false;
        }
    }
}
