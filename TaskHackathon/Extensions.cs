using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataModels.Answer;
using DataModels.Common;
using DataModels.Profile;

namespace TaskHackathon
{
    public class Extensions
    {
        //private void AdditionalQU(string query,TaskAnswer answer,UserProfile userProfile)
        //{
        //    string queryInLowerCase = query.ToLower();

        //    string matchResult = MatchRelation(queryInLowerCase);
        //    if(matchResult != null)
        //    {
        //        // check if present in userprofile and populate values
        //        if(userProfile!=null && userProfile.KnownPeopleList!= null)
        //        {
        //            foreach(Person p in userProfile.KnownPeopleList)
        //            {
        //                if(p.Relation==matchResult) 
        //                {
        //                    PassangerInfo passengerInfo = new PassangerInfo();
        //                    passengerInfo.Name = p.Name;
        //                    passengerInfo.Gender = p.Gender;
        //                    // Age not populated 
        //                    TrainBookingState trainBookingState = answer.TaskState as TrainBookingState;
        //                    if(trainBookingState !=null)
        //                    {
        //                        trainBookingState.PassangerInfoList.Add(passengerInfo);
        //                        trainBookingState.NumberOfPassangers++;
        //                        if(String.IsNullOrEmpty(p.ContactNumber))
        //                            trainBookingState.PhoneNumber = p.ContactNumber;
        //                    }
        //                    break;
        //                }
        //            }
                        
        //        }
        //    }else if(matchResult = MatchLocation(queryInLowerCase))
        //    {
        //        if(isMatchLocationForSource(queryInLowerCase))
        //        {
        //            TrainBookingState trainBookingState = answer.TaskState as TrainBookingState;
        //                    if(trainBookingState !=null)
        //                    {
        //                        trainBookingState.Source.Name = userProfile.HomeTown;
        //                    }

        //        }else if(isMatchLocationForDestination(queryInLowerCase))
        //        {
        //            TrainBookingState trainBookingState = answer.TaskState as TrainBookingState;
        //            if(trainBookingState !=null)
        //            {
        //                trainBookingState.Destination.Name = userProfile.HomeTown;
        //            }
        //        }
        //    }
            
        //}

        //private string MatchRelation(string query)
        //{
        //    foreach(string s in Relation)
        //    {
        //        Match result = Regex.Match(query, "("+s+")");
        //        if (result.Success)
        //            return result.Value;
            
        //    }
        //    return null;
        //}

        //private string string MatchLocation(string query)
        //{
        //    List<string> locationList = new List<string>(){"home","hometown"};
        //    foreach(string s in locationList)
        //    {
        //        Match result = Regex.Match(query, "("+s+")");
        //        if (result.Success)
        //            return result.Value;
            
        //    }
        //    return null;
        //}

        //private bool isMatchLocationForSource(string query)
        //{
        //    Match result = Regex.Match(query, "(from)");
        //        if (result.Success)
        //            return true;
        //    return false;
        //}
        //private bool isMatchLocationForDestination(string query)
        //{
        //    Match result = Regex.Match(query, "(to)");
        //        if (result.Success)
        //            return true;
        //    return false;
        //}
    }
}
