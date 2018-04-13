using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OhmValue.Models
{
    public class ResistorBands
    {
        public List<Band> Bands { get; set; }
        public string selectedBandA { get; set; }
        public string selectedBandB { get; set; }
        public string selectedMultiplierBand { get; set; }
        public string selectedToleranceBand { get; set; }
    }

    public class Band
    {
        public string BandName { get; set; }
        public List<BandColor> BandColors { get; set; }
    }

    public class BandColor
    {
        public string ColorDescription { get; set; }
        public string ColorCode { get; set; }
        public string TextColorCode { get; set; }
        public double BandValue { get; set; }
    }
}