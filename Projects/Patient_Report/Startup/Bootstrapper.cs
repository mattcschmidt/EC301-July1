using Autofac;
using Example_DVH.ViewModels;
using Patient_Report.ViewModels;
using Patient_Report.Views;
using VMS.TPS.Common.Model.API;

namespace Patient_Report.Startup
{
    public class Bootstrapper
    {
        /// <summary>
        /// Generate an IOC Container using Autofac.
        /// </summary>
        /// <param name="planSetup"></param>
        /// <returns></returns>
       public IContainer Bootstrap(PlanSetup planSetup)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<MainView>().AsSelf();

            //viewmodels
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<DVHViewModel>().AsSelf();
            builder.RegisterType<ReportViewModel>().AsSelf();
            builder.RegisterInstance<PlanSetup>(planSetup);
            return builder.Build();
        }
    }
}
