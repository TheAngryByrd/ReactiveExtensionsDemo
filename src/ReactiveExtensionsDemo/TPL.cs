using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveExtensionsDemo
{
    [TestClass]
    public class TPL
    {
        [TestMethod]
        public async Task Rx1()
        {
            var tweetList = await Observable.Range(0, 3)
                     .SelectMany(async page => await GetTweets(page))
                     .Aggregate(new List<Tweet>(), (acc, items) =>
                     {
                         acc.AddRange(items);
                         return acc;
                     });
        }

        [TestMethod]
        public async Task Rx2()
        {
            var tweetList = await Observable.Range(0, 3)
                     .SelectMany(page =>
                            Observable.Defer(() => GetTweets(page).ToObservable())
                                      .Retry(3)
                                      .Catch(Observable.Empty<IEnumerable<Tweet>>()) //swallow the exception
                                      )
                     .Aggregate(new List<Tweet>(), (acc, items) =>
                     {
                         acc.AddRange(items);
                         return acc;
                     });
        }

        [TestMethod]
        public async Task TPL1()
        {
            var tweetList = new List<Tweet>();
            for (int page = 0; page < 3; page++)
            {
                var tweets = await GetTweets(page);
                tweetList.AddRange(tweets);
            }
        }

        [TestMethod]
        public async Task TPL2()
        {
            var tweetListTasks = Enumerable.Range(0, 3)
                                           .Select(page => GetTweets(page));
            var results = await Task.WhenAll(tweetListTasks);
            var tweetList = results.SelectMany(x => x).ToList();
        }

        int count = 0;

        public async Task<IEnumerable<Tweet>> GetTweets(int page)
        {
            #region THIS IS SECRET DON'T LOOK
            if (page == 1 && count == 0)
            {
                count++;
                throw new Exception();
            }                
            #endregion

            await Task.Delay(2000);
            var tweets = Enumerable.Range(0, page * 3 + 1).Select(_ => new Tweet());
            return tweets;
        } 
    }

    public class Tweet
    {

    }
}
