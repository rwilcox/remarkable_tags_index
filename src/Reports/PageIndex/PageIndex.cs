using System.Text.Json;



namespace Reports {
    namespace PageIndex {
        class PageIndex : BaseReports {
            private string rootFolder;

            public PageIndex(string rootFolder) {
                this.rootFolder = rootFolder;
            }


            void newPageIfRequired(Dictionary<string, Page> dict, string puuid) {
                if (!dict.ContainsKey(puuid)) {
                    dict[puuid] = new Page(puuid);
                }
            }


            /// the files we get will be the $uuid.content files
            Notebook processNotebookFile(System.IO.FileInfo current, String notebookName) {
                // Console.Write(current.FullName);
                Dictionary<string, Page> pagesByUUID = new Dictionary<string, Page>();

                var output = new Notebook();
                output.fileLocation = current.FullName;
                output.notebook_uuid = Path.GetFileNameWithoutExtension(current.FullName);

                JsonDocument doc = JsonDocument.Parse(current.OpenRead());

                /*
                  doc has two interesting properties
                     * pageTags: a list of objects that are tag entries. There are from 0 to many entries in this array of objects
                     * pages: an array of uuids for every page
                       OR pages may not be there. In that case the pages structure
                       is inside cPages - cPages.pages, which is an array of objects

                       for every item in the pages array find out if we've built up the tags for that page. If so we'll have a ton of tags. (If not, create a new Page entry as it doesn't yet exist)
                */
                var foundTags = doc.RootElement.TryGetProperty("pageTags", out var pageTags);
                if (foundTags) {
                    var c = 0;
                    foreach (JsonElement tagEntry in pageTags.EnumerateArray()) {
                        c = c++;
                        string puuid = tagEntry.GetProperty("pageId").GetString();

                        newPageIfRequired(pagesByUUID, puuid);
                        Page foundPage = pagesByUUID[puuid];

                        foundPage.tags.Add(tagEntry.GetProperty("name").GetString());
                    }
                }

               if (doc.RootElement.TryGetProperty("pages", out var pageArray)) {
                   var c = 0;
                   foreach (JsonElement pageUUID in pageArray.EnumerateArray() ) {
                       c++;
                       var str = pageUUID.GetString();

                       newPageIfRequired(pagesByUUID, str);
                       var foundPageRef = pagesByUUID[str];
                       foundPageRef.number = c;
                       output.pages.Add(foundPageRef);
                   }
               } else {
                   if (!doc.RootElement.TryGetProperty("cPages", out var checking)) {
                       // Console.Write("bailing: " + notebookName + "(" + current.FullName + ")\n");
                       // TODO: better error handling here??!
                       return output;
                   }
                   var cPages = doc.RootElement.GetProperty("cPages").GetProperty("pages");
                   var c = 0;
                   foreach(JsonElement cPageInfo in cPages.EnumerateArray()) {
                       c++;
                       var str = cPageInfo.GetProperty("id").GetString();
                       newPageIfRequired(pagesByUUID, str);
                       var foundPageRef = pagesByUUID[str];
                       foundPageRef.number = c;
                       output.pages.Add(foundPageRef);
                       //Console.Write(output.pages.Count.ToString() + "\n");
                   }
               }

                return output;
            }


            public override void run(System.IO.FileInfo[] files, string format) {
                var present = new  Reports.PageIndex.Presenters.PlainText();
                var jPresent = new Reports.PageIndex.Presenters.JsonPresenter();

                var notebookDict = new System.Collections.Generic.Dictionary<string, Notebook>();

                foreach (System.IO.FileInfo fi in files) {
                    String notebookName = this.getNotebookNameForContentsFile(fi);

                    Notebook n = processNotebookFile(fi, notebookName);
                    n.name = notebookName;
                    notebookDict.Add(n.fileLocation, n);
                    // TODO: Notebook needs to show where it is in the Remarkable file system hierarchy
                }
                switch (format) {
                    case "text": {
                        present.write(notebookDict);
                        break;
                    }
                    case "json": {
                        jPresent.write(notebookDict);
                        break;
                    }
                }
            }
        }
    }
}
