using System.Text.Json;
using TagReferences = System.Collections.Generic.Dictionary<string, TagInfo>;

namespace Reports {
    namespace TagIndex {
        class TagIndex : IBaseReports {
            private TagReferences processFile(TagReferences gathered, System.IO.FileInfo current) {
                JsonDocument doc = JsonDocument.Parse(current.OpenRead());

                String notebookName = this.getNotebookNameForContentsFile(current);
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

                        currentTag.appendPageReference(notebookName, this.getPageNumberForPageUUID(pageId, doc, current.FullName));
                    }
                }

                return gathered;
            }


             public override void run(System.IO.FileInfo[] files) {
                Console.WriteLine("Notebooks");

                Console.WriteLine("================================================");
                TagReferences referencesCollection = new TagReferences();
                foreach (System.IO.FileInfo fi in files) {
                    String notebookName = this.getNotebookNameForContentsFile(fi);
                    Console.WriteLine(notebookName);
                    Console.WriteLine(" * Last Slithin sync date: " + fi.LastWriteTimeUtc);

                    processFile(referencesCollection, fi);
                }

                String today = DateTime.Today.ToString("yyyy-MM-dd");

                Console.WriteLine("");
                Console.WriteLine("TAG INFORMATION (generated " + today + ")");
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

    }

}
