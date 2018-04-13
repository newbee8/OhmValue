using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OhmValue.Models;
using System.Drawing;
using OhmValue.Interfaces;


namespace OhmValue.Controllers
{
    public class OhmValueController : Controller, IOhmValueCalculator
    {
        enum Colors { Black, Brown, Red, Orange, Yellow, Green, Blue, Violet, Gray, White }

        Band bandA = new Band { BandName = "Band A", BandColors = new List<BandColor>() };
        Band bandB = new Band { BandName = "Band B", BandColors = new List<BandColor>() };
        Band multiplierBands = new Band { BandName = "Multiplier", BandColors = new List<BandColor>() };
        Band toleranceBands = new Band { BandName = "Tolerance", BandColors = new List<BandColor>() };

        // GET: OhmValue
        public ActionResult OhmValueCalculator()
        {
            ResistorBands resistorBands = SetBands();

            resistorBands.selectedBandA = "Black";
            resistorBands.selectedBandB = "Black";
            resistorBands.selectedMultiplierBand = "Silver";
            resistorBands.selectedToleranceBand = "Silver";

            var ohmValue = CalculateOhmValue(resistorBands.selectedBandA, resistorBands.selectedBandB, resistorBands.selectedMultiplierBand, resistorBands.selectedToleranceBand);
            ViewBag.OhmValue = ohmValue;
            return View(resistorBands);
        }

        [HttpPost]
        public ActionResult OhmValueCalculator(ResistorBands bandModel)
        {
            ResistorBands resistorBands = SetBands();
            var ohmValue = CalculateOhmValue(bandModel.selectedBandA, bandModel.selectedBandB, bandModel.selectedMultiplierBand, bandModel.selectedToleranceBand);
            ViewBag.OhmValue = ohmValue;
            return View(resistorBands);
        }

        /***Slight Deviation from the mentioned requirement: returning double instead of int***/
        public double CalculateOhmValue(string bandAColor, string bandBColor, string bandCColor, string bandDColor)
        {
            double bandAValue = bandA.BandColors.First(b => b.ColorDescription == bandAColor).BandValue;
            double bandBValue = bandB.BandColors.First(b => b.ColorDescription == bandBColor).BandValue;
            double multiplierValue = multiplierBands.BandColors.First(b => b.ColorDescription == bandCColor).BandValue;
            double tolerance = toleranceBands.BandColors.First(b => b.ColorDescription == bandDColor).BandValue;

            double multiplier = Math.Pow(10, multiplierValue);
            double ohmValue = Int32.Parse(bandAValue.ToString() + bandBValue.ToString()) * multiplier;

            var maxRange = ohmValue + ((ohmValue * tolerance) / 100);
            var minRange = ohmValue - ((ohmValue * tolerance) / 100);

            ViewBag.Tolerance = tolerance;
            ViewBag.Range = " = (" + minRange.ToString() + " to " + maxRange.ToString() + " Ohms)";

            return ohmValue;
        }


        public ResistorBands SetBands()
        {
            //Gold and Silver are 2 extra band colors for the Multiplier band (Not part of enum 'Colors' )
            multiplierBands.BandColors.Add(constructBand("Silver", -2));
            multiplierBands.BandColors.Add(constructBand("Gold", -1));

            //Adding the colors that are common in all the 3 bands to the corresponding list
            foreach (Colors color in Enum.GetValues(typeof(Colors)))
            {
                bandA.BandColors.Add(constructBand(color.ToString()));
                bandB.BandColors.Add(constructBand(color.ToString()));
                multiplierBands.BandColors.Add(constructBand(color.ToString()));
            }

            //Tolerance Bands do not increment in any particular order. So, manually adding them to the list with corresponding values.
            toleranceBands.BandColors.Add(constructBand("Silver", 10));
            toleranceBands.BandColors.Add(constructBand("Gold", 5));
            toleranceBands.BandColors.Add(constructBand("Brown", 1));
            toleranceBands.BandColors.Add(constructBand("Red", 2));
            toleranceBands.BandColors.Add(constructBand("Orange", 3));
            toleranceBands.BandColors.Add(constructBand("Yellow", 4));
            toleranceBands.BandColors.Add(constructBand("Green", 0.5));
            toleranceBands.BandColors.Add(constructBand("Blue", 0.25));
            toleranceBands.BandColors.Add(constructBand("Violet", 0.1));
            toleranceBands.BandColors.Add(constructBand("Gray", 0.05));

            ResistorBands resistorBands = new ResistorBands { Bands = new List<Band>() };
            resistorBands.Bands.Add(bandA);
            resistorBands.Bands.Add(bandB);
            resistorBands.Bands.Add(multiplierBands);
            resistorBands.Bands.Add(toleranceBands);

            return resistorBands;
        }

        public BandColor constructBand(string colorName, double? value = null)
        {
            Color color = Color.FromName(colorName);

            //Contrast color to make the text readable on the view
            Color textColor = (((color.R + color.B + color.G) / 3) > 128) ? Color.Black : Color.White;

            return new BandColor
            {
                ColorCode = "RGB(" + color.R.ToString() + "," + color.G.ToString() + "," + color.B.ToString() + ")",
                ColorDescription = colorName,
                BandValue = value != null ? (double)value : (int)Enum.Parse(typeof(Colors), colorName),
                TextColorCode = "RGB(" + textColor.R.ToString() + "," + textColor.G.ToString() + "," + textColor.B.ToString() + ")"
            };
        }

    }
}