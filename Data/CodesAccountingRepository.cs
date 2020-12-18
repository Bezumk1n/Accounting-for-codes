using CodesAccounting.Data;
using CodesAccounting.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Windows;
using System;
using System.Collections.ObjectModel;

namespace CodesAccounting.ViewModel
{
    public class CodesAccountingRepository
    {
        public async Task<List<Codes>> LoadCodesAsync()
        {
            using (CodesAccountingDbContext context = new CodesAccountingDbContext())
            {
                return await Task.Run(() => context.Codes.OrderBy(i => i.Title).ToList());
            }
        }
        public async Task<List<Templates>> LoadTemplatesAsync()
        {
            using (CodesAccountingDbContext context = new CodesAccountingDbContext())
            {
                return await Task.Run(() => context.Templates.OrderBy(i => i.Title).ToList());
            }
        }
        public void AddOrUpdateTemplates(List<Templates> newTemplates)
        {
            using (CodesAccountingDbContext context = new CodesAccountingDbContext())
            {
                int added = 0;
                int updated = 0;
                foreach (var item in newTemplates)
                {
                    Templates entity = context.Templates.FirstOrDefault(db => db.ISBN == item.ISBN);
                    if (entity != null)
                    {
                        entity.Title = item.Title;
                        entity.Course = item.Course;
                        entity.Level = item.Level;
                
                        context.Templates.Update(entity);
                        updated++;
                    }
                    else
                    {
                        context.Templates.Add(item);
                        added++;
                    }
                }
                context.SaveChanges();
                MessageBox.Show($"Добавлено {added} шаблонов\n" +
                    $"Обновлено {updated} шаблонов.");
            }
        }
        public bool AddCodes(List<Codes> newCodes)
        {
            using (CodesAccountingDbContext context = new CodesAccountingDbContext())
            {
                int count = 0;
                foreach (Codes item in newCodes)
                {
                    Codes entity = context.Codes.FirstOrDefault(db => db.Code == item.Code);
                    if (entity == null)
                    {
                        Templates model = context.Templates.FirstOrDefault(db => db.ISBN == item.ISBN);
                        if (model != null)
                        {
                            context.Codes.Add(new Codes
                            {
                                ISBN = item.ISBN,
                                Title = model.Title,
                                Course = model.Course,
                                Level = model.Level,
                                Code = item.Code,
                                Month = item.Month,
                                AddDate = DateTime.Now.ToString("yyyy-MM-dd HH-mm"),
                                Active = "Да",
                                IsUsed = false,
                                Comments = "",
                                TemplateId = model.Id
                            });
                            count++;
                        }
                        else
                        {
                            MessageBox.Show($"ISBN {item.ISBN} отсутствует в базе, проверьте пожалуйста шаблон.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Код {item.Code} уже содержится в базе. Не добавлено.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                context.SaveChanges();
                if (count > 0)
                {
                    MessageBox.Show($"Добавлено {count} кодов.", "Коды", MessageBoxButton.OK, MessageBoxImage.Information);
                    return true;
                }
                return false;
            }
        }
        public void UpdateCodes(List<Codes> codes)
        {
            using (CodesAccountingDbContext context = new CodesAccountingDbContext())
            {

                foreach (var code in codes)
                {
                    code.UseDate = DateTime.Now.ToString("yyyy-MM-dd HH-mm");
                    code.IsUsed = true;
                    code.Active = "Нет";
                }
                context.Codes.UpdateRange(codes);
                context.SaveChanges();
            }
        }
        public void SaveAll(ObservableCollection<Codes> codes)
        {
            using (CodesAccountingDbContext context = new CodesAccountingDbContext())
            {
                context.Codes.UpdateRange(codes);
                context.SaveChanges();
            }
        }
    }
}