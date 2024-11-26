using System;
using System.Collections.Generic;

namespace Core
{
    public class Observer
    {
        private static Observer _instance;
        public static Observer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Observer();
                }
                return _instance;
            }
        }
        private Observer() {
            listeners = new Dictionary<EventId, Action<object>>();
        }
        public Dictionary<EventId, Action<object>> listeners;
        public void Register(EventId id, Action<object> action)
        {
            if (!listeners.ContainsKey(id))
            {
                listeners[id] = action;
            }
            else
            {
                listeners[id] += action;
            }
        }

        public void Unregister(EventId id, Action<object> action)
        {
            if (listeners.ContainsKey(id))
            {
                listeners[id] -= action;
            }
        }
        public void Broadcast(EventId id, object data)
        {
            if (listeners.ContainsKey(id))
            {
                listeners[id]?.Invoke(data);
            }

        }
    }
}
