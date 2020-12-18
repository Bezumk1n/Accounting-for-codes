using CodesAccounting.Events;
using CodesAccounting.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace CodesAccounting.ViewModel
{
    public class NavigationViewModel
    {
        private EventsAgregator events;
        private readonly CodesAccountingRepository repository;

        public ObservableCollection<Navigation> NavigationTemplates { get; }
        private ICollectionView TemplatesView { get; set; }

        private Navigation selectedItem;
        public Navigation SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                if (selectedItem != null)
                {
                    events.Publish(selectedItem.Id);
                }
            }
        }
        private string filter;
        public string Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                TemplatesView.Refresh();
            }
        }

        public NavigationViewModel(CodesAccountingRepository repository, EventsAgregator events)
        {
            this.events = events;
            this.repository = repository;
            NavigationTemplates = new ObservableCollection<Navigation>();

            TemplatesView = CollectionViewSource.GetDefaultView(NavigationTemplates);
            TemplatesView.Filter += TemplatesFilter;

            LoadTemplatesAsync();
        }

        private bool TemplatesFilter(object obj)
        {
            if (filter == null)
            {
                return true;
            }

            Navigation navigationTemplates = obj as Navigation;

            string[] multipleFilter = null;
            if (filter.Contains('*'))
            {
                multipleFilter = filter.Split('*');
            }

            if (multipleFilter != null)
            {
                bool check;
                for (int i = 0; i < multipleFilter.Length; i++)
                {
                    check = navigationTemplates.DisplayMember.ToLower().Contains(multipleFilter[i]);

                    if (check == false)
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return navigationTemplates.DisplayMember.ToLower().Contains(filter);
            }
        }

        public async void LoadTemplatesAsync()
        {
            var list = await repository.LoadTemplatesAsync();
            var navigationList = list
                .Select(x =>
                new Navigation
                {
                    Id = x.Id,
                    DisplayMember = x.ISBN + " " + x.Title
                })
                .ToList();

            NavigationTemplates.Clear();

            foreach (var item in navigationList)
            {
                NavigationTemplates.Add(item);
            }
        }
    }
}
