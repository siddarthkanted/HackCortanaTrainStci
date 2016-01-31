using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Common
{
    public class QueryExtractedData
    {
        public string query;
        
        public TaskStateCode currentState;

        public List<MetaData> metadataList;

        public List<ProfileData> profileData;

    }

    public class MetaData
    {
        public TaskStateCode code;

        public Object dataObject;
    }

    public class ProfileData
    {
        public string key;
        public string value;
    }
}
