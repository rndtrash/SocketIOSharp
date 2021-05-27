using System;
using System.Collections.Generic;

namespace EngineIOSharp
{
    public abstract class EventHandler
    {
        /// <summary><c>Events</c> stores all event handlers</summary>
        protected Dictionary<string, EventHandler<EventArgs>> Events = new Dictionary<string, EventHandler<EventArgs>>();

        ~EventHandler()
        {
            Off();
        }

        /// <summary>
        /// <c>On</c> method adds an event handler to given event
        /// </summary>
        /// <param name="eventName">Name of event</param>
        /// <param name="d">Events handler</param>
        public void On(string eventName, EventHandler<EventArgs> d)
        {
            if (!Events.ContainsKey(eventName))
                Events.Add(eventName, d);
            else
            {
                Events[eventName] += d;
            }
        }

        public void Once(string eventName, EventHandler<EventArgs> d)
        {
            EventHandler<EventArgs> eh = null;
            eh = (sender, args) =>
            {
                d(sender, args);
                Off(eventName, eh);
            };
            
            On(eventName, eh);
        }

        public void Off()
        {
            foreach (var eventName in Events.Keys)
            {
                Events[eventName] = null;
            }
            
            Events.Clear();
        }
        
        public void Off(string eventName)
        {
            if (!Events.ContainsKey(eventName))
                return;

            Events[eventName] = null; // FIXME: is it necessary?
            Events.Remove(eventName);
        }

        public void Off(string eventName, EventHandler<EventArgs> d)
        {
            if (!Events.ContainsKey(eventName))
                return;

            Events[eventName] -= d;
            if (Events[eventName].GetInvocationList().Length == 0)
            {
                Events[eventName] = null;
                Events.Remove(eventName);
            }
        }

        public void Emit(string eventName, EventArgs e)
        {
            if (!Events.ContainsKey(eventName) || Events[eventName] == null)
                return;
            
            Events[eventName].Invoke(this, e);
        }
    }
}