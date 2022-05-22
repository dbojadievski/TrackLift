using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

using TrackLift.Base;

namespace TrackLift.Models
{
    public class ModelBase : DataBindableBase
    {
        private Guid id;
        private DateTime createdAt;
        private DateTime updatedAt;

        public Guid Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        public DateTime CreatedAt
        {
            get => createdAt;
            set => SetProperty(ref createdAt, value);
        }

        public DateTime UpdatedAt
        {
            get => updatedAt;
            set => SetProperty(ref updatedAt, value);
        }
    }
}
