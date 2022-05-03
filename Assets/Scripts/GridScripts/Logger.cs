using System.Collections.Generic;

namespace GameEngineProfiler
{
    public interface ILoggable
    {
        /// <summary>
        /// Log method.
        /// </summary>
        /// <returns></returns>
        public string Log();
    };

    public abstract class Observer
    {
        public abstract void Update(ILoggable iLog);
    };

    public class LogObserver : Observer {
        
        /// <summary>
        /// Observed subject
        /// </summary>
        private Subject _subject;

        public LogObserver(Subject s)
        {
            _subject = s;
            _subject.Attach(this);
        }

        ~LogObserver()
        {
            _subject.Detach(this);
        }

        public override void Update(ILoggable iLog)
        {
            
        }
    }
    
    public class Subject
    {
        private List<Observer> _observers;

        public void Attach(Observer o)
        {
            _observers.Add(o);
        }

        public void Detach(Observer o)
        {
            _observers.Remove(o);
        }

        public void Notify(ILoggable iLog)
        {
            foreach (Observer o in _observers)
            {
                o.Update(iLog);
            }
        }
    };
}