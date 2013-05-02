using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PuzzlePathDimension {
  sealed class Configuration {
    private Configuration() { }

    static public string UserDataPath {
#if DEBUG
      get { return System.Environment.CurrentDirectory + "/Content"; }
#else
      get { return System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/PuzzlePath"; }
#endif
    }
  }
}
