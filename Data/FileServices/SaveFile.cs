using CodesAccounting.Model;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CodesAccounting.Data.FileServices
{
    public class SaveFile
    {
        public void SaveTemplates(List<Templates> templates)
        {
            var json = JsonConvert.SerializeObject(templates);

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = ".json";
            if (sfd.ShowDialog() == true)
            {
                using (StreamWriter sw = new StreamWriter(sfd.FileName))
                {
                    sw.Write(json);
                    sw.Close();
                }                
            }
        }

        public bool SaveCodes(List<Codes> codes)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd HH-mm");
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "Коды " + date + ".xls";

            if (sfd.ShowDialog() == true)
            {
                Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.GetEncoding(1251)))
                {
                    codes.Insert(0, new Codes
                    {
                        ISBN = "ISBN",
                        Title = "Наименование",
                        Code = "Код",
                        Month = "Срок использования после активации",
                    });

                    foreach (var code in codes)
                    {
                        sw.Write(code.ISBN + '\t');
                        sw.Write(code.Title + '\t');
                        sw.Write(code.Code + '\t');
                        sw.Write(code.Month);
                        sw.WriteLine();
                    }

                    sw.Close();

                    return true;
                }
            }
            return false;
        }
    }
}
