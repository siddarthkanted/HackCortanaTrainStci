using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataModels.Helper
{
    public class TrainDataController
    {
        private Trains trains;

        public TrainDataController()
        {
            trains = ConvertTrainsXmlToTrainsObject();
        }

        //http://azurehacktrainwebapplication.azurewebsites.net/getTrainListOnStationCode?fromStationCode=cbe&toStationCode=hyb
        //http://localhost:64548/getTrainListOnStationCode?fromStationCode=cbe&toStationCode=hyb
     
        public List<Train> GetTrainListOnStationCode(string fromStationCode, string toStationCode)
        {
            //trains which are passing fromStation and ToStation
            List<Train> trainList =
                trains.Train.FindAll(
                    x => x.TrainRecord.TrainScheduleItem.Exists(y => y.StationCode.Equals(fromStationCode)) &&
                         x.TrainRecord.TrainScheduleItem.Exists(y => y.StationCode.Equals(toStationCode)));

            //fromStation DayTime should be less than  ToStation DayTime
            trainList =
                trainList.FindAll(x =>
                    (IsFromStationDayTimeLessThanToStationDayTime(
                        x.TrainRecord.TrainScheduleItem.Find(y => y.StationCode.Equals(fromStationCode)),
                        x.TrainRecord.TrainScheduleItem.Find(y => y.StationCode.Equals(toStationCode)))));

            //AddTrainFare_SeatAvailability(trainList, fromStationCode, toStationCode);

            return trainList;
        }


        //private void AddTrainFare_SeatAvailability(List<Train> trainList, string fromStationCode, string toStationCode)
        //{
        //    ErailApi erailApi = new ErailApi(trainList, fromStationCode, toStationCode);
        //    erailApi.AddTrainFare_SeatAvailability();
        //}


        private bool IsFromStationDayTimeLessThanToStationDayTime(TrainScheduleItem fromStationTrainScheduleItem, TrainScheduleItem toStationTrainScheduleItem)
        {
            if (fromStationTrainScheduleItem.DepartureTime.Equals("Destination")) return false;
            if (toStationTrainScheduleItem.DepartureTime.Equals("Destination")) return true;
            if (fromStationTrainScheduleItem.Day > toStationTrainScheduleItem.Day) return false;
            if (fromStationTrainScheduleItem.Day < toStationTrainScheduleItem.Day) return true;

            TimeSpan fromStationDepatureTime = DateTime.Parse(fromStationTrainScheduleItem.DepartureTime).TimeOfDay;
            TimeSpan toStationDepatureTime = DateTime.Parse(toStationTrainScheduleItem.DepartureTime).TimeOfDay;

            return TimeSpan.Compare(fromStationDepatureTime, toStationDepatureTime) < 0;
        }

        //http://azurehacktrainwebapplication.azurewebsites.net/getTrainOnTrainNumber?trainNumber=12647
        //http://localhost:64548/getTrainOnTrainNumber?trainNumber=12647
        public Train GetTrainOnTrainNumber(string trainNumber)
        {
            Train train = trains.Train.Find(x => x.TrainDetails.TrainNumber.Equals(trainNumber));
            return train;
        }

        private static StreamReader ReadFile(string filePath)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = filePath;
            Stream stream = assembly.GetManifestResourceStream(resourceName);
            StreamReader reader = new StreamReader(stream);
            return reader;
        }

        public Trains ConvertTrainsXmlToTrainsObject()
        {
            XmlSerializer trainsXmlSerializer = new XmlSerializer(typeof(Trains));
            TextReader trainsTextReader = ReadFile(@"DataModels.Helper.Trains_Hyd_SC_KCG.xml");
            Trains trains = (Trains)trainsXmlSerializer.Deserialize(trainsTextReader);
            trainsTextReader.Close();
            return trains;
        }
    }
}
