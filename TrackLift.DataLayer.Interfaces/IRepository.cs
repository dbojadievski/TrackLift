using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TrackLift.Models;

namespace TrackLift.DataLayer.Interfaces
{
    public interface IRepository<Type> where Type: ModelBase
    {
        public String TableName { get; }

        bool DoesTableExist();
        bool CreateTable();
        bool DeleteTable();

        public IEnumerable<Type> GetAll();

        public bool Insert(IEnumerable<Type> entries);
        public bool Update(IEnumerable<Type> entries);

        public bool Delete(IEnumerable<Type> entries);

        /// <summary>
        /// Read the name again. Are you SURE you want to do this?
        /// </summary>
        /// <returns></returns>
        public bool DeleteAll();
    }
}
