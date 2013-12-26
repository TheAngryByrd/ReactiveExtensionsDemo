using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reactive.Linq;
using System.Diagnostics;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

namespace ReactiveExtensionsDemo
{
    [TestClass]
    public class BasicReactive
    {
        [TestMethod]
        public void ObservableRange()
        {
            var range = Observable.Range(0, 10);

            range.Subscribe(x => Debug.WriteLine(x),
                            e => Debug.WriteLine(e),
                            () => Debug.WriteLine("finished"));
        }

        [TestMethod]
        public void ObservableTimer()
        {
            var range = Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1)).Timestamp();

            range.Subscribe(x => Debug.WriteLine(x),
                            e => Debug.WriteLine(e),
                            () => Debug.WriteLine("finished"));
            Thread.Sleep(6000);
        }

        [TestMethod]
        public void FromEnumberable()
        {
            var enumberable = new List<int> { 0, 1, 2, 3, 4, 5, };

            var source = enumberable.ToObservable();

            source.Subscribe(x => Debug.WriteLine(x),
                            e => Debug.WriteLine(e),
                            () => Debug.WriteLine("finished"));
        }


    }
}
