using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using static System.Diagnostics.Debug;
namespace TrackLift.Models
{
    public class SetGroup : ModelBase
    {
        private ObservableCollection<Set> sets;

        public ObservableCollection<Set> Sets
        {
            get => sets;
            set => SetProperty(ref sets, value);
        }

        public SetGroup()
        {
            Sets = new ObservableCollection<Set>();
        }

        public Boolean TryAddSet(Set set)
        {
            Boolean added = false;

            Assert(set != null);
            if (set != null)
            {
                Sets.Add(set);
                added = true;
            }

            return added;
        }
    }
}
