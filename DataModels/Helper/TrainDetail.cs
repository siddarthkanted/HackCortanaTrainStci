using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Helper
{
    public class Fare
    {
        public string cls { get; set; }
        public string fare { get; set; }
    }

    public class Result
    {
        public string trainno { get; set; }
        public string type { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string age { get; set; }
        public string quota { get; set; }
        public List<Fare> fare { get; set; }
    }

    public class TrainFare
    {
        public string status { get; set; }
        public Result result { get; set; }
    }

    public class Seat
    {
        public string date { get; set; }
        public string seat { get; set; }
    }

    public class Result2
    {
        public string trainno { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string cls { get; set; }
        public string quota { get; set; }
        public string error { get; set; }
        public List<Seat> seats { get; set; }
    }

    public class TrainSeatAvailability
    {
        public string status { get; set; }
        public Result2 result { get; set; }
    }

    public class TrainScheduleItem
    {
        public string SerialNumber { get; set; }
        public string StationCode { get; set; }
        public string StationName { get; set; }
        public string ArrivalTime { get; set; }
        public string DepartureTime { get; set; }
        public int Day { get; set; }
        public string Halt { get; set; }
        public string Distance { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string IsHalting { get; set; }
    }

    public class TrainRecord
    {
        public string Provider { get; set; }
        public string TrainName { get; set; }
        public string TrainNumber { get; set; }
        public List<TrainScheduleItem> TrainScheduleItem { get; set; }
    }

    public class DaysRunning
    {
        public List<string> Day { get; set; }
    }

    public class ClassesAvailable
    {
        public List<string> Class { get; set; }
    }

    public class RakeShareTrainNumbers
    {
        public List<string> RakeShareTrainNumber { get; set; }
    }

    public class CoachAttribute
    {
        public string CarNumber { get; set; }
        public string CarType { get; set; }
        public string CarName { get; set; }
    }

    public class CoachComposition
    {
        public List<CoachAttribute> CoachAttributes { get; set; }
    }

    public class TrainAttributes
    {
        public string IsPantryAvailable { get; set; }
        public string IsCateringAvailable { get; set; }
        public string HasPrepaidTaxiService { get; set; }
        public string HasPrepaidAutoService { get; set; }
        public DaysRunning DaysRunning { get; set; }
        public string TrainType { get; set; }
        public string FareType { get; set; }
        public string PairTrainNumber { get; set; }
        public string PairTrainName { get; set; }
        public ClassesAvailable ClassesAvailable { get; set; }
        public RakeShareTrainNumbers RakeShareTrainNumbers { get; set; }
        public CoachComposition CoachComposition { get; set; }
    }

    public class TrainDetails
    {
        public string Provider { get; set; }
        public string TrainName { get; set; }
        public string TrainNumber { get; set; }
        public TrainAttributes TrainAttributes { get; set; }
    }

    public class TrainData
    {
        public TrainFare TrainFare { get; set; }
        public TrainSeatAvailability TrainSeatAvailability { get; set; }
        public TrainRecord TrainRecord { get; set; }
        public TrainDetails TrainDetails { get; set; }
    }
}
