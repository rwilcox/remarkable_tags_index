using System.Text.Json;
using TagReferences = System.Collections.Generic.Dictionary<string, TagInfo>;

namespace Reports {
    namespace TagIndex {
        class TagIndex : BaseReports {
            private string rootFolder;

            public TagIndex(string rootFolder) {
                this.rootFolder = rootFolder;
            }

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

                        string notebookUUID = System.IO.Path.GetFileNameWithoutExtension(current.Name);
                        currentTag.appendPageReference(notebookName,
                                                       this.getPageNumberForPageUUID(pageId, doc, current.FullName),
                                                       pageId, notebookUUID);
                    }
                }

                return gathered;
            }


             public override void run(System.IO.FileInfo[] files, string format) {
                var present = new Reports.TagIndex.Presenters.PlainText();
                var jpresent = new Reports.TagIndex.Presenters.JsonPresenter();

                // Console.WriteLine("Notebooks");
                // Console.WriteLine("================================================");

                var referencesCollection = new TagReferences();
                foreach (System.IO.FileInfo fi in files) {
                    String notebookName = this.getNotebookNameForContentsFile(fi);
                    // Console.WriteLine(notebookName);
                    // Console.WriteLine(" * Last Slithin sync date: " + fi.LastWriteTimeUtc);

                    processFile(referencesCollection, fi);
                }

                switch (format) {
                    case "text":
                        present.write(referencesCollection);
                        break;
                    case "json":
                        jpresent.write(referencesCollection, this.rootFolder);
                        break;
                }
            }
        }

    }

}
