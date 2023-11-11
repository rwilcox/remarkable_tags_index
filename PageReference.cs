using System.IO;

public class PageReference : IComparable {
     public string notebookName { get; }

    // while we do most of the reading from (UUID).content files the DATA lives in other files
    // HOWEVER, regardless of the organization of pages or notebooks in the RM they end up flat on the file system
    // (currently)
    private String notebookUUID;

    private int pageNumber;
    private String pageUUID;

    public PageReference(string notebookName, int pageNumber, string pageUUID, string notebookUUID)
    {
        this.notebookName = notebookName;
        this.pageNumber = pageNumber;
        this.pageUUID = pageUUID;
        this.notebookUUID = notebookUUID;
    }

   public int getPageNumber() {
    return this.pageNumber;
   }

    public int humanPageNumber() {
        return this.pageNumber + 1;
    }

    public string relativeRMFileLocation() {
        return Path.Combine(notebookUUID, pageUUID + ".rm");
    }

    public string prints() {
        String humanPageNumber = null;
        if (this.pageNumber == -1) {
            humanPageNumber = "NOT FOUND?!";
        } else {
            humanPageNumber = String.Format("{0:D}", this.pageNumber + 1);
        }
        return ("  * " + this.notebookName + " page " + humanPageNumber);
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
