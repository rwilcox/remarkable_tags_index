public class PageReference : IComparable {
    private String notebookName;
    private int pageNumber;

    public PageReference(string notebookName, int pageNumber)
    {
        this.notebookName = notebookName;
        this.pageNumber = pageNumber;
    }

   public int getPageNumber() {
    return this.pageNumber;
   }

    public void print() {
        String humanPageNumber = null;
        if (this.pageNumber == -1) {
            humanPageNumber = "NOT FOUND?!";
        } else {
            humanPageNumber = String.Format("{0:D}", this.pageNumber + 1);
        }
        Console.WriteLine("  * " + this.notebookName + " page " + humanPageNumber);
    }

    int IComparable.CompareTo(object? inObj) {
        PageReference obj = (PageReference)(inObj);
        if (obj == null) return 1;

        if (this.pageNumber == obj.getPageNumber()) {
            return 0;
        }

        if (this.pageNumber < obj.getPageNumber()) {
            return -1;
        }

        if (this.pageNumber > obj.getPageNumber()) {
            return 1;
        }

        return 0;
    }
}
