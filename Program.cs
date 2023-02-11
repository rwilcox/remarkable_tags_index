using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

using TagReferences = System.Collections.Generic.Dictionary<string, TagInfo>;

class Program {
  static String getNotebookNameForContentsFile(System.IO.FileInfo contents) {
    String output = "";
    String onlyName = Path.GetFileNameWithoutExtension(contents.Name);
    foreach (System.IO.FileInfo metadata in contents.Directory.EnumerateFiles(onlyName + ".metadata")) {
      //Console.WriteLine("Found metadata file for " + contents.Name);
      JsonDocument doc = JsonDocument.Parse(metadata.OpenRead());
      return doc.RootElement.GetProperty("visibleName").GetString();
    }

    return output;
  }


  // result will be 0 based
  static int getPageNumberForPageUUID(String pUUID, JsonDocument doc, String notebookFileName) {
    int output = -1;

    TagInfo mine = new TagInfo();
    // ?? does cPages mean continuous pages?? RPW 12-28-2022 ?
    if (doc.RootElement.TryGetProperty("cPages", out var pagesElement)) {
      JsonElement pages = pagesElement.GetProperty("pages");
      foreach (JsonElement currentPage in pages.EnumerateArray()) {
        output++;
        String currentPageId = currentPage.GetProperty("id").GetString();
        if (currentPageId == pUUID ) {
          return output;
        }
      }
    }

    if (output == -1) {
        if (doc.RootElement.TryGetProperty("pages", out var pagesOnlyElement)) {
          foreach (JsonElement currentPage in pagesOnlyElement.EnumerateArray()) {
            output++;
            if (currentPage.GetString() == pUUID) {
              return output;
            }
          }
        }

      Console.WriteLine("Looking for " + pUUID + " in all the wrong places: " + notebookFileName);
    }
    return output;
  }


  static TagReferences processFile(TagReferences gathered, System.IO.FileInfo current) {
    JsonDocument doc = JsonDocument.Parse(current.OpenRead());

    String notebookName = getNotebookNameForContentsFile(current);
    // Console.WriteLine(notebookName);
    if (doc.RootElement.TryGetProperty("pageTags", out var pageTagsElement)) {
      foreach( JsonElement currentPageTag in pageTagsElement.EnumerateArray()) {
        String currentTagName = currentPageTag.GetProperty("name").GetString();
        String pageId = currentPageTag.GetProperty("pageId").GetString();

        TagInfo currentTag = null;
        gathered.TryGetValue(currentTagName, out currentTag);
        if (currentTag == null) {
          currentTag = new TagInfo(); // we know that while pages etc use UUIDs tag names are just the strings. (No actual unified index)
          currentTag.setName(currentTagName);
          gathered[currentTagName] = currentTag;
        }

      currentTag.appendPageReference(notebookName, getPageNumberForPageUUID(pageId, doc, current.FullName));
      }
    }

    return gathered;
  }

  static void Main(string[] args) {
    String myRoot = args[0];
    DirectoryInfo notebookDir = new DirectoryInfo(myRoot);

    System.IO.FileInfo[] files = null; 
    files = notebookDir.GetFiles("*.content");
    // Console.Write(files.Length);

    Console.WriteLine("Notebooks");
    Console.WriteLine("================================================");
    TagReferences referencesCollection = new TagReferences();
    foreach (System.IO.FileInfo fi in files) {
      String notebookName = getNotebookNameForContentsFile(fi);
      Console.WriteLine(notebookName);
      Console.WriteLine(" * Last Slithin sync date: " + fi.LastWriteTimeUtc);

      processFile(referencesCollection, fi);
    }

    Console.WriteLine("");
    Console.WriteLine("TAG INFORMATION");
    Console.WriteLine("================================================");
    Console.Write("Count of tags: ");
    Console.WriteLine(referencesCollection.Keys.Count);
    Console.WriteLine("================================================");

    foreach (KeyValuePair<string, TagInfo> current in referencesCollection.OrderBy(k => k.Key)) {
        current.Value.print();
    }
    Console.WriteLine("================================================");
  }
}