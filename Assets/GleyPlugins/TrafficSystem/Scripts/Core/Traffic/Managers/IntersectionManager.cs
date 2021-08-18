using System.Collections.Generic;
using UnityEngine;

namespace GleyTrafficSystem
{
    /// <summary>
    /// Updates all intersections
    /// </summary>
    public class IntersectionManager : MonoBehaviour
    {
        private GenericIntersection[] allIntersections;
        private List<GenericIntersection> activeIntersections;
        private WaypointManager waypointManager;
        private bool debugIntersections;
        private bool stopIntersectionUpdate;


        /// <summary>
        /// Initialize intersection manager
        /// </summary>
        /// <param name="allIntersections"></param>
        /// <param name="activeIntersections"></param>
        /// <param name="waypointManager"></param>
        /// <param name="greenLightTime"></param>
        /// <param name="yellowLightTime"></param>
        /// <param name="debugIntersections"></param>
        /// <param name="stopIntersectionUpdate"></param>
        /// <returns></returns>
        public IntersectionManager Initialize(GenericIntersection[] allIntersections, List<GenericIntersection> activeIntersections, WaypointManager waypointManager, float greenLightTime, float yellowLightTime, bool debugIntersections, bool stopIntersectionUpdate)
        {
            IntersectionEvents.onActiveIntersectionsChanged += SetActiveIntersection;
            this.debugIntersections = debugIntersections;
            this.stopIntersectionUpdate = stopIntersectionUpdate;
            this.allIntersections = allIntersections;
            this.waypointManager = waypointManager;

            for (int i = 0; i < allIntersections.Length; i++)
            {
                allIntersections[i].Initialize(waypointManager, greenLightTime, yellowLightTime);
            }

            SetActiveIntersection(activeIntersections);
            return this;
        }


        /// <summary>
        /// Used to reset the intersection when cars are removed from it
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        internal void ResetIntersections(Vector3 center, float radius)
        {
            float radiusSq = radius * radius;
            for (int i = 0; i < activeIntersections.Count; i++)
            {
                List<IntersectionStopWaypointsIndex> stopWaypoints = activeIntersections[i].GetWaypoints();
                for (int j = 0; j < stopWaypoints.Count; j++)
                {
                    for (int k = 0; k < stopWaypoints[j].roadWaypoints.Count; k++)
                    {
                        if (Vector3.SqrMagnitude(center - waypointManager.GetWaypoint(stopWaypoints[j].roadWaypoints[k]).position) < radiusSq)
                        {
                            activeIntersections[i].ResetIntersection();
                            break;
                        }
                    }
                }           
            }
        }

        /// <summary>
        /// Initialize all active intersections
        /// </summary>
        /// <param name="activeIntersections"></param>
        public void SetActiveIntersection(List<GenericIntersection> activeIntersections)
        {
            for (int i = 0; i < activeIntersections.Count; i++)
            {
                if (this.activeIntersections != null)
                {
                    if (!this.activeIntersections.Contains(activeIntersections[i]))
                    {
                        activeIntersections[i].ResetIntersection();
                    }
                }
            }
            this.activeIntersections = activeIntersections;
        }


        /// <summary>
        /// Called on every frame to update active intersection road status
        /// </summary>
        public void UpdateIntersections()
        {
#if UNITY_EDITOR
            if (stopIntersectionUpdate)
                return;
#endif
            for (int i = 0; i < activeIntersections.Count; i++)
            {
                activeIntersections[i].UpdateIntersection();
            }
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (debugIntersections)
            {
                for (int k = 0; k < allIntersections.Length; k++)
                {
                    List<IntersectionStopWaypointsIndex> stopWaypoints = allIntersections[k].GetWaypoints();
                    for (int i = 0; i < stopWaypoints.Count; i++)
                    {

                        for (int j = 0; j < stopWaypoints[i].roadWaypoints.Count; j++)
                        {
                            if (waypointManager.GetWaypoint(stopWaypoints[i].roadWaypoints[j]).stop == true)
                            {
                                Gizmos.color = Color.red;
                                Gizmos.DrawSphere(waypointManager.GetWaypoint(stopWaypoints[i].roadWaypoints[j]).position, 1);
                            }
                        }
                    }
                }
            }
        }
#endif
    }
}