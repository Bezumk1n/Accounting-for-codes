namespace CodesAccounting.ViewModel
{
    public class MainViewModel
    {
        private readonly CodesAccountingRepository repository;
        public CodesViewModel CodesViewModel { get; }
        public TemplatesViewModel TemplatesViewModel { get; }
        public MainViewModel()
        {
            repository = new CodesAccountingRepository();
            CodesViewModel = new CodesViewModel(repository);
            TemplatesViewModel = new TemplatesViewModel(repository);
        }
        public void SaveAll()
        {
            CodesViewModel.SaveAll();
        }
    }
}
