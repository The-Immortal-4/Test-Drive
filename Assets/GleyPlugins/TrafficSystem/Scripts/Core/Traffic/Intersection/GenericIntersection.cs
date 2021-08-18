using System.Collections.Generic;
using UnityEngine;

namespace GleyTrafficSystem
{
    /// <summary>
    /// Base class for all intersections
    /// </summary>
    [System.Serializable]
    public class GenericIntersection : IIntersection
    {
        public string name;


        public virtual void Initialize(WaypointManager waypointManager, float greenLightTime, float yellowLightTime)
        {

        }

        public virtual void ResetIntersection()
        {

        }


        public virtual void UpdateIntersection()
        {

        }


        public virtual bool IsPathFree(int waypointIndex)
        {
            return false;
        }


        public virtual void VehicleEnter(Waypoint waypoint)
        {

        }


        public virtual void VehicleLeft(Waypoint waypoint)
        {

        }


        public virtual List<IntersectionStopWaypointsIndex> GetWaypoints()
        {
            return new List<IntersectionStopWaypointsIndex>();
        }

        public virtual void ResetIntersections(Vector3 center, float radius)
        {
            
        }
    }
}