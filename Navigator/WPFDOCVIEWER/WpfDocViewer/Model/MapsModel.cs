using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDocViewer.Model
{
    /// <summary>
    /// Mark Lane
    /// Use this Map Activity as an ObservableCollection and bind the List controls
    /// to each Path= to show the proper relationship.
    /// </summary>
    public class MapActivityModel
    {
        public MapActivityModel(string Map, string Activity)
        {
            MapName = Map;
            ActivityName = Activity;
        }
        public string MapName
        {
            get;
            set;
        }
        public string ActivityName
        {
            get;
            set;
        }
    }
}
