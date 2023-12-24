class Page {
    public int number { get; set; }
    public List<string> tags { get; set; }
    public string uuid { get; set; }

    public Page(string iuuid) {
        uuid = iuuid;
        tags = new List<string>();
    }

    public void print() {
        Console.Write("  * ");
        Console.Write(number.ToString());
        Console.Write(" " + String.Join(",", tags));
        Console.Write("\n");
    }
}
