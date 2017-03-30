using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;

namespace CJR_NewBugTracker.Models
{
    public static class PdfUploadValidator
    {
        public static object PdfFormatException { get; private set; }

        public static bool IsWebFriendlyPdf(HttpPostedFileBase file)
        {

            // check for actual object
            if (file == null)
                return false;
            // check size - file must be less than 2 MB and greater than 1 KB
            if (file.ContentLength > 67108864)
                return false;
            string fileExt = Path.GetExtension(file.FileName).ToLower();
            if (fileExt == ".pdf" || fileExt == ".doc" || fileExt == ".xls")
            {
                return true;
            }
            else
            {
                return false;

            }
        }
    }
}