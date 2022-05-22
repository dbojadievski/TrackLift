using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackLift.Models
{
    public class Set : ModelBase
    {
        private Exercise exercise;
        private Double weight;
        private Double repCount;

        public Exercise Exercise
        {
            get => exercise;
            set => SetProperty(ref exercise, value);
        }

        public Double Weight
        {
            get => weight;
            set => SetProperty(ref weight, value);
        }

        public Double RepCount
        {
            get => repCount;
            set => SetProperty(ref repCount, value);
        }
    }
}
