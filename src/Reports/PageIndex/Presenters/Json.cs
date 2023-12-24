using System.Text.Json;

namespace Reports {
    namespace PageIndex {
        namespace Presenters {

            class JsonPresenter {
                public void write(Dictionary<string, Notebook> input) {
                    string json = JsonSerializer.Serialize(input);
                    Console.WriteLine(json);
                }
            }
        }
    }
}
