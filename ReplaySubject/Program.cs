using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

// *****************************************************************************************************//
// *** A subject acts similar to a proxy in that it acts as both a subscriber and a publisher        ***//
// *** It's IObserver interface can be used to subscribe to multiple streams or sequences of data.   ***//
// *** The data is then published through it's IObservable interface.                                ***//
// ***                                                                                               ***//
// *** In this example a simple ReplaySubject is used to subscribe to multiple news feeds            ***//
// *** that provide random news headlines. Before the subject is subscribed to the feeds, we use     ***//
// *** Timestamp operator to timestamp each headline. Subscribers can then subscribe to the subject  ***//
// *** observable interface to observe the data stream(s) or a subset of the stream(s) based on      ***//
// *** time.                                                                                         ***//
// ***                                                                                               ***//
// *** A ReplaySubject buffers items it receives. So a subscription created at a later time can      ***//
// *** access items from the sequence which have already been published.                             ***//
// ***                                                                                               ***//
// *** A subscriptions is created to the ReplaySubject that receives only local news headlines which ***//
// *** occurred 10 seconds before the local news subscription was created. So we basically have the  ***//
// *** ReplaySubject "replay" what happened 10 seconds earlier.                                      ***//
// ***                                                                                               ***//
// *** A local news headline just contains the newsLocation substring ("in your area.").             ***//
// ***                                                                                               ***//
// *****************************************************************************************************//
ReplaySubject<Timestamped<string>> myReplaySubject = new ReplaySubject<Timestamped<string>>();

// *****************************************************************//
// *** Create news feed #1 and subscribe the ReplaySubject to it ***//
// *****************************************************************//
NewsHeadlineFeed newsFeed1 = new NewsHeadlineFeed("Headline News Feed #1");

// Timestamp() timestamps the output from HeadlineFeed, it has a Timestamp property.
newsFeed1.HeadlineFeed.Timestamp().Subscribe(myReplaySubject);

// *****************************************************************//
// *** Create news feed #2 and subscribe the ReplaySubject to it ***//
// *****************************************************************//
NewsHeadlineFeed newsFeed2 = new NewsHeadlineFeed("Headline News Feed #2");
newsFeed2.HeadlineFeed.Timestamp().Subscribe(myReplaySubject);

// *****************************************************************************************************//
// *** Create a subscription to the subject's observable sequence. This subscription will filter for ***//
// *** only local headlines that occurred within the last 10 seconds.                                ***//
// ***                                                                                               ***//
// *** Since we are using a ReplaySubject with timestamped headlines, we can subscribe to the        ***//
// *** headlines already past. The ReplaySubject will "replay" them for the localNewSubscription     ***//
// *** from its buffered sequence of headlines.                                                      ***//
// *****************************************************************************************************//
Console.WriteLine("Waiting for 10 seconds before subscribing to local news headline feed.\n");
Thread.Sleep(10000);

Console.WriteLine($"\n*** Creating local news headline subscription at {DateTime.Now.ToString()} ***\n");
Console.WriteLine(
    "This subscription asks the ReplaySubject for the buffered headlines that\n" +
        "occurred within the last 10 seconds.\n\nPress ENTER to exit.",
    DateTime.Now.ToString());

DateTime lastestHeadlineTime = DateTime.Now;
DateTime earliestHeadlineTime = lastestHeadlineTime - TimeSpan.FromSeconds(10);

IDisposable localNewsSubscription = myReplaySubject.Where(
    x => x.Value.Contains("in your area.") &&
        (x.Timestamp >= earliestHeadlineTime) &&
        (x.Timestamp < lastestHeadlineTime))
    .Subscribe(
        x =>
        {
            Console.WriteLine(
                $"\n************************************\n" +
                        $"***[ Local news headline report ]***\n" +
                        $"************************************\n" +
                        $"Time         : {x.Timestamp.ToString()}\n{x.Value}\n\n");
        });

Console.ReadLine();

// *******************************//
// *** Cancel the subscription ***//
// *******************************//
localNewsSubscription.Dispose();

// *************************************************************************//
// *** Unsubscribe all the ReplaySubject's observers and free resources. ***//
// *************************************************************************//
myReplaySubject.Dispose();
