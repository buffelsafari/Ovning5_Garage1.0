using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage10.IO
{
    class FileStorage : IStorage
    {
        public void Load(ILoadable thing)
        {
            using (FileStream stream = new FileStream("garagesavefile.txt", FileMode.Open, FileAccess.Read))
            {
                StreamReader reader = new StreamReader(stream);
                thing.Load(reader);
                
            }

        }

        public void Save(ISavable thing)
        {
            using (FileStream stream = new FileStream("garagesavefile.txt", FileMode.Create, FileAccess.Write))
            {
                StreamWriter writer = new StreamWriter(stream);
                thing.Save(writer);
                writer.Flush();
            }
        }



    }
}
