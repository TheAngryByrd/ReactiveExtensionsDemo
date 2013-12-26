using System;
using System.Linq;
using System.Reactive.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reactive.Subjects;
using System.Diagnostics;
using System.Collections.Generic;
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

        [TestMethod]
        public void Buffer()
        {
            var myInbox = EndlessBarrageOfEmail().ToObservable();

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

        private IEnumerable<string> EndlessBarrageOfEmail()
        {
            var random = new Random();
            var emails = new List<String> { "Here is an email!", "Another email!", "Yet another email!" };
            for (int i = 0 ; i<20 ; i ++ )
            {
                // Return some random emails at random intervals.
                yield return emails[random.Next(emails.Count)];
                Thread.Sleep(random.Next(1000));
            }
        }
    }
}
