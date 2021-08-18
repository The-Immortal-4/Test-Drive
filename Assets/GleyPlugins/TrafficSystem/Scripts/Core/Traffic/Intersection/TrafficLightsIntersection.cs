using System.Collections.Generic;
using UnityEngine;

namespace GleyTrafficSystem
{
    [System.Serializable]
    public class TrafficLightsIntersection : GenericIntersection
    {
        public List<IntersectionStopWaypointsIndex> stopWaypoints;
        public float greenLightTime;
        public float yellowLightTime;
      
        private float currentTime;
        private int nrOfRoads;
        private int currentRoad;
        private bool yellowLight;


        /// <summary>
        /// Constructor used for conversion from editor intersection type
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stopWaypoints"></param>
        /// <param name="greenLightTime"></param>
        /// <param name="yellowLightTime"></param>
        public TrafficLightsIntersection(string name, List<IntersectionStopWaypointsIndex> stopWaypoints, float greenLightTime, float yellowLightTime)
        {
            this.name = name;
            this.stopWaypoints = stopWaypoints;
            this.greenLightTime = greenLightTime;
            this.yellowLightTime = yellowLightTime;
        }


        /// <summary>
        /// Assigns corresponding waypoints to work with this intersection and setup traffic lights
        /// </summary>
        /// <param name="waypointManager"></param>
        /// <param name="greenLightTime"></param>
        /// <param name="yellowLightTime"></param>
        public override void Initialize(WaypointManager waypointManager, float greenLightTime, float yellowLightTime)
        {
            nrOfRoads = stopWaypoints.Count;

            if (nrOfRoads == 0)
            {
                Debug.LogWarning("Intersection " + name + " has some unassigned references");
                return;
            }

            for (int i = 0; i < stopWaypoints.Count; i++)
            {
                for (int j = 0; j < stopWaypoints[i].roadWaypoints.Count; j++)
                {
                    waypointManager.GetWaypoint(stopWaypoints[i].roadWaypoints[j]).SetIntersection(this, false, true, false, false);
                }
            }
            
            currentRoad = Random.Range(0, nrOfRoads);
            SetInitialTrafficLights();
            UpdateCurrentIntersectionWaypoints(currentRoad);
            SetGreenLight(currentRoad);
            currentTime = Time.realtimeSinceStartup;
            if (greenLightTime >= 0)
            {
                this.greenLightTime = greenLightTime;
            }
            if (yellowLightTime >= 0)
            {
                this.yellowLightTime = yellowLightTime;
            }
        }


        /// <summary>
        /// Change traffic lights color
        /// </summary>
        public override void UpdateIntersection()
        {
            if (yellowLight == false)
            {
                if (Time.realtimeSinceStartup - currentTime > greenLightTime)
                {
                    UpdateCurrentIntersectionWaypoints(currentRoad);
                    SetYellowLight(currentRoad);
                    yellowLight = true;
                    currentTime = Time.realtimeSinceStartup;
                }
            }
            else
            {
                if (Time.realtimeSinceStartup - currentTime > yellowLightTime)
                {
                    SetRedLight(currentRoad);
                    currentRoad++;
                    currentRoad = GetValidValue(currentRoad);
                    UpdateNextIntersectionWaypoints(currentRoad);
                    SetGreenLight(currentRoad);
                    yellowLight = false;
                    currentTime = Time.realtimeSinceStartup;
                }
            }
        }


        /// <summary>
        /// Used for editor applications
        /// </summary>
        /// <returns></returns>
        public override List<IntersectionStopWaypointsIndex> GetWaypoints()
        {
            return stopWaypoints;
        }


        /// <summary>
        /// Set intersection to red light
        /// </summary>
        private void SetInitialTrafficLights()
        {
            for (int i = 0; i < stopWaypoints.Count; i++)
            {
                for (int j = 0; j < stopWaypoints[i].redLightObjects.Count; j++)
                {
                    if (stopWaypoints[i].redLightObjects[j] != null)
                    {
                        stopWaypoints[i].redLightObjects[j].SetActive(true);
                    }
                    else
                    {
                        Debug.LogWarning("Intersection " + name + " has null red light objects");
                    }
                }
            }
            for (int i = 0; i < stopWaypoints.Count; i++)
            {
                for (int j = 0; j < stopWaypoints[i].yellowLightObjects.Count; j++)
                {
                    if (stopWaypoints[i].yellowLightObjects[j] != null)
                    {
                        stopWaypoints[i].yellowLightObjects[j].SetActive(false);
                    }
                    else
                    {
                        Debug.LogWarning("Intersection " + name + " has null yellow light objects");
                    }
                }
            }
            for (int i = 0; i < stopWaypoints.Count; i++)
            {
                for (int j = 0; j < stopWaypoints[i].greenLightObjects.Count; j++)
                {
                    if (stopWaypoints[i].greenLightObjects[j] != null)
                    {
                        stopWaypoints[i].greenLightObjects[j].SetActive(false);
                    }
                    else
                    {
                        Debug.LogWarning("Intersection " + name + " has null green light objects");
                    }
                }
            }
        }


        private void UpdateCurrentIntersectionWaypoints(int road)
        {
            //set current road to red color
            for (int j = 0; j < stopWaypoints[road].roadWaypoints.Count; j++)
            {
                WaypointEvents.TriggerStopIndicatorChangedEvent(stopWaypoints[road].roadWaypoints[j]);
            }
        }


        private void UpdateNextIntersectionWaypoints(int road)
        {
            // set current color to green
            for (int j = 0; j < stopWaypoints[road].roadWaypoints.Count; j++)
            {
                WaypointEvents.TriggerStopIndicatorChangedEvent(stopWaypoints[road].roadWaypoints[j]);
            }

        }


        /// <summary>
        /// Set traffic lights to red for the current road
        /// </summary>
        /// <param name="road"></param>
        private void SetRedLight(int road)
        {
            for (int j = 0; j < stopWaypoints[road].redLightObjects.Count; j++)
            {
                if (stopWaypoints[road].redLightObjects[j] != null)
                {
                    stopWaypoints[road].redLightObjects[j].SetActive(true);
                }
            }

            for (int j = 0; j < stopWaypoints[road].yellowLightObjects.Count; j++)
            {
                if (stopWaypoints[road].yellowLightObjects[j] != null)
                {
                    stopWaypoints[road].yellowLightObjects[j].SetActive(false);
                }
            }

            for (int j = 0; j < stopWaypoints[road].greenLightObjects.Count; j++)
            {
                if (stopWaypoints[road].greenLightObjects[j] != null)
                {
                    stopWaypoints[road].greenLightObjects[j].SetActive(false);
                }
            }
        }


        /// <summary>
        /// Set traffic lights to yellow for the current road
        /// </summary>
        /// <param name="road"></param>
        private void SetYellowLight(int road)
        {
            for (int j = 0; j < stopWaypoints[road].redLightObjects.Count; j++)
            {
                if (stopWaypoints[road].redLightObjects[j] != null)
                {
                    stopWaypoints[road].redLightObjects[j].SetActive(false);
                }
            }

            for (int j = 0; j < stopWaypoints[road].yellowLightObjects.Count; j++)
            {
                if (stopWaypoints[road].yellowLightObjects[j] != null)
                {
                    stopWaypoints[road].yellowLightObjects[j].SetActive(true);
                }
            }

            for (int j = 0; j < stopWaypoints[road].greenLightObjects.Count; j++)
            {
                if (stopWaypoints[road].greenLightObjects[j] != null)
                {
                    stopWaypoints[road].greenLightObjects[j].SetActive(true);
                }
            }
        }


        /// <summary>
        /// Set traffic lights to green for the current road
        /// </summary>
        /// <param name="road"></param>
        private void SetGreenLight(int road)
        {
            for (int j = 0; j < stopWaypoints[road].redLightObjects.Count; j++)
            {
                if (stopWaypoints[road].redLightObjects[j] != null)
                {
                    stopWaypoints[road].redLightObjects[j].SetActive(false);
                }
            }

            for (int j = 0; j < stopWaypoints[road].yellowLightObjects.Count; j++)
            {
                if (stopWaypoints[road].yellowLightObjects[j] != null)
                {
                    stopWaypoints[road].yellowLightObjects[j].SetActive(false);
                }
            }

            for (int j = 0; j < stopWaypoints[road].greenLightObjects.Count; j++)
            {
                if (stopWaypoints[road].greenLightObjects[j] != null)
                {
                    stopWaypoints[road].greenLightObjects[j].SetActive(true);
                }
            }
        }


        /// <summary>
        /// Correctly increment the road number
        /// </summary>
        /// <param name="roadNumber"></param>
        /// <returns></returns>
        private int GetValidValue(int roadNumber)
        {
            if (roadNumber >= nrOfRoads)
            {
                roadNumber = roadNumber % nrOfRoads;
            }
            if (roadNumber < 0)
            {
                roadNumber = nrOfRoads + roadNumber;
            }
            return roadNumber;
        }
    }
}
