using System;

namespace CodesAccounting.Events
{
    public class EventsAgregator
    {
        public event Action<int> SelectedTemplateIsChanged;
        public void TemplateIsChanged(int templateId)
        {
            SelectedTemplateIsChanged?.Invoke(templateId);
        }
    }
}
