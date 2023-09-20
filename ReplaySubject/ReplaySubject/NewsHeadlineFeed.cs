

//*********************************************************************************//
//***                                                                           ***//
//*** The NewsHeadlineFeed class is just a mock news feed in the form of an     ***//
//*** observable sequence in Reactive Extensions.                               ***//
//***                                                                           ***//
//*********************************************************************************//

using System.Reactive.Concurrency;
using System.Reactive.Linq;

internal class NewsHeadlineFeed
{
    //*** A list of predefined news events to combine with a simple location string ***//
    private static readonly string[] newsEvents =
    {
        "A tornado occurred ",
        "Weather watch for snow storm issued ",
        "A robbery occurred ",
        "We have a lottery winner ",
        "An earthquake occurred ",
        "Severe automobile accident "
    };

    //*** A list of predefined location strings to combine with a news event. ***//
    private static readonly string[] newsLocations =
    {
        "in your area.",
        "in Dallas, Texas.",
        "somewhere in Iraq.",
        "Lincolnton, North Carolina",
        "Redmond, Washington"
    };
    private string feedName; // Feedname used to label the stream
    private IObservable<string> headlineFeed; //The actual data stream
    private Random rand = new Random(); // Used to stream random headlines.

    public NewsHeadlineFeed(string name)
    {
        feedName = name;

        //*****************************************************************************************//
        //*** Using the Generate operator to generate a continuous stream of headline that occur ***//
        //*** randomly within 5 seconds.                                                        ***//
        //*****************************************************************************************//
        headlineFeed = Observable.Generate(
            // the initial string value
            RandNewsEvent(),
            // the resulting string is passed here, condition to terminate the loop
            evt => true,
            // the iterator or the loop called after the above condition is checked
            evt => RandNewsEvent(),
            // the selector function, gives you the opportunity to change the value that the iterator function spat out.
            evt =>
            {
                Thread.Sleep(rand.Next(3000));
                return evt;
            },
            // the scheduler on which this observable is executed.
            Scheduler.ThreadPool);
    }

    public IObservable<string> HeadlineFeed { get { return headlineFeed; } }


    //****************************************************************//
    //*** Some very simple formatting of the headline event string ***//
    //****************************************************************//
    private string RandNewsEvent()
    {
        return "Feedname     :" +
            $" {feedName}\nHeadline     :" + 
            $" {newsEvents[rand.Next(newsEvents.Length)]}{newsLocations[rand.Next(newsLocations.Length)]}";
    }
}
