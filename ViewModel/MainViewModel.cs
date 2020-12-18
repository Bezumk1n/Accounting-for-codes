using CodesAccounting.Events;

namespace CodesAccounting.ViewModel
{
    public class MainViewModel
    {
        private readonly CodesAccountingRepository repository;

        private readonly EventsAgregator events;

        public CodesViewModel CodesViewModel { get; }
        public TemplatesViewModel TemplatesViewModel { get; }
        public NavigationViewModel NavigationViewModel { get; }

        public MainViewModel()
        {
            repository = new CodesAccountingRepository();
            events = new EventsAgregator();
            CodesViewModel = new CodesViewModel(repository, events);
            TemplatesViewModel = new TemplatesViewModel(repository);
            NavigationViewModel = new NavigationViewModel(repository, events);
        }
        public void SaveAll()
        {
            CodesViewModel.SaveAll();
        }
    }
}
