using System;
using System.Collections.Generic;
using System.IO;

namespace MapReduce
{
    public class StringTokenizer : IDisposable
    {
        private readonly string _input;

        public StringTokenizer(string input)
        {
            _input = input;
        }

        public IEnumerable<string> GetTokens()
        {
            var token = string.Empty;
            foreach (var c in _input)
            {
                if (char.IsControl(c) || char.IsWhiteSpace(c))
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
                token += c;
            }
            if(!string.IsNullOrEmpty(token))
            {
                yield return token;
            }
        }

        public void Dispose()
        {
        }
    }
}