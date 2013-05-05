using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PuzzlePathDimension {
  sealed class Configuration {
    private Configuration() { }

    static public string UserDataPath {
#if DEBUG
      get { return System.Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Content"; }
#else
      get { return System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) 
        + Path.DirectorySeparatorChar + "PuzzlePath"; }
#endif
    }
  }
}
