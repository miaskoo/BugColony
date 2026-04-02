using UnityEngine;
using Zenject;

namespace BugColony.Infrastructure
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Application.runInBackground = true;
        }
    }
}
