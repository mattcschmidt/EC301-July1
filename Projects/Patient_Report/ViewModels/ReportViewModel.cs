using Example_DVH.ViewModels;
using Patient_Report.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace Patient_Report.ViewModels
{
    public class ReportViewModel : BindableBase
    {
        public List<string> Structures { get; set; }
        private string selectedStructure;

        public string SelectedStructure
        {
            get { return selectedStructure; }
            set
            {
                SetProperty(ref selectedStructure, value);
                AddMetric.RaiseCanExecuteChanged();
            }
        }
        public List<string> Metrics { get; set; }

        private PlanSetup _planSetup;
        private string selectedMetric;

        public string SelectedMetric
        {
            get { return selectedMetric; }
            set
            {
                SetProperty(ref selectedMetric, value);
                AddMetric.RaiseCanExecuteChanged();
            }
        }
        public DelegateCommand AddMetric { get; private set; }
        public ObservableCollection<DVHMetric> DQPs { get; private set; }
        public ReportViewModel(PlanSetup planSetup)
        {
            _planSetup = planSetup;
            Structures = new List<string>(_planSetup.StructureSet.Structures.Select(x => x.Id));
            Metrics = new List<string>(Enum.GetNames(typeof(DoseMetricType)));
            DQPs = new ObservableCollection<DVHMetric>();
            AddMetric = new DelegateCommand(OnAddMetric, CanAddMetric);
        }

        private void OnAddMetric()
        {
            DQPs.Add(new DVHMetric
            {
                StructureId = SelectedStructure,
                DoseMetric = SelectedMetric,
                OutputValue = CalculateOutputValue()//replace with method to get metric.
            });
        }

        private string CalculateOutputValue()
        {
            string output = "";
            Structure s = _planSetup.StructureSet.Structures.SingleOrDefault(x => x.Id == SelectedStructure);
            DVHData dvh = _planSetup.GetDVHCumulativeData(
               s,
                VMS.TPS.Common.Model.Types.DoseValuePresentation.Absolute,
                VMS.TPS.Common.Model.Types.VolumePresentation.Relative,
                1);
            switch (Metrics.IndexOf(SelectedMetric))
            {
                case (int)DoseMetricType.Mean://mean
                    output = dvh.MeanDose.ToString();
                    break;
                case (int)DoseMetricType.Max:
                    output = dvh.MaxDose.ToString();
                    break;
                case (int)DoseMetricType.Min:
                    output = dvh.MinDose.ToString();
                    break;
                case (int)DoseMetricType.Volume:
                    output = s.Volume.ToString("F2") + "cc";
                    break;
                case (int)DoseMetricType.gEUD:
                    output = CalculateGEUD(s, 
                        _planSetup as PlanningItem, 
                        structure_ids.FirstOrDefault(x => x.Key == s.Id)).ToString("F2");
                    break;
            }
            return output;
        }
        Dictionary<string, double> structure_ids = new Dictionary<string, double>()
        {
            { "Heart", 0.5 },
            { "Cord",20 },
            { "Parotid",0.5 },
            { "Lung",0.5 },
            { "Bladder",0.5 },
            { "Rectum", 20 },
            {"stem",20 },
            {"PTV",-0.1 }
        };
        /// <summary>
        /// Method from WUSTL-ClinicalDev open source.
        /// Terms of use from the code apply.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="pi"></param>
        /// <param name="a_lookup"></param>
        /// <returns></returns>
        private double CalculateGEUD(Structure s, PlanningItem pi, KeyValuePair<string, double> a_lookup)
        {
            //collect the DVH
            //if volume is not relative, make sure to normalize over the total volume during geud calculation.
            //double volume = s.Volume;
            //remember plansums must be absolute dose.
            DVHData dvh = pi.GetDVHCumulativeData(s, DoseValuePresentation.Absolute, VolumePresentation.Relative, 0.1);
            if (dvh == null)
            {
                MessageBox.Show("Could not calculate DVH");
                return Double.NaN;
            }
            //we need to get the differential volume from the definition. Loop through Volumes and take the difference with the previous dvhpoint
            double running_sum = 0;
            int counter = 0;
            foreach (DVHPoint dvhp in dvh.CurveData.Skip(1))
            {
                //volume units are in % (divide by 100)
                double vol_diff = Math.Abs(dvhp.Volume - dvh.CurveData[counter].Volume) / 100;
                double dose = dvhp.DoseValue.Dose;
                running_sum += vol_diff * Math.Pow(dose, a_lookup.Value);
                counter++;
            }
            double geud = Math.Pow(running_sum, 1 / a_lookup.Value);
            return geud;
        }
        private bool CanAddMetric()
        {
            return SelectedStructure != null && SelectedMetric != null;
        }
    }
}
