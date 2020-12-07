using CodesAccounting.Model;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace CodesAccounting.Data.FileServices
{
    public class ParsingCodeFile
    {
        public List<Codes> ParseCodesFile(string[] files)
        {
            List<Codes> codesList = new List<Codes>();

            for (int i = 0; i < files.Length; i++)
            {
                if (!files[i].Contains(".txt"))
                {
                    continue;
                }

                if (files[i].Contains("978") && !files[i].Contains("_formatted"))
                {
                    continue;
                }

                if (files[i].Contains("978") && files[i].Contains("_month_") && files[i].Contains("_formatted"))
                {
                    string isbn = files[i].Substring(files[i].IndexOf("978"));
                    isbn = isbn.Remove(13, isbn.Length - 13);

                    string month = files[i].Substring(files[i].IndexOf("_Gratis_") + 8);
                    month = month.Remove(month.IndexOf("_month_") + 6);

                    string[] codes = File.ReadAllLines(files[i]);
                    for (int j = 0; j < codes.Length; j++)
                    {
                        codesList.Add(new Codes
                        {
                            ISBN = isbn,
                            Code = codes[j],
                            Month = month
                        });
                    }
                    continue;
                }

                if (files[i].Contains("978") && files[i].Contains("_formatted"))
                {
                    string isbn = files[i].Substring(files[i].IndexOf("978"));
                    isbn = isbn.Remove(13, isbn.Length - 13);

                    string[] codes = File.ReadAllLines(files[i]);
                    for (int j = 0; j < codes.Length; j++)
                    {
                        codesList.Add(new Codes
                        {
                            ISBN = isbn,
                            Code = codes[j],
                        });
                    }
                }
                else
                {
                    string[] codes = File.ReadAllLines(files[i]);
                    string isbn = null;

                    // Пытаемся найти ISBN внутри файла
                    for (int j = 0; j < codes.Length; j++)
                    {
                        if (codes[j].Contains("Notes") && codes[j].Contains("978"))
                        {
                            isbn = codes[j].Replace("-", "");
                            isbn = isbn.Substring(isbn.IndexOf("978"));
                            isbn = isbn.Remove(13, isbn.Length - 13);
                            break;
                        }
                    }

                    if (isbn != null)
                    {
                        int startIndex = 0;
                        for (int j = 0; j < codes.Length; j++)
                        {
                            if (codes[j].Contains("---"))
                            {
                                startIndex = j;
                                break;
                            }
                        }
                        for (int j = startIndex + 1; j < codes.Length; j++)
                        {
                            codesList.Add(new Codes
                            {
                                ISBN = isbn,
                                Code = codes[j]
                            });
                        }
                    }

                    // Если ISBN внутри не нашли:
                    else
                    {
                        int startIndex = 0;
                        for (int j = 0; j < codes.Length; j++)
                        {
                            if (codes[j].Contains("---"))
                            {
                                startIndex = j;
                                break;
                            }
                        }
                        if (startIndex > 0)
                        {
                            if (codes[0].Contains("Family and Friends"))
                            {
                                for (int j = startIndex + 1; j < codes.Length; j++)
                                {
                                    codesList.Add(new Codes { ISBN = "Family & Friends без ISBN", Code = codes[j] });
                                }
                            }
                            else if (codes[0].Contains("English File 4e"))
                            {
                                for (int j = startIndex + 1; j < codes.Length; j++)
                                {
                                    codesList.Add(new Codes { ISBN = "English File 4ED без ISBN", Code = codes[j] });
                                }
                            }
                            else if (codes[0].Contains("Headway 5e"))
                            {
                                for (int j = startIndex + 1; j < codes.Length; j++)
                                {
                                    codesList.Add(new Codes { ISBN = "Headway 5ED без ISBN", Code = codes[j] });
                                }
                            }
                            else if (codes[0].Contains("Oxford Discover Futures"))
                            {
                                for (int j = startIndex + 1; j < codes.Length; j++)
                                {
                                    codesList.Add(new Codes { ISBN = "Oxford Discover Futures без ISBN", Code = codes[j] });
                                }
                            }
                            // Далее можно добавлять иные правила определения книг без явно указанного ISBN
                            // Не забудь сначала добавить новый ISBN в базу данных и в файл Templates.json (чтобы шаблон сразу добавлялся при создании базы)!
                            //else if (codes[0].Contains("Какое-то название"))
                            //{
                            //    for (int j = startIndex + 1; j < temp.Length; j++)
                            //    {
                            //        codesList.Add(new CodesModel { ISBN = "EF без ISBN", Code = codes[j] });
                            //    }
                            //}
                            else
                            {
                                MessageBox.Show("Нет модели для файла:\n" +
                                    $"{files[i]} \n" +
                                    $"Добавление кодов прервано.", "Ошибка добавления", MessageBoxButton.OK, MessageBoxImage.Error);
                                return null;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Нет модели для файла:\n" +
                                $"{files[i]} \n" +
                                $"Добавление кодов прервано.", "Ошибка добавления", MessageBoxButton.OK, MessageBoxImage.Error);
                            return null;
                        }
                    }
                }
            }
            return codesList;
        }
    }
}
