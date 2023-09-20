// *********************************************************************************//
// ***                                                                           ***//
// *** The NewsHeadlineFeed class is just a mock news feed in the form of an     ***//
// *** observable sequence in Reactive Extensions.                               ***//
// ***                                                                           ***//
// *********************************************************************************//
using System.Reactive.Concurrency;
using System.Reactive.Linq;

internal class NewsHeadlineFeed
{
    // *** A list of predefined news events to combine with a simple location string ***//
    private static readonly string[] NewsEvents =
    {
        "A tornado occurred ",
        "Weather watch for snow storm issued ",
        "A robbery occurred ",
        "We have a lottery winner ",
        "An earthquake occurred ",
        "Severe automobile accident ",
    };

    // *** A list of predefined location strings to combine with a news event. ***//
    private static readonly string[] NewsLocations =
    {
        "in your area.",
        "in Dallas, Texas.",
        "somewhere in Iraq.",
        "Lincolnton, North Carolina",
        "Redmond, Washington",
    };

    private string feedName; // Feedname used to label the stream
    private IObservable<string> headlineFeed; // The actual data stream
    private Random rand = new Random(); // Used to stream random headlines.

    public NewsHeadlineFeed(string name)
    {
        feedName = name;

        // *****************************************************************************************//
        // *** Using the Generate operator to generate a continuous stream of headline that occur ***//
        // *** randomly within 5 seconds.                                                        ***//
        // *****************************************************************************************//
        headlineFeed = Observable.Generate(
            RandNewsEvent(),
            evt => true,
            evt => RandNewsEvent(),
            evt =>
            {
                Thread.Sleep(rand.Next(3000));
                return evt;
            });
    }

    public IObservable<string> HeadlineFeed
    {
        get { return headlineFeed; }
    }

    // ****************************************************************//
    // *** Some very simple formatting of the headline event string ***//
    // ****************************************************************//
    private string RandNewsEvent()
    {
        return "Feedname     :" +
            $" {feedName}\nHeadline     :" +
            $" {NewsEvents[rand.Next(NewsEvents.Length)]}{NewsLocations[rand.Next(NewsLocations.Length)]}";
    }
}
