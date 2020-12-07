using CodesAccounting.Model;
using Microsoft.Win32;
using System.Collections.Generic;

namespace CodesAccounting.Data.FileServices
{
    public class OpenFiles
    {
        public List<Templates> OpenTemplatesFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "TXT files(*.txt)|*.txt|JSON files(*.json)|*.json";

            if (ofd.ShowDialog() == true)
            {
                ParsingTemplateFile ptf = new ParsingTemplateFile();
                var templatesList = ptf.ParseTemplatesFile(ofd.FileName);
                return templatesList;
            }
            return null;
        }
        public List<Codes> OpenCodesFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "TXT files(*.txt)|*.txt";

            if (ofd.ShowDialog() == true)
            {
                ParsingCodeFile pcf = new ParsingCodeFile();
                var codesList = pcf.ParseCodesFile(ofd.FileNames);
                return codesList;
            }
            return null;
        }
    }
}
