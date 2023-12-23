public class TagInfo {
  private String name;
  public List<PageReference> usages { get; }


  public void setName(String n) {
    this.name = n;
  }


  public String getName() {
    return this.name;
  }


  public void appendPageReference(String notebookName, int pageNumber, string pageUUID, string notebookUUID) {
    PageReference newRef = new PageReference(notebookName, pageNumber, pageUUID, notebookUUID);
    usages.Add(newRef);
  }


  public void print() {
    Console.WriteLine(this.name);
    foreach (PageReference currentRef in usages) {
      Console.WriteLine(currentRef.prints());
    }
  }


    public TagInfo() {
        this.usages = new List<PageReference>();
    }
}
