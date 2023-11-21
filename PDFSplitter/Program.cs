using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;

public class PDFSplitter
{
    public static void Main(string[] args)
    {
        string inputFilePath = @"C:\Users\hersh\Documents\Projects\cpp\PDFSplitter\PDFSplitter\example.pdf";
        string outputDirectory = @"C:\Users\hersh\Documents\Projects\cpp\PDFSplitter\PDFSplitter\output\";

        using (PdfReader reader = new PdfReader(inputFilePath))
        {
            // Retrieve all the bookmarks
            var bookmarks = SimpleBookmark.GetBookmark(reader);

            if (bookmarks != null)
            {
                // Iterate through each bookmark
                foreach (var bookmark in bookmarks)
                {
                    // Split the bookmark and its children recursively
                    SplitBookmark(reader, bookmark, "", outputDirectory);
                }
            }
        }
    }

    public static string RemoveSpecialCharacters(string str)
    {
        StringBuilder sb = new StringBuilder();
        foreach (char c in str)
        {
            if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }

    private static void SplitBookmark(PdfReader reader, IDictionary<string, object> bookmark, string parentTitle, string outputDirectory)
    {
        // Get the title and page number of the bookmark
        string title = (string)bookmark["Title"];
        int pageNumber = int.Parse(((string)bookmark["Page"]).Split(' ')[0]);

        // Combine the parent title and current title to create a unique filename
        string fileName = RemoveSpecialCharacters(title);

        // Extract the page from the PDF
        Document document = new Document();
        PdfCopy copy = new PdfCopy(document, new FileStream(Path.Combine(outputDirectory, $"{fileName}.pdf"), FileMode.Create));
        document.Open();
        copy.AddPage(copy.GetImportedPage(reader, pageNumber));
        document.Close();

        // Check if the bookmark has children
        if (bookmark.ContainsKey("Kids"))
        {
            var children = (System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>>)bookmark["Kids"];
            foreach (var child in children)
            {
                // Split the child bookmark recursively with the current title as the parent title
                SplitBookmark(reader, child, fileName, outputDirectory);
            }
        }
    }
}
