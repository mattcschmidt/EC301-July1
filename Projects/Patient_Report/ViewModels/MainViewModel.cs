using Example_DVH.ViewModels;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;

namespace Patient_Report.ViewModels
{
    public class MainViewModel
    {
        public MainViewModel(DVHViewModel dVHViewModel,
            ReportViewModel reportViewModel)
        {
            DVHViewModel = dVHViewModel;
            ReportViewModel = reportViewModel;
        }


        public DVHViewModel DVHViewModel { get; }
        public ReportViewModel ReportViewModel { get; }
    }
}
