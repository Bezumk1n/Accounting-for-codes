using CodesAccounting.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace CodesAccounting.Data.FileServices
{
    class ParsingTemplateFile
    {
        public List<Templates> ParseTemplatesFile(string path)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            if (path.EndsWith(".txt"))
            {
                List<Templates> templatesList = new List<Templates>();

                try
                {
                    string[] source = File.ReadAllLines(path, Encoding.GetEncoding(1251));
                    foreach (var item in source)
                    {
                        string[] row = item.Split('\t');
                        if (row[0] != "")
                        {
                            templatesList.Add(new Templates
                            {
                                ISBN = row[0],
                                Title = row[1],
                                Course = row[2],
                                Level = row[3]
                            });
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("При попытке загрузить файл шаблонов возникла ошибка, проверьте пожалуйста шаблон.", "Ошибка загрузки", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                // Возвращаем список без первой строки и без дублей.
                // Проверка по ISBN, берется только первое значение, если такое же попадается далее - игнорируется.
                return templatesList.Skip(1).GroupBy(t => t.ISBN).Select(f => f.First()).ToList();
            }
            else if (path.EndsWith(".json"))
            {
                try
                {
                    string json = File.ReadAllText(path);
                    var templatesList = JsonConvert.DeserializeObject<List<Templates>>(json);
                    return JsonConvert.DeserializeObject<List<Templates>>(json);
                }
                catch
                {
                    MessageBox.Show("При попытке загрузить файл шаблонов возникла ошибка, проверьте пожалуйста шаблон.", "Ошибка загрузки", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
            return null;
        }
    }
}
