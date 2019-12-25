using System.Collections.Generic;

namespace WHC
{

    public struct ChartData
    {
        public bool isOverGoal { get; set; }

        public List<string> categories { get; set; }
        public List<SeriesItem> series { get; set; }

        public struct SeriesItem
        {
            public string type { get; set; }
            public string name { get; set; }
            public List<SubItem> data { get; set; }
            public struct SubItem
            {
                public decimal y { get; set; }
                public string color { get; set; }
            }

        }

    }

    public struct ChartDataSimple
    {
        public List<string> categories { get; set; }
        public List<SeriesItem> series { get; set; }

        public struct SeriesItem
        {
            public string name { get; set; }
            public List<decimal?> data { get; set; }

        }

    }
}