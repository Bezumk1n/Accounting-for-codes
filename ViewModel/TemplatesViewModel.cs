using CodesAccounting.Data.FileServices;
using CodesAccounting.Model;
using Prism.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CodesAccounting.ViewModel
{
    public class TemplatesViewModel
    {
        public ObservableCollection<Templates> Templates { get; }
        private readonly CodesAccountingRepository repository;
        public ICommand ShowTemplatesCommand { get; }
        public ICommand AddTemplatesCommand { get; }
        public ICommand UploadTemplatesCommand { get; }

        public TemplatesViewModel(CodesAccountingRepository repository)
        {
            this.repository = repository;
            Templates = new ObservableCollection<Templates>();
            ShowTemplatesCommand = new DelegateCommand(LoadTemplatesAsync);
            AddTemplatesCommand = new DelegateCommand(AddTemplates);
            UploadTemplatesCommand = new DelegateCommand(UploadTemplates);
        }

        private async void UploadTemplates()
        {
            List<Templates> templatesToSave = await repository.LoadTemplatesAsync();
            new SaveFile().SaveTemplates(templatesToSave);
        }

        private void AddTemplates()
        {
            var newTemplates = new OpenFiles().OpenTemplatesFile();
            if (newTemplates != null && newTemplates.Count > 0)
            {
                repository.AddOrUpdateTemplates(newTemplates);
            }
        }

        public async void LoadTemplatesAsync()
        {
            var templates = await repository.LoadTemplatesAsync();
            Templates.Clear();
            foreach (var item in templates)
            {
                Templates.Add(item);
            }
        }
    }
}
