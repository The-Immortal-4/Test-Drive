using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GleyTrafficSystem
{
    /// <summary>
    /// Performs waypoint operations
    /// </summary>
    public class WaypointManager : MonoBehaviour
    {
        private PositionValidator positionValidator;
        private CurrentSceneData waypointsGrid;
        private Waypoint[] allWaypoints;
        private List<Waypoint> disabledWaypoints;
        private BlinkType[] blinkTypes;
        private int[] target;//contains the car index that has the waypoint as current waypoint
        private bool debugWaypoints;
        private bool debugDisabledWaypoints;


        /// <summary>
        /// Initialize waypoints
        /// </summary>
        /// <param name="positionValidator"></param>
        /// <param name="waypointsGrid"></param>
        /// <param name="nrOfVehicles"></param>
        /// <param name="debugWaypoints"></param>
        /// <param name="debugDisabledWaypoints"></param>
        /// <returns></returns>
        public WaypointManager Initialize(PositionValidator positionValidator, CurrentSceneData waypointsGrid, int nrOfVehicles, bool debugWaypoints, bool debugDisabledWaypoints)
        {
            this.waypointsGrid = waypointsGrid;
            this.debugWaypoints = debugWaypoints;
            this.debugDisabledWaypoints = debugDisabledWaypoints;
            this.positionValidator = positionValidator;
            allWaypoints = waypointsGrid.allWaypoints;
            disabledWaypoints = new List<Waypoint>();
            target = new int[nrOfVehicles];
            blinkTypes = new BlinkType[nrOfVehicles];
            WaypointEvents.onStopIndicatorChanged += ChangeStopValue;
            return this;
        }


        #region NextWaypoint
        //TODO Methods from this region can be simplified


        /// <summary>
        /// Directly set the target waypoint for the vehicle at index.
        /// Used to set first waypoint after vehicle initialization
        /// </summary>
        /// <param name="index"></param>
        /// <param name="freeWaypointIndex"></param>
        public void SetTargetWaypoint(int index, int freeWaypointIndex)
        {
            target[index] = freeWaypointIndex;
        }


        /// <summary>
        /// Set next waypoint for the vehicle index
        /// </summary>
        /// <param name="index">vehicle index</param>
        /// <param name="vehicleType">vehicle type</param>
        /// <param name="blink">blink required</param>
        /// <returns>true if waypoint was changed</returns>
        public bool SetNextWaypoint(int index, VehicleTypes vehicleType, bool blink)
        {
            bool returnValue = false;
            Waypoint oldWaypoint = GetWaypoint(target[index]);

            //if waypoint has multiple neighbors it might be possible to change lanes
            bool possibleLaneChange = false;
            if (oldWaypoint.neighbors.Count > 1)
            {
                possibleLaneChange = true;
            }
            oldWaypoint.Passed();

            //check direct neighbors
            if (oldWaypoint.neighbors.Count > 0)
            {
                Waypoint[] possibleWaypoints = GetAllWaypoints(oldWaypoint.neighbors).Where(cond => cond.allowedCars.Contains(vehicleType) && cond.temporaryDisabled == false).ToArray();
                if (possibleWaypoints.Length > 0)
                {
                    target[index] = possibleWaypoints[Random.Range(0, possibleWaypoints.Length)].listIndex;
                    returnValue = true;
                }
            }

            //check other lanes
            if (returnValue == false)
            {
                if (oldWaypoint.otherLanes.Count > 0)
                {
                    Waypoint[] possibleWaypoints = GetAllWaypoints(oldWaypoint.otherLanes).Where(cond => cond.allowedCars.Contains(vehicleType) && cond.temporaryDisabled == false).ToArray();
                    if (possibleWaypoints.Length > 0)
                    {
                        target[index] = possibleWaypoints[Random.Range(0, possibleWaypoints.Length)].listIndex;
                        returnValue = true;
                    }
                }
            }

            //check neighbors that are not allowed
            if (returnValue == false)
            {
                if (oldWaypoint.neighbors.Count > 0)
                {
                    Waypoint[] possibleWaypoints = GetAllWaypoints(oldWaypoint.neighbors).Where(cond => cond.temporaryDisabled == false).ToArray();
                    if (possibleWaypoints.Length > 0)
                    {
                        target[index] = possibleWaypoints[Random.Range(0, possibleWaypoints.Length)].listIndex;
                        returnValue = true;
                    }
                }
            }

            //check other lanes that are not allowed
            if (returnValue == false)
            {
                if (oldWaypoint.otherLanes.Count > 0)
                {
                    Waypoint[] possibleWaypoints = GetAllWaypoints(oldWaypoint.otherLanes).Where(cond => cond.temporaryDisabled == false).ToArray();
                    if (possibleWaypoints.Length > 0)
                    {
                        target[index] = possibleWaypoints[Random.Range(0, possibleWaypoints.Length)].listIndex;
                        returnValue = true;
                    }
                }
            }

            Waypoint newWaypoint = GetWaypoint(target[index]);
            //if waypoint changed
            if (returnValue == true)
            {
                if (newWaypoint.stop == true || newWaypoint.giveWay == true)
                {
                    WaypointEvents.TriggerWaypointStateChangedEvent(index, newWaypoint.stop, newWaypoint.giveWay);
                }
            }
            //stop blinking
            if (blink == false)
            {
                Blink(possibleLaneChange, index, oldWaypoint, newWaypoint, Vector3.zero, Vector3.zero);
            }
            return returnValue;
        }


        /// <summary>
        /// Used when a vehicle is in give way state to check if it can continue
        /// </summary>
        /// <param name="index"></param>
        /// <param name="vehicleType"></param>
        /// <param name="freeDistanceToCheck"></param>
        /// <returns></returns>
        public bool IsAllowedToChange(int index, VehicleTypes vehicleType, float freeDistanceToCheck)
        {
            Waypoint currentWaypoint = GetWaypoint(target[index]);
            if (currentWaypoint.IsInIntersection())
            {
                if (currentWaypoint.CanChange())
                {
                    if (SetNextWaypoint(index, vehicleType, false))
                    {
                        return true;
                    }
                }
                return false;
            }
            return ChangeLane(index, vehicleType, freeDistanceToCheck);
        }


        /// <summary>
        /// Check if a change of lane is possible
        /// Used to overtake and give way
        /// </summary>
        /// <param name="index"></param>
        /// <param name="vehicleType"></param>
        /// <param name="freeDistanceToCheck"></param>
        /// <returns></returns>
        public bool ChangeLane(int index, VehicleTypes vehicleType, float freeDistanceToCheck)
        {
            Waypoint[] possibleWaypoints = new Waypoint[0];
            //check other lane
            if (GetWaypoint(target[index]).otherLanes.Count > 0)
            {
                possibleWaypoints = GetAllWaypoints(GetWaypoint(target[index]).otherLanes).Where(cond => cond.allowedCars.Contains(vehicleType)).ToArray();
            }

            //if not available continue straight
            if (possibleWaypoints.Length == 0)
            {
                if (GetWaypoint(target[index]).neighbors.Count > 0)
                {
                    possibleWaypoints = GetAllWaypoints(GetWaypoint(target[index]).neighbors).Where(cond => cond.allowedCars.Contains(vehicleType)).ToArray();

                    //if cannot continue -> try other car types routes
                    if (possibleWaypoints.Length == 0)
                    {
                        possibleWaypoints = GetAllWaypoints(GetWaypoint(target[index]).neighbors);
                    }
                }
            }

            //if cannot continue -> force change lane
            if (possibleWaypoints.Length == 0)
            {
                if (GetWaypoint(target[index]).otherLanes.Count > 0)
                {
                    possibleWaypoints = GetAllWaypoints(GetWaypoint(target[index]).otherLanes);
                }
            }

            return CheckWaypoint(index, GetNrOfWaypointsToCheck(index, freeDistanceToCheck), possibleWaypoints);
        }


        /// <summary>
        /// Convert list index to play waypoint
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Waypoint[] GetAllWaypoints(List<int> index)
        {
            Waypoint[] result = new Waypoint[index.Count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = GetWaypoint(index[i]);
            }
            return result;
        }


        /// <summary>
        /// Check if the prev waypoints are free
        /// </summary>
        /// <param name="index"></param>
        /// <param name="freeWaypointsNeeded"></param>
        /// <param name="possibleWaypoints"></param>
        /// <returns></returns>
        private bool CheckWaypoint(int index, int freeWaypointsNeeded, Waypoint[] possibleWaypoints)
        {
            if (possibleWaypoints.Length > 0)
            {
                Waypoint nextWaypoint = possibleWaypoints[Random.Range(0, possibleWaypoints.Length)];
                if (IsTargetFree(nextWaypoint, freeWaypointsNeeded, GetWaypoint(target[index])))
                {
                    SetNextWaypoint(index, nextWaypoint);
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Check if prev waypoints are free
        /// </summary>
        /// <param name="waypoint"></param>
        /// <param name="level"></param>
        /// <param name="initialWaypoint"></param>
        /// <returns></returns>
        private bool IsTargetFree(Waypoint waypoint, int level, Waypoint initialWaypoint)
        {
#if UNITY_EDITOR
            if (debugWaypoints)
            {
                Debug.DrawLine(waypoint.position, initialWaypoint.position, Color.green, 1);
            }
#endif
            if (level == 0)
            {
                return true;
            }
            if (waypoint == initialWaypoint)
            {
                return true;
            }
            if (target.Contains(waypoint.listIndex))
            {
                if (waypoint.giveWay == false)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (waypoint.prev.Count <= 0)
                {
                    return true;
                }
                level--;
                if (waypoint.giveWay == false)
                {
                    for (int i = 0; i < waypoint.prev.Count; i++)
                    {
                        if (!IsTargetFree(GetWaypoint(waypoint.prev[i]), level, initialWaypoint))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }


        /// <summary>
        /// Set a waypoint as target
        /// </summary>
        /// <param name="index"></param>
        /// <param name="waypoint"></param>
        private void SetNextWaypoint(int index, Waypoint waypoint)
        {
            GetWaypoint(target[index]).Passed();
            target[index] = waypoint.listIndex;
            if (GetWaypoint(target[index]).stop == true || GetWaypoint(target[index]).giveWay == true)
            {
                WaypointEvents.TriggerWaypointStateChangedEvent(index, GetWaypoint(target[index]).stop, GetWaypoint(target[index]).giveWay);
            }
        }
        #endregion


        /// <summary>
        /// Converts index to waypoint
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Waypoint GetWaypoint(int index)
        {
            return allWaypoints[index];
        }


        /// <summary>
        /// Check what vehicle is in front
        /// </summary>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        /// <returns>
        /// 1-> if 1 is in front of 2
        /// 2-> if 2 is in front of 1
        /// 0-> if it is not possible to determine
        /// </returns>
        public int IsInFront(int index1, int index2)
        {
            //compares waypoints to determine which vehicle is in front 
            int distance = 0;
            //if no neighbors are available -> not possible to determine
            if (GetWaypoint(target[index1]).neighbors.Count == 0)
            {
                return 0;
            }

            //check next 10 waypoints to find waypoint 2
            int startWaypointIndex = GetWaypoint(target[index1]).neighbors[0];
            while (startWaypointIndex != target[index2] && distance < 10)
            {
                distance++;
                if (GetWaypoint(startWaypointIndex).neighbors.Count == 0)
                {
                    //if not found -> not possible to determine
                    return 0;
                }
                startWaypointIndex = GetWaypoint(startWaypointIndex).neighbors[0];
            }


            int distance2 = 0;
            if (GetWaypoint(target[index2]).neighbors.Count == 0)
            {
                return 0;
            }

            startWaypointIndex = GetWaypoint(target[index2]).neighbors[0];
            while (startWaypointIndex != target[index1] && distance2 < 10)
            {
                distance2++;
                if (GetWaypoint(startWaypointIndex).neighbors.Count == 0)
                {
                    //if not found -> not possible to determine
                    return 0;
                }
                startWaypointIndex = GetWaypoint(startWaypointIndex).neighbors[0];
            }

            //if no waypoints found -> not possible to determine
            if (distance == 10 && distance2 == 10)
            {
                return 0;
            }

            if (distance2 > distance)
            {
                return 2;
            }

            return 1;
        }


        /// <summary>
        /// Check if 2 vehicles have the same target
        /// </summary>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        /// <returns></returns>
        public bool IsSameTarget(int index1, int index2)
        {
            return target[index1] == target[index2];
        }


        /// <summary>
        /// Get a random free waypoint from a grid cell
        /// </summary>
        /// <param name="firstTime"></param>
        /// <param name="currentGridRow"></param>
        /// <param name="currentGridColumn"></param>
        /// <param name="carType"></param>
        /// <param name="halfCarLength"></param>
        /// <param name="halfCarHeight"></param>
        /// <returns></returns>
        public int GetFreeWaypoint(bool firstTime, int currentGridRow, int currentGridColumn, VehicleTypes carType, float halfCarLength, float halfCarHeight, float halfCarWidth, float frontWheelOffset)
        {
            int freeWaypointIndex;

            //get a free waypoint with the specified characteristics
            if (firstTime)
            {
                freeWaypointIndex = waypointsGrid.GetNeighborCellWaypoint(currentGridRow, currentGridColumn, Random.Range(0, 2), carType);
            }
            else
            {
                freeWaypointIndex = waypointsGrid.GetNeighborCellWaypoint(currentGridRow, currentGridColumn, 1, carType);
            }

            if (freeWaypointIndex != -1)
            {
                //if a valid waypoint was found, check if it was not manually disabled
                if (GetWaypoint(freeWaypointIndex).temporaryDisabled)
                {
                    return -1;
                }

                //check if the car type can be instantiated on selected waypoint
                if (positionValidator.IsValid(GetWaypoint(freeWaypointIndex).position, halfCarLength, halfCarHeight, halfCarWidth, firstTime, frontWheelOffset, GetOrientation(freeWaypointIndex)))
                {
                    return freeWaypointIndex;
                }
            }

            return -1;
        }


        /// <summary>
        /// Get position of the target waypoint
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector3 GetTargetPosition(int index)
        {
            return GetWaypoint(target[index]).position;
        }


        /// <summary>
        /// Get rotation of the target waypoint
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Quaternion GetTargetRotation(int index)
        {
            if (GetWaypoint(target[index]).neighbors.Count == 0)
            {
                return Quaternion.identity;
            }
            return Quaternion.LookRotation(GetWaypoint(GetWaypoint(target[index]).neighbors[0]).position - GetWaypoint(target[index]).position);
        }


        Quaternion GetOrientation(int index)
        {
            if (GetWaypoint(index).neighbors.Count == 0)
            {
                return Quaternion.identity;
            }
            return Quaternion.LookRotation(GetWaypoint(GetWaypoint(index).neighbors[0]).position - GetWaypoint(index).position);
        }

        /// <summary>
        /// Get waypoint speed
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public float GetMaxSpeed(int index)
        {
            return GetWaypoint(target[index]).maxSpeed;
        }


        /// <summary>
        /// Remove target waypoint for the vehicle at index
        /// </summary>
        /// <param name="index"></param>
        public void RemoveTargetWaypoint(int index)
        {
            target[index] = 0;
        }


        /// <summary>
        /// Converts distance to waypoint number
        /// </summary>
        /// <param name="index"></param>
        /// <param name="lookDistance"></param>
        /// <returns></returns>
        public int GetNrOfWaypointsToCheck(int index, float lookDistance)
        {
            Waypoint currentWaypoint = GetWaypoint(target[index]);
            float waypointDistance = 4;
            if (currentWaypoint.neighbors.Count > 0)
            {
                waypointDistance = Vector3.Distance(currentWaypoint.position, GetWaypoint(currentWaypoint.neighbors[0]).position);
            }
            return Mathf.CeilToInt(lookDistance / waypointDistance);
        }


        /// <summary>
        /// Switch the stop value of a waypoint
        /// </summary>
        /// <param name="waypointIndex"></param>
        private void ChangeStopValue(int waypointIndex)
        {
            allWaypoints[waypointIndex].stop = !allWaypoints[waypointIndex].stop;
            for (int i = 0; i < target.Length; i++)
            {
                if (target[i] == waypointIndex)
                {
                    WaypointEvents.TriggerWaypointStateChangedEvent(i, GetWaypoint(waypointIndex).stop, GetWaypoint(waypointIndex).giveWay);
                }
            }
        }


        #region Blinking
        //TODO Blinking should be controlled from another script
        /// <summary>
        /// Determine if blink is required
        /// </summary>
        /// <param name="possibleLaneChange"></param>
        /// <param name="index"></param>
        /// <param name="oldWaypoint"></param>
        /// <param name="newWaypoint"></param>
        /// <param name="oldPoz"></param>
        /// <param name="forward"></param>
        public void Blink(bool possibleLaneChange, int index, Waypoint oldWaypoint, Waypoint newWaypoint, Vector3 oldPoz, Vector3 forward)
        {
            float angle;

            //overtake
            if (oldWaypoint == null)
            {
                Vector3 currentWaypointPosition = GetWaypoint(target[index]).position;
                angle = Vector3.SignedAngle(forward, currentWaypointPosition - oldPoz, Vector3.up);
                SetBlinkType(angle, index);
                return;
            }

            Waypoint targetWaypoint = newWaypoint;
            //multiple lanes
            if (possibleLaneChange)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (targetWaypoint.neighbors.Count > 0)
                    {
                        targetWaypoint = GetWaypoint(targetWaypoint.neighbors[0]);
                    }
                }
                angle = Vector3.SignedAngle(oldWaypoint.position - GetWaypoint(oldWaypoint.prev[0]).position, targetWaypoint.position - oldWaypoint.position, Vector3.up);
                SetBlinkType(angle, index);
                return;
            }


            //giveWay
            if (newWaypoint.giveWay == true)
            {
                if (targetWaypoint.neighbors.Count > 0)
                {
                    targetWaypoint = GetWaypoint(targetWaypoint.neighbors[0]);
                }
                else
                {
                    if (targetWaypoint.otherLanes.Count > 0)
                    {
                        targetWaypoint = GetWaypoint(targetWaypoint.otherLanes[0]);
                    }
                }
                if (targetWaypoint != null)
                {
                    angle = Vector3.SignedAngle(oldWaypoint.position - GetWaypoint(target[index]).position, oldWaypoint.position - targetWaypoint.position, Vector3.up);
                    SetBlinkType(angle, index);
                }
                return;
            }

            //stop blinking              
            if (targetWaypoint.neighbors.Count > 0)
            {
                targetWaypoint = GetWaypoint(targetWaypoint.neighbors[0]);
                angle = Vector3.SignedAngle(oldWaypoint.position - GetWaypoint(target[index]).position, oldWaypoint.position - targetWaypoint.position, Vector3.up);
                if (Mathf.Abs(angle) < 1)
                {
                    blinkTypes[index] = BlinkType.Stop;
                }
            }
        }


        /// <summary>
        /// Returns blink type
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public BlinkType GetBlinkType(int index)
        {
            return blinkTypes[index];
        }


        /// <summary>
        /// Determine the blink direction
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="index"></param>
        private void SetBlinkType(float angle, int index)
        {
            if (Mathf.Abs(angle) < 1)
            {
                blinkTypes[index] = BlinkType.Stop;
            }
            else
            {
                if (angle > 5)
                {
                    blinkTypes[index] = BlinkType.BlinkRight;
                }
                else
                {
                    if (angle < -5)
                    {
                        blinkTypes[index] = BlinkType.BlinkLeft;
                    }
                }
            }
        }
        #endregion


        /// <summary>
        /// Update the camera for position validator
        /// </summary>
        /// <param name="activeCameras"></param>
        public void UpdateCamera(Transform[] activeCameras)
        {
            positionValidator.UpdateCamera(activeCameras);
        }


        /// <summary>
        /// Makes waypoints on a given radius unavailable
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        public void DisableAreaWaypoints(Vector3 center, float radius)
        {
            GridCell cell = waypointsGrid.GetCell(center);
            List<Vector2Int> neighbors = waypointsGrid.GetCellNeighbors(cell.row, cell.column, 1, false);
            for (int i = neighbors.Count - 1; i >= 0; i--)
            {
                cell = waypointsGrid.GetCell(neighbors[i]);
                for (int j = 0; j < cell.waypointsInCell.Count; j++)
                {
                    Waypoint waypoint = GetWaypoint(cell.waypointsInCell[j]);
                    if (Vector3.SqrMagnitude(center - waypoint.position) < radius)
                    {
                        disabledWaypoints.Add(waypoint);
                        waypoint.temporaryDisabled = true;
                    }
                }
            }
        }


        /// <summary>
        /// Enables unavailable waypoints
        /// </summary>
        public void EnableAllWaypoints()
        {
            for (int i = 0; i < disabledWaypoints.Count; i++)
            {
                disabledWaypoints[i].temporaryDisabled = false;
            }
            disabledWaypoints = new List<Waypoint>();
        }


        /// <summary>
        /// Cleanup
        /// </summary>
        private void OnDestroy()
        {
            WaypointEvents.onStopIndicatorChanged -= ChangeStopValue;
        }


        private void OnDrawGizmos()
        {
            if (debugWaypoints)
            {
                for (int i = 0; i < target.Length; i++)
                {
                    if (target[i] != 0)
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawSphere(GetWaypoint(target[i]).position, 1);
                    }
                }


            }
            if (debugDisabledWaypoints)
            {
                for (int i = 0; i < disabledWaypoints.Count; i++)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawSphere(disabledWaypoints[i].position, 1);
                }
            }
        }
    }
}