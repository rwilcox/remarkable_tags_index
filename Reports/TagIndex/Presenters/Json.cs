using TagReferences = System.Collections.Generic.Dictionary<string, TagInfo>;
using System.IO;
using System.Text.Json;

namespace Reports {
    namespace TagIndex {
        namespace Presenters {

            class ReferenceDTO {
                public string notebookName { get; set; }
                public int pageNumber { get; set; }
                public string rmFilePath { get; set; }
            }


            /// an object which is a data transfer object over a dictionary whose keys are tag names and values the places they are used
            class TagIndexDTO {
                public Dictionary< string, List<ReferenceDTO> > tags { get; set; }
            }


            class JsonPresenter {
                public void write(TagReferences referenceCollections, string rootFolder) {
                    TagIndexDTO index = new TagIndexDTO {
                        tags = new Dictionary< string, List<ReferenceDTO> >()
                    };

                    foreach (KeyValuePair<string, TagInfo> current in referenceCollections.OrderBy(k => k.Key)) {
                        List<ReferenceDTO> currentUsages = new List<ReferenceDTO>();
                        foreach(PageReference usage in current.Value.usages) {
                            currentUsages.Add(new ReferenceDTO {
                                    notebookName = usage.notebookName,
                                    pageNumber = usage.humanPageNumber(),
                                    rmFilePath = Path.Combine(rootFolder, usage.relativeRMFileLocation())
                            });
                        }
                        index.tags.Add(current.Key, currentUsages);
                    }

                    string json = JsonSerializer.Serialize(index);
                    Console.WriteLine(json);
                }
            }
        }
    }
}
