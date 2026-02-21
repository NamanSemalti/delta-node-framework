using System;
using System.Collections.Generic;
using CardMatch.Core.Interfaces;

namespace CardMatch.Infrastructure.EventBus
{
    public sealed class SimpleEventBus : IEventBus
    {
        private readonly Dictionary<Type, Delegate> _handlers = new();

        public void Subscribe<T>(Action<T> handler)
        {
            var type = typeof(T);

            if (_handlers.TryGetValue(type, out var existing))
                _handlers[type] = Delegate.Combine(existing, handler);
            else
                _handlers[type] = handler;
        }

        public void Unsubscribe<T>(Action<T> handler)
        {
            var type = typeof(T);

            if (!_handlers.TryGetValue(type, out var existing))
                return;

            var current = Delegate.Remove(existing, handler);

            if (current == null)
                _handlers.Remove(type);
            else
                _handlers[type] = current;
        }

        public void Publish<T>(T evt)
        {
            if (_handlers.TryGetValue(typeof(T), out var del))
                ((Action<T>)del)?.Invoke(evt);
        }
    }
}