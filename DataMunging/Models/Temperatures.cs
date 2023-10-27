namespace DataMunging.Models
{
    public class Temperatures
    {
        public int Day { get; set; }

        public int MxT { get; set; }

        public int MnT { get; set; }

        public int AvT { get; set; }

        public int? HDDay { get; set; }

        public double AvDP { get; set; }

        public double? HrP1 { get; set; }

        public double TPcpn { get; set; }

        public string WxType { get; set; }

        public string PDir { get; set; }

        public double AvSp { get; set; }

        public string Dir { get; set; }

        public int MxS { get; set; }

        public double SkyC { get; set; }

        public int MxR { get; set; }

        public int MnR { get; set; }

        public double AvSLP { get; set; }

        public int MinDiff { get; set; }
    }
}