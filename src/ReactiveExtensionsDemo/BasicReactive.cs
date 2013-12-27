using System;
using System.Reactive.Subjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reactive.Linq;
using System.Diagnostics;
using System.Threading;
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

        public event EventHandler RegularNetEvent;
        [TestMethod]
        public void DotNetEvent()
        {
            RegularNetEvent += (s,e) => Debug.WriteLine("evoked");
            RegularNetEvent(this, null);       
        } 
        
        [TestMethod]
        public void FromDotNetEvent()
        {
            var reactiveEvent = Observable.FromEventPattern(ev => RegularNetEvent += ev, ev => RegularNetEvent -= ev);
            //var reactiveEvent = Observable.FromEventPattern(this, "RegularNetEvent");
            
            reactiveEvent.Subscribe(x => Debug.WriteLine("evoked"));
            RegularNetEvent(this, null);     
        }

        [TestMethod]
        public void Subject()
        {
            var subject = new Subject<int>();
            subject.Subscribe(x => Debug.WriteLine(x));

            subject.OnNext(0);
            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(0);         
        }

        [TestMethod]
        public void StopNotifying()
        {
            var remote = new Subject<bool>();

            var tvSubscription = remote.Subscribe(x => Debug.WriteLine("TV on: {0}", x));
            var speakerSubscription = remote.Subscribe(x => Debug.WriteLine("Speakers on: {0}", x));

            remote.OnNext(true);
            speakerSubscription.Dispose();

            remote.OnNext(false);
        }

    }
}
