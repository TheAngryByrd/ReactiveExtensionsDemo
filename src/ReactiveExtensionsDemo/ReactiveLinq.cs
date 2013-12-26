using System;
using System.Reactive.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reactive.Subjects;
using System.Diagnostics;

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
    }
}
