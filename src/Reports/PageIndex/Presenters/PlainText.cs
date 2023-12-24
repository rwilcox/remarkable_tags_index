namespace Reports {
    namespace PageIndex {
        namespace Presenters {
            class PlainText {
                public void write(Dictionary<string, Notebook> input) {
                    // TODO: need to listen to folder structure
                    foreach (KeyValuePair<string, Notebook> current in input.OrderBy(k => k.Key)) {
                        current.Value.print();
                    }
                }
            }
        }
    }
}
