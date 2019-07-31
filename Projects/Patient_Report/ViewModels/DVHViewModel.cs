using OxyPlot;
using OxyPlot.Series;
using VMS.TPS.Common.Model.API;

namespace Example_DVH.ViewModels
{
    public class DVHViewModel
    {
        private PlanSetup _planSetup;

        private PlotModel myPlotModel;        
        public PlotModel MyPlotModel
        {
            get { return myPlotModel; }
            set { myPlotModel = value; }
        }

        public DVHViewModel(PlanSetup planSetup)
        {
            _planSetup = planSetup;
            MyPlotModel = new PlotModel
            {
                Title = $"DVH for {_planSetup.Id}",
                LegendTitle = "Structures"
            };
            DrawDVH();
        }

        private void DrawDVH()
        {
            MyPlotModel.Series.Clear();
            foreach(Structure s in _planSetup.StructureSet.Structures)
            {
                if(s.DicomType !="MARKER" && s.DicomType != "SUPPORT" && s.HasSegment)
                {
                    DVHData dvh = _planSetup.GetDVHCumulativeData(s,
                        VMS.TPS.Common.Model.Types.DoseValuePresentation.Absolute,
                        VMS.TPS.Common.Model.Types.VolumePresentation.Relative,
                        1);
                    LineSeries lineSeries = new LineSeries
                    {
                        Title = s.Id,
                        Color = OxyColor.Parse(s.Color.ToString())
                    };
                    foreach(var dvhData in dvh.CurveData)
                    {
                        lineSeries.Points.Add(new DataPoint
                            (dvhData.DoseValue.Dose,
                            dvhData.Volume));
                    }
                    MyPlotModel.Series.Add(lineSeries);
                }
            }
        }
    }
}
