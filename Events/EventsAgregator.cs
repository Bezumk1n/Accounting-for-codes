using System;

namespace CodesAccounting.Events
{
    public class EventsAgregator
    {
        public event Action<int> Subscribe;
        public void Publish(int templateId)
        {
            Subscribe?.Invoke(templateId);
        }
    }
}
