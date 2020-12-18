using System;

namespace CodesAccounting.Events
{
    public class EventsAgregator
    {
        public event Action<object> Subscribe;
        public void Publish(object obj)
        {
            Subscribe?.Invoke(obj);
        }
    }
}
