using CodesAccounting.Data.FileServices;
using CodesAccounting.Model;
using Prism.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using System.ComponentModel;
using System.Windows.Data;

namespace CodesAccounting.ViewModel
{
    public class CodesViewModel
    {
        private readonly CodesAccountingRepository repository;

        public ObservableCollection<Codes> Codes { get; set; }
        private ICollectionView CodesView { get; set; }

        public ICommand AddCodesCommand { get; }
        public ICommand ExportCodesCommand { get; }
        public ICommand FindCommand { get; }
        public ICommand UnblockCommand { get; }

        private bool hideUsedCodes = true;
        public bool HideUsedCodes
        {
            get { return hideUsedCodes; }
            set 
            {
                hideUsedCodes = value;
                CodesView.Refresh();
            }
        }
        private string filter = "";
        public string Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                // Сейчас фильтр срабатывает после нажатия кнопки Найти, но можно включить динамический поиск,
                // фильтр будет срабатывать каждый раз при наборе символа:
                //CodesView.Refresh();
            }
        }
        private Codes selectedItem;
        public Codes SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
            }
        }

        public CodesViewModel(CodesAccountingRepository repository)
        {
            this.repository = repository;
            Codes = new ObservableCollection<Codes>();

            CodesView = CollectionViewSource.GetDefaultView(Codes);
            CodesView.Filter += CodesFilter;

            AddCodesCommand = new DelegateCommand(AddCodes);
            ExportCodesCommand = new DelegateCommand(ExportCodes);
            FindCommand = new DelegateCommand(FindButton);
            UnblockCommand = new DelegateCommand(Unblock);

            LoadCodesAsync();
        }

        private void Unblock()
        {
            selectedItem.Active = "Да";
            selectedItem.UseDate = "";
            selectedItem.IsUsed = false;

            CodesView.Refresh();
        }

        private bool CodesFilter(object obj)
        {
            Codes codes = obj as Codes;

            string[] multipleFilter = null;
            if (filter.Contains('*'))
            {
                multipleFilter = filter.Split('*');
            }

            if (hideUsedCodes)
            {
                if (multipleFilter != null)
                {
                    bool check;
                    for (int i = 0; i < multipleFilter.Length; i++)
                    {
                        check = codes.Active.Contains("Да") &&
                            codes.ISBN.ToLower().Contains(multipleFilter[i]) |
                            codes.Title.ToLower().Contains(multipleFilter[i]) |
                            codes.Code.ToLower().Contains(multipleFilter[i]);
                        if (check == false)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    return codes.Active.Contains("Да") &&
                            codes.ISBN.ToLower().Contains(filter) |
                            codes.Title.ToLower().Contains(filter) |
                            codes.Code.ToLower().Contains(filter);
                }
            }
            else
            {
                if (multipleFilter != null)
                {
                    bool check;
                    for (int i = 0; i < multipleFilter.Length; i++)
                    {
                        check = codes.ISBN.ToLower().Contains(multipleFilter[i]) |
                            codes.Title.ToLower().Contains(multipleFilter[i]) |
                            codes.Code.ToLower().Contains(multipleFilter[i]);
                        if (check == false)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    return codes.ISBN.ToLower().Contains(filter) |
                            codes.Title.ToLower().Contains(filter) |
                            codes.Code.ToLower().Contains(filter);
                }
            }
        }

        private void FindButton()
        {
            CodesView.Refresh();
        }               

        private void ExportCodes()
        {
            var exportCodes = Codes.Where(e => e.IsUsed == true && e.Active == "Да").ToList();

            if (exportCodes.Count > 0)
            {
                bool ok = new SaveFile().SaveCodes(exportCodes);
                if (ok)
                {
                    // Пропускаем первую запись, т.к. это шапка
                    exportCodes = exportCodes.Skip(1).ToList();
                    repository.UpdateCodes(exportCodes);

                    CodesView.Refresh();
                }
            }
        }

        private void AddCodes()
        {
            var newCodes = new OpenFiles().OpenCodesFile();
            if (newCodes != null && newCodes.Count > 0)
            {
                bool ok = repository.AddCodes(newCodes);

                if (ok)
                {
                    SaveAll();
                    LoadCodesAsync();
                }
            }
        }

        public async void LoadCodesAsync()
        {
            var codes = await repository.LoadCodesAsync();

            Codes.Clear();

            foreach (var item in codes)
            {
                Codes.Add(item);
            }
        }

        public void SaveAll()
        {
            repository.SaveAll(Codes);
        }
    }
}
