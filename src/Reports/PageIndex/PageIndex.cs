struct Page {
    public int number;
    public string[] tags;
    public string uuid;
}

struct Notebook {
    public string name;
    public Page[] pages;
}

namespace Reports {
    namespace PageIndex {
        class PageIndex : BaseReports {
            private string rootFolder;

            public PageIndex(string rootFolder) {
                this.rootFolder = rootFolder;
            }


            /// the files we get will be the $uuid.content files
            Notebook processNotebookFile(System.IO.FileInfo current) {
                JsonDocument doc = JsonDocument.Parse(current.OpenRead());

                /*
                  doc has two interesting properties
                     * pages: an array of uuids for every page
                     * pageTags: a list of objects that are tag entries. There are from 0 to many entries in this array of objects

                  for every item in the pages array find all its matching items
                  in the pagesTags array, and map just the name of that out.

                */
                var output = new Notebook();

                return output;
            }


            public override void run(System.IO.FileInfo[] files, string format) {
                var present = new  Reports.PageIndex.Presenters.PlainText();
                var notebookDict = new System.Collections.Generic.Dictionary<string, Notebook>();

                foreach (System.IO.FileInfo fi in files) {
                    String notebookName = this.getNotebookNameForContentsFile(fi);
                    if (!notebookDict.ContainsKey(notebookName)) {
                            var n = new Notebook();
                            n.name = notebookName;

                    }

                    Notebook n = processNotebookFile(fi);
                    notebookDict.Add(notebookName, n);   // TODO: probably should be path or something??!

                }
                Console.Write("hi");
            }
        }
    }
}
