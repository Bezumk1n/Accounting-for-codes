using CodesAccounting.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace CodesAccounting.Data
{
    public class DataSeed
    {
        public void Seed(CodesAccountingDbContext context)
        {
            string filepath = Path.Combine("Templates/Templates.json");

            if (File.Exists(filepath))
            {
                using (StreamReader sr = new StreamReader(filepath))
                {
                    string json = sr.ReadToEnd();
                    List<Templates> templates = JsonConvert.DeserializeObject<List<Templates>>(json);

                    context.AddRange(templates);
                    context.SaveChanges();
                }
            }
        }
    }
}
