using System;
using System.Linq;
using System.Collections.ObjectModel;

using static System.Diagnostics.Debug;
namespace TrackLift.Models
{
    public class Session : ModelBase
    {
        private ObservableCollection<SetGroup> setGroups;

        public ObservableCollection<SetGroup> SetGroups
        {
            get => setGroups;
            set => SetProperty(ref setGroups, value);
        }

        public Session()
        {
            SetGroups = new ObservableCollection<SetGroup>();
        }
    }
}
