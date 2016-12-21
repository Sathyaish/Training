using System;

namespace AirlineScheduleExample
{
    public class Flight
    {
        public FlightScheduleChanged ScheduleChanged;

        public string Carrier { get; set; }
        
        public string FlightNumber { get; set; }

        public string FromPort { get; set; }

        public string DestinationPort { get; set; }

        public DateTime ScheduledDepartureTime { get; set; }

        public DateTime ScheduledArrivalTime { get; set; }

        public DateTime ExpectedDepartureTime { get; protected set; }

        public DateTime ExpectedArrivalTime { get; protected set; }

        public TimeSpan EstimatedDelay { get; set; }
    }
}