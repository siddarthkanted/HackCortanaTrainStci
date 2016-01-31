using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModels.Profile;

namespace DataModels.Common
{
    public class PassangerInfo
    {
        public PassangerInfo()
        {
            Id = 0;
            Name = string.Empty;
            Age = 0;
            Gender = Gender.None;
            BerthChoice = BerthChoice.None;
            IsComplete = false;
            Relation = Relation.Other;
        }

        public uint Id;

        public string Name;

        public uint Age;

        public Gender Gender;

        public BerthChoice BerthChoice;

        public Relation Relation;

        public bool IsComplete;
    }
}
