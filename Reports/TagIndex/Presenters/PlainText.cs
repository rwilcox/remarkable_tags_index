using TagReferences = System.Collections.Generic.Dictionary<string, TagInfo>;

namespace Reports {
    namespace TagIndex {
        namespace Presenters {

            class PlainText {
                public void write(TagReferences referencesCollection) {
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

}
