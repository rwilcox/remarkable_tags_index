class Notebook {
    public string name { get; set; }
    public List<Page> pages { get; set; }
    public string fileLocation { get; set; }
    public string notebook_uuid { get; set; }

    public Notebook() {
        pages = new List<Page>();
    }

    public void print() {
        Console.Write(name + " pages: " + pages.Count.ToString() + "(" + fileLocation + ")\n");
        foreach (Page current in pages)
        {
            current.print();
        }
    }
}
