using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataModels.Helper
{
    public class TrainUtil
    {
        public static List<Train> ConvertJsonToTrainData(string json)
        {
            List<Train> trainData = JsonConvert.DeserializeObject<List<Train>>(json);
            //trainData = trainData.GetRange(0, 5);
            return trainData;
        }

        public static List<Train> DownlondJsonFromUrl(string fromStation, string toStation)
        {
            //string url = "http://azurehacktrainwebapplication.azurewebsites.net/getTrainListOnStationCode?fromStationCode={0}&toStationCode={1}";
            //url = String.Format(url, fromStation.ToLower(), toStation.ToLower());

            //using (var client = new WebClient())
            //{

            //    var response = client.DownloadData(url);

            //    string responseString = Encoding.Default.GetString(response);

            //    return ConvertJsonToTrainData(responseString);

            //}

            TrainDataController trainDataController = new TrainDataController();
            return trainDataController.GetTrainListOnStationCode(fromStation, toStation);
        }

        public static List<string> ConvertTrainDataListToStringList(List<Train> trainDataList)
        {
            List<string> stringList = new List<string>();
            foreach (Train trainData in trainDataList)
            {
                stringList.Add(String.Format("{0}|{1}|{2}", trainData.TrainRecord.TrainNumber, trainData.TrainRecord.TrainName, trainData.TrainRecord.TrainScheduleItem[0].DepartureTime));
            }
            return stringList;
        } 

    }
}
