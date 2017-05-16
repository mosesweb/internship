using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace MosesPraktik.Features.Timer_Job
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>


    [Guid("b6f6b68e-af4d-4702-a291-9447a05adb71")]
    public class Timer_JobEventReceiver : SPFeatureReceiver
    {

        public void DeleteJob(SPJobDefinitionCollection jobs)
        {
            foreach (SPJobDefinition job in jobs)
            {
                if (job.Name.Equals(ErrandJobDefinition.JobName, 
                    StringComparison.OrdinalIgnoreCase))
                {
                    job.Delete();
                }
            }
        }
       
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWebApplication webApp = properties.Feature.Parent as SPWebApplication;
            DeleteJob(webApp.JobDefinitions);

            ErrandJobDefinition errandsJob = new ErrandJobDefinition(webApp);

            SPDailySchedule schedule = new SPDailySchedule();
            schedule.BeginHour = 9;
            schedule.EndHour = 11;
            errandsJob.Schedule = schedule;
            errandsJob.Update();
        }

        // Uncomment the method below to handle the event raised before a feature is deactivated

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPWebApplication webApp = properties.Feature.Parent as SPWebApplication;
            DeleteJob(webApp.JobDefinitions);
        }
    }
}
