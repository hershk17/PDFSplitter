using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfSplitter
{
    public class Class1
    {
        public static void Main()
        {
            // The path of the source PDF file
            string sourcePath = @"C:\Users\hersh\Downloads\sample.pdf";
            // The path of the output folder
            string outputPath = @"C:\Users\hersh\Downloads\Output";

            // Open the source PDF file as a PdfReader
            using (PdfReader reader = new PdfReader(sourcePath))
            {
                // Get the number of pages in the source PDF file
                int pageCount = reader.NumberOfPages;

                // Loop through the pages of the source PDF file
                for (int i = 1; i <= pageCount; i++)
                {
                    // Create a new document for the output PDF file
                    Document document = new Document();
                    // Create a new PdfCopy for copying pages
                    PdfCopy copy = new PdfCopy(document, new FileStream(outputPath + i + ".pdf", FileMode.Create));
                    // Open the document
                    document.Open();
                    // Copy the current page from the source PDF file to the output PDF file
                    copy.AddPage(copy.GetImportedPage(reader, i));
                    // Close the document
                    document.Close();
                }
            }
        }
    }
}
