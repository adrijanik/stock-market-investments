using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GraInwestycyjna
{
    public static class TimerExample // In App_Code folder
    {
        static Timer _timer; // From System.Timers
        static List<DateTime> _listOfDates; // Stores timer results
        public static List<DateTime> DateList // Gets the results
        {
            get
            {
                if (_listOfDates == null) // Lazily initialize the timer
                {
                    Start(); // Start the timer
                }
                return _listOfDates; // Return the list of dates
            }
        }
        static void Start()
        {
            _listOfDates = new List<DateTime>(); // Allocate the list
            _timer = new Timer(3000); // Set up the timer for 3 seconds
            //
            // Type "_timer.Elapsed += " and press tab twice.
            //
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            _timer.Enabled = true; // Enable it
        }
        static void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _listOfDates.Add(DateTime.Now); // Add date on each timer event
            
        }
    }
}
