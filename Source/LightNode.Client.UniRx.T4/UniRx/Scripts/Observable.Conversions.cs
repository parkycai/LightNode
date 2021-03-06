﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UniRx
{
    public static partial class Observable
    {
        /// <summary>
        /// Convert to awaitable IEnumerator.
        /// </summary>
        public static IEnumerator ToCoroutine<T>(this IObservable<T> source)
        {
            var running = true;
            source.Subscribe(
                ex => { running = false; },
                () => { running = false; });

            while (running)
            {
                yield return null;
            }
        }

        public static IObservable<T> AsObservable<T>(this IObservable<T> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return Observable.Create<T>(observer => source.Subscribe(observer));
        }

        public static IObservable<T> ToObservable<T>(this IEnumerable<T> source)
        {
            return source.ToObservable(Scheduler.CurrentThread);
        }

        public static IObservable<T> ToObservable<T>(this IEnumerable<T> source, IScheduler scheduler)
        {
            return Observable.Create<T>(observer =>
            {
                IEnumerator<T> e;
                try
                {
                    e = source.GetEnumerator();
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                    return Disposable.Empty;
                }

                var flag = new SingleAssignmentDisposable();

                flag.Disposable = scheduler.Schedule(self =>
                {
                    if (flag.IsDisposed)
                    {
                        e.Dispose();
                        return;
                    }

                    bool hasNext;
                    var current = default(T);
                    try
                    {
                        hasNext = e.MoveNext();
                        if (hasNext) current = e.Current;
                    }
                    catch (Exception ex)
                    {
                        e.Dispose();
                        observer.OnError(ex);
                        return;
                    }

                    if (hasNext)
                    {
                        observer.OnNext(current);
                        self();
                    }
                    else
                    {
                        e.Dispose();
                        observer.OnCompleted();
                    }
                });

                return flag;
            });
        }

        public static IObservable<T> Cast<T>(this IObservable<object> source)
        {
            return source.Select(x => (T)source);
        }

        public static IObservable<T> OfType<T>(this IObservable<object> source)
        {
            return source.Where(x => x is T).Cast<T>();
        }
    }
}