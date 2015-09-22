using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Sender.Container;

namespace Sender.App
{
    partial class Service : ServiceBase
    {
        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
#if DEBUG
          //  HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();
#endif


            var bootstrapper = new Bootstrapper();

        }

        protected override void OnStop()
        {
        }
    }
}

