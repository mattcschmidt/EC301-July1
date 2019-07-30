using Patient_Report.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VMS.TPS.Common.Model.API;

namespace Patient_Report.Views
{
    /// <summary>
    /// Interaction logic for ReportView.xaml
    /// </summary>
    public partial class ReportView : UserControl
    {
        private ReportViewModel _reportViewModel;

        public ReportView(PlanSetup planSetup)
        {
            _reportViewModel = new ReportViewModel(planSetup);
            this.DataContext = _reportViewModel;
            InitializeComponent();
        }
    }
}
