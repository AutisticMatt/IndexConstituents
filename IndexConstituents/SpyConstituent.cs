using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AutisticMatt.IndexConstituents
{

    /// <summary>
    /// Class format is based on the data from Sharadar.
    /// </summary>
    public class SpyConstituent
    {
        public enum ActionEnum
        {
            historical = 1,
            current = 2,
            added = 3,
            removed = 4,
        }

        public DateTime DT { get; set; }
        public ActionEnum Action { get; set; }
        public string Ticker { get; set; }
        public string Name { get; set; }
        public string ContraName { get; set; }
        public string ContraTicker { get; set; }
        public string Notes { get; set; }

        private static CultureInfo provider = CultureInfo.InvariantCulture;
        public static SpyConstituent FromCSV(string line)
        {
            var flds = line.Split(',');
            var bh = new SpyConstituent();
            bh.Action = ToActionEnum(flds[1]);
            bh.ContraName = flds[5];
            bh.ContraTicker = flds[4];
            bh.DT = DateTime.ParseExact(flds[0], "yyyy-MM-dd", provider);
            bh.Name = flds[3];
            bh.Notes = flds[6];
            bh.Ticker = flds[2];
            return bh;
        }
        private static ActionEnum ToActionEnum(string fld)
        {
            switch(fld)
            {
                case "historical": return ActionEnum.historical;
                case "current": return ActionEnum.current;
                case "added": return ActionEnum.added;
                case "removed": return ActionEnum.removed;
            }
            throw new MissingFieldException();
        }
        public static List<SpyConstituent> FromCSV(string[] lines)
        {
            var all = new List<SpyConstituent>(lines.Length);
            foreach (var line in lines.Skip(1))
            {
                all.Add(SpyConstituent.FromCSV(line));
            }
            return all;
        }




    }
}
