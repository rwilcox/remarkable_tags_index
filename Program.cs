using System;
using System.IO;
using System.Collections.Generic;
using Reports.TagIndex;

class Program {


  /// below main method uses: https://github.com/dotnet/command-line-api/
  /// <summary>
  ///   tags_explorer LOCATION_OF_YOUR_REMARKABLE_FILES [--additional-options=...]
  /// </summary>
  /// <param name="argument">location where the RM files are</param>
  /// <param name="report">report type (defaults to tag-index)"
  /// <param name="format">what format to output (default: plain)</param>
  static void Main(string argument, string report="tag-index", string format="text") {
    DirectoryInfo notebookDir = new DirectoryInfo(argument);

    System.IO.FileInfo[] files = null;
    files = notebookDir.GetFiles("*.content");
    // Console.Write(files.Length);

    TagIndex ti = new Reports.TagIndex.TagIndex(argument);
    ti.run(files);
  }
}
