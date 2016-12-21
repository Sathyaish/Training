using System;

namespace AirlineScheduleExample
{
    public class FlightScheduleChangedArgs
    {
        public string FlightNumber { get; private set; }
        public DateTime ScheduledDepartureTime { get; private set; }
        public DateTime ScheduledArrivalTime { get; private set; }
        public DateTime ExpectedDepartureTime { get;  private set; }
        public DateTime ExpectedArrivalTime { get; private set; }
        public TimeSpan EstimatedDelay { get; private set; }

        public FlightScheduleChanged(string flightNumber, 
            DateTime scheduledDepartureTime, DateTime scheduledArrivalTime, 
            DateTime expectedDepartureTime, DateTime expectedArrivalTime, 
            TimeSpan estimatedDelay)
        {
            FlightNumber = flightNumber;
            ScheduledDepartureTime = scheduledDepartureTime;
            ScheduledArrivalTime = ScheduledArrivalTime;
            ExpectedDepartureTime = expectedDepartureTime;
            ExpectedArrivalTime = expectedArrivalTime;
            EstimatedDelay = estimatedDelay;
        }
    }
}