using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace MD
{
    class Program
    {
        // Tash1 is Text encryption
        public string Tash1;
        // the result
        public string TashR;

        static void Main(string[] args)
        {
            
        }
        public void Tashfer()
        {
            string t = Tash1;
            MD5 m = MD5.Create();
            byte[] inp = Encoding.ASCII.GetBytes(t);
            byte[] hash = m.ComputeHash(inp);
            t = BitConverter.ToString(hash).Replace("-", "");
            TashR = t;

        }
    }
}
