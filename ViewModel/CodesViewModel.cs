using CodesAccounting.Data.FileServices;
using CodesAccounting.Model;
using Prism.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using System.ComponentModel;
using System.Windows.Data;
using CodesAccounting.Events;

namespace CodesAccounting.ViewModel
{
    public class CodesViewModel : ViewModelBase
    {
        private readonly CodesAccountingRepository repository;

        public ObservableCollection<Codes> Codes { get; set; }
        private ICollectionView CodesView { get; set; }

        public ICommand AddCodesCommand { get; }
        public ICommand ExportCodesCommand { get; }
        public ICommand FindCommand { get; }
        public ICommand UnblockCommand { get; }

        private bool hideUsedCodes = false;
        public bool HideUsedCodes
        {
            get { return hideUsedCodes; }
            set 
            {
                hideUsedCodes = value;
                CodesView.Refresh();
            }
        }
        private int selectedTemplateId;
        private string filter = "";
        public string Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                OnPropertyChanged();
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

        public CodesViewModel(CodesAccountingRepository repository, EventsAgregator events)
        {
            events.Subscribe += Events_Subscribe;
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

        private void Events_Subscribe(object obj)
        {
            selectedTemplateId = (int)obj;
            CodesView.Refresh();
        }

        private void Unblock()
        {
            if (selectedItem != null && selectedItem.Active == "Нет")
            {
                selectedItem.Active = "Да";
                selectedItem.UseDate = "";
                selectedItem.IsUsed = false;

                CodesView.Refresh();
            }
        }

        private bool CodesFilter(object obj)
        {
            Codes codes = obj as Codes;

            if (hideUsedCodes)
            {
                if (filter != "")
                {
                    return codes.Code.Contains(filter) && codes.Active.Contains("Да");
                }

                return codes.TemplateId == selectedTemplateId && codes.Active.Contains("Да");
            }
            else
            {
                if (filter != "")
                {
                    return codes.Code.Contains(filter);
                }

                return codes.TemplateId == selectedTemplateId;
            }
        }

        private void FindButton()
        {
            CodesView.Refresh();
            Filter = "";
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
            foreach (var item in Codes)
            {
                if (item.IsUsed == true && item.Active == "Да")
                {
                    item.IsUsed = false;
                }
            }
            repository.SaveAll(Codes);
        }
    }
}
