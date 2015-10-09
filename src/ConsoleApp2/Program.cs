using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Org.BouncyCastle.Cms;

namespace ConsoleApp2
{
    public class Program
    {
        public void Main(string[] args)
        {
            if (args.Length > 0)
            {
                string fullFileName = Path.GetFullPath(args[0]);
                FileStream file = new FileStream(fullFileName, FileMode.Open);
                CmsSignedData signedFile = new CmsSignedData(file);
            }
        }
    }
}
