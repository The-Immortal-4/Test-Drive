using System.Collections.Generic;
using Unity.Collections;
#if USE_GLEY_TRAFFIC
using Unity.Mathematics;
#endif
using UnityEngine;

namespace GleyTrafficSystem
{
    /// <summary>
    /// Controls the number of active vehicles
    /// </summary>
    public class DensityManager : MonoBehaviour
    {
#if USE_GLEY_TRAFFIC
        private TrafficVehicles trafficVehicles;
        private WaypointManager waypointManager;
        private CurrentSceneData currentSceneData;
        private int maxNrOfVehicles;
        private int currentnrOfVehicles;

        private NativeArray<float3> activeCameraPositions;




        /// <summary>
        /// Initializes the density manager script
        /// </summary>
        /// <param name="trafficVehicles"></param>
        /// <param name="waypointManager"></param>
        /// <param name="currentSceneData"></param>
        /// <param name="activeCameras"></param>
        /// <param name="maxNrOfVehicles">Is the maximum allowed number of vehicles in the current scene. It cannot be increased later</param>
        /// <returns></returns>
        public DensityManager Initialize(TrafficVehicles trafficVehicles, WaypointManager waypointManager, CurrentSceneData currentSceneData, NativeArray<float3> activeCameraPositions, int maxNrOfVehicles)
        {

            this.trafficVehicles = trafficVehicles;
            this.waypointManager = waypointManager;
            this.activeCameraPositions = activeCameraPositions;
            this.currentSceneData = currentSceneData;
            this.maxNrOfVehicles = maxNrOfVehicles;
            List<GridCell> gridCells = new List<GridCell>();
            for (int i = 0; i < activeCameraPositions.Length; i++)
            {
                gridCells.Add(currentSceneData.GetCell(activeCameraPositions[i].x, activeCameraPositions[i].z));
            }
            LoadInitialVehicles(gridCells);

            return this;
        }


        /// <summary>
        /// Change vehicle density
        /// </summary>
        /// <param name="nrOfVehciles">cannot be greater than max vehicle number set on initialize</param>
        public void UpdateMaxCars(int nrOfVehciles)
        {
            maxNrOfVehicles = nrOfVehciles;
        }


        /// <summary>
        /// Update active grid cells. Only active grid cells are allowed to perform additional operations like updating traffic lights  
        /// </summary>
        public void UpdateGrid(int level)
        {
            currentSceneData.UpdateActiveCells(activeCameraPositions, level);
        }


        /// <summary>
        /// Ads new vehicles if required
        /// </summary>
        public void UpdateVehicleDensity()
        {
            if (currentnrOfVehicles < maxNrOfVehicles)
            {
                int activeCameraIndex = UnityEngine.Random.Range(0, activeCameraPositions.Length);
                GridCell gridCell = currentSceneData.GetCell(activeCameraPositions[activeCameraIndex].x, activeCameraPositions[activeCameraIndex].z);
                AddVehicle(false, gridCell.row, gridCell.column);
            }
        }

        /// <summary>
        /// Remove a vehicle if required
        /// </summary>
        /// <param name="index">vehicle to remove</param>
        /// <param name="force">remove the vehicle even if not all conditions for removing are met</param>
        /// <returns>true if a vehicle was really removed</returns>
        public bool RemoveVehicle(int index, bool force)
        {
#if DEBUG_TRAFFIC
            return false;
#endif
            if (trafficVehicles.CanBeRemoved(index) || force == true)
            {
                currentnrOfVehicles--;
                AIEvents.TriggerVehicleChangedStateEvent(index, trafficVehicles.GetCollider(index), SpecialDriveActionTypes.Destroyed);
                trafficVehicles.DisableVehicle(index);
                waypointManager.RemoveTargetWaypoint(index);
                return true;
            }
            return false;
        }


        /// <summary>
        /// Update the active camera used to determine if a vehicle is in view
        /// </summary>
        /// <param name="activeCameras"></param>
        public void UpdateCameraPositions(NativeArray<float3> activeCameras)
        {
            activeCameraPositions = activeCameras;
        }


        /// <summary>
        /// Add all vehicles around the player even if they are inside players view
        /// </summary>
        /// <param name="currentGridRow"></param>
        /// <param name="currentGridColumn"></param>
        private void LoadInitialVehicles(List<GridCell> gridCells)
        {
            for (int i = 0; i < maxNrOfVehicles; i++)
            {
#if DEBUG_TRAFFIC
                AddVehicleDebug(i);
#else
                int cellIndex = UnityEngine.Random.Range(0, gridCells.Count);
                AddVehicle(true, gridCells[cellIndex].row, gridCells[cellIndex].column);
#endif
            }
        }


        /// <summary>
        /// Trying to load an idle vehicle if exists
        /// </summary>
        /// <param name="firstTime">initial load</param>
        /// <param name="currentGridRow"></param>
        /// <param name="currentGridColumn"></param>
        private void AddVehicle(bool firstTime, int currentGridRow, int currentGridColumn)
        {
            //TODO Optimize this process to never fail to add a new vehicle if requested
            int idleVehicleIndex = trafficVehicles.GetIdleVehicleIndex();

            //if an idle vehicle exists
            if (idleVehicleIndex != -1)
            {
                int freeWaypointIndex = waypointManager.GetFreeWaypoint(firstTime, currentGridRow, currentGridColumn, trafficVehicles.GetVehicleType(idleVehicleIndex), trafficVehicles.GetHalfVehicleLength(idleVehicleIndex), trafficVehicles.GetVehicleHeight(idleVehicleIndex), trafficVehicles.GetVehicleWidth(idleVehicleIndex), trafficVehicles.GetFrontWheelOffset(idleVehicleIndex));

                //if a free waypoint exists
                if (freeWaypointIndex != -1)
                {
                    VehicleComponent vehicle = trafficVehicles.GetIdleVehicle(idleVehicleIndex);
                    //if a vehicle is suitable for that waypoint
                    if (vehicle)
                    {
                        currentnrOfVehicles++;
                        int index = vehicle.GetIndex();
                        waypointManager.SetTargetWaypoint(index, freeWaypointIndex);
                        trafficVehicles.ActivateVehicle(vehicle, waypointManager.GetTargetPosition(index), waypointManager.GetTargetRotation(index));
                        DensityEvents.TriggerVehicleAddedEvent(index, waypointManager.GetTargetPosition(index), trafficVehicles.GetMaxSpeed(index), trafficVehicles.GetPowerStep(index), trafficVehicles.GetBrakeStep(index));
                    }
                }
            }
        }

#if DEBUG_TRAFFIC
        private void AddVehicleDebug(int waypointIndex)
        {
            int idleVehicleIndex = trafficVehicles.GetIdleVehicleIndex();

            if (idleVehicleIndex != -1)
            {
                VehicleComponent vehicle = trafficVehicles.GetIdleVehicle(idleVehicleIndex);
                if (vehicle)
                {
                    currentnrOfVehicles++;
                    int index = vehicle.GetIndex();
                    waypointManager.SetTargetWaypoint(index, waypointIndex);
                    trafficVehicles.ActivateVehicle(vehicle, waypointManager.GetTargetPosition(index), waypointManager.GetTargetRotation(index));
                    DensityEvents.TriggerVehicleAddedEvent(index, waypointManager.GetTargetPosition(index), trafficVehicles.GetMaxSpeed(index), trafficVehicles.GetPowerStep(index), trafficVehicles.GetBrakeStep(index));
                }
            }
        }
#endif
#endif
    }

}