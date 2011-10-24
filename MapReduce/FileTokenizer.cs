using System;
using System.Collections.Generic;
using System.IO;

namespace MapReduce
{
    public class FileTokenizer : IDisposable
    {
        private readonly StreamReader reader;

        public FileTokenizer(string filename)
        {
            reader = new StreamReader(filename);
        }

        public IEnumerable<string> GetTokens()
        {
            var token = string.Empty;
            var buffer = new char[1];
            while (reader.Read(buffer, 0, 1) > 0)
            {
                if (char.IsControl(buffer[0]) || char.IsWhiteSpace(buffer[0]))
                {
                    if (string.IsNullOrEmpty(token))
                    {
                        continue;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(token))
                        {
                            yield break;
                        }

                        yield return token;
                        token = string.Empty;
                        continue;
                    }
                }
                token += buffer[0];
            }
            
        }

        public void Dispose()
        {
            reader.Close();
        }
    }
}