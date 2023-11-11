using System.Text.Json;
using Reports;

namespace Reports {
    abstract class IBaseReports {
        abstract public void run(System.IO.FileInfo[] files);

        public String getNotebookNameForContentsFile(System.IO.FileInfo contents) {
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
        public int getPageNumberForPageUUID(String pUUID, JsonDocument doc, String notebookName) {
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

                Console.WriteLine("Looking for " + pUUID + " in all the wrong places: " + notebookName);
            }
            return output;
        }
    }
}
