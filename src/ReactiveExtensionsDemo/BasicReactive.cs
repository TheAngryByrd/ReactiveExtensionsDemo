using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reactive.Linq;
using System.Diagnostics;
using System.Threading;

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
        
        



    }
}
