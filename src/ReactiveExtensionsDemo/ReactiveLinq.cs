using System;
using System.Linq;
using System.Reactive.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reactive.Subjects;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Reactive.Testing;
using System.Reactive.Concurrency;
using System.Threading;

namespace ReactiveExtensionsDemo
{
    [TestClass]
    public class ReactiveLinq
    {
        [TestMethod]
        public void Where()
        {
            var subject = new Subject<string>();

            subject.Where(x => x.Length > 3).Subscribe(x => Debug.WriteLine(x));

            subject.OnNext("telerik");
            subject.OnNext("has");
            subject.OnNext("a");
            subject.OnNext("party");
        }

        [TestMethod]
        public void Select()
        {
            var subject = new Subject<string>();

            subject.Select(x => x.Length).Subscribe(x => Debug.WriteLine(x));
            

            subject.OnNext("telerik");
            subject.OnNext("has");
            subject.OnNext("a");
            subject.OnNext("party");
        }

        [TestMethod]
        public void Merge()
        {
            var source1 = Observable.Repeat(1, 3);
            var source2 = Observable.Repeat(2, 3);

            //Merge - returns the two streams merged basing of when each source returns its value
            var mergedSource = source1.Merge(source2);

            mergedSource.Subscribe(x => Debug.WriteLine(x));
        }   

        [TestMethod]
        public void Zip()
        {
            var source1 = Observable.Repeat(1, 4);
            var source2 = Observable.Repeat(2, 3);

            //Zip - returns values from one stream paired with values from another, 
            //      only when a couple is available.
            var mergedSource = source1.Zip(source2, /*Map or Reduce*/ (x, y) => x + y);

            mergedSource.Subscribe(x => Debug.WriteLine(x));
        }

        [TestMethod]
        public void Retry()
        {
            var eventStream = Observable.Range(0,3).Concat(Observable.Throw<int>(new Exception()));

            eventStream.Retry(2).Subscribe(x => Debug.WriteLine(x),
                                           e => Debug.WriteLine(e));
        }


        private IObservable<string> EndlessBarrageOfEmail(IScheduler sched = null)
        {
            sched = sched ?? Scheduler.CurrentThread;
            var random = new Random();
            var emails = new List<String> { "Here is an email!", "Another email!", "Yet another email!" };
            return Observable.Timer(TimeSpan.FromSeconds(0), TimeSpan.FromMilliseconds(random.Next(1000)), sched)
                .Select(_ => emails[random.Next(emails.Count)]);

        }

        [TestMethod]
        public void Buffer()
        {
            var myInbox = EndlessBarrageOfEmail();

            var getMailEveryThreeSeconds = myInbox.Buffer(TimeSpan.FromSeconds(3));

            getMailEveryThreeSeconds.Subscribe(emails =>
            {
                Debug.WriteLine("You've got {0} new messages!  Here they are!", emails.Count());
                foreach (var email in emails)
                {
                    Debug.WriteLine("> {0}", email);
                }
                Debug.WriteLine("");
            });
        }

        [TestMethod]
        public void BufferAsync()
        {
            var myInbox = EndlessBarrageOfEmail(Scheduler.TaskPool);

            var getMailEveryThreeSeconds = myInbox.Buffer(TimeSpan.FromSeconds(3), Scheduler.TaskPool);

            getMailEveryThreeSeconds.Subscribe(emails =>
            {
                Debug.WriteLine("You've got {0} new messages!  Here they are!", emails.Count());
                foreach (var email in emails)
                {
                    Debug.WriteLine("> {0}", email);
                }
                Debug.WriteLine("");
            });
            Thread.Sleep(8000);
        }

        [TestMethod]
        public void TestScheduler()
        {
            //This is how you become a Timelord
            var sched = new TestScheduler();

            var myInbox = EndlessBarrageOfEmail(sched);

            var getMailEveryThreeSeconds = myInbox.Buffer(TimeSpan.FromSeconds(3), sched);

            getMailEveryThreeSeconds.Subscribe(emails =>
            {
                Debug.WriteLine("You've got {0} new messages!  Here they are!", emails.Count());
                foreach (var email in emails)
                {
                    Debug.WriteLine("> {0}", email);
                }
                Debug.WriteLine("");
            });
            //sched.Start(() => getMailEveryThreeSeconds);
            sched.AdvanceBy(TimeSpan.FromMilliseconds(100).Ticks);
            sched.AdvanceBy(TimeSpan.FromMilliseconds(3500).Ticks);

            sched.AdvanceBy(TimeSpan.FromMilliseconds(30500).Ticks);
        }
    }
}
