using UnityEngine;

namespace GleyTrafficSystem
{
    public static class Manager
    {
        /// <summary>
        /// Initialize the traffic
        /// </summary>
        /// <param name="activeCamera">camera that follows the player</param>
        /// <param name="nrOfVehicles">max number of traffic vehicles active in the same time</param>
        /// <param name="carPool">available vehicles asset</param>
        /// <param name="minDistanceToAdd">min distance from the player to add new vehicle</param>
        /// <param name="distanceToRemove">distance at which traffic vehicles can be removed</param>
        public static void Initialize(Transform activeCamera, int nrOfVehicles, VehiclePool carPool, float minDistanceToAdd, float distanceToRemove)
        {
#if USE_GLEY_TRAFFIC
            TrafficManager.Instance.Initialize(new Transform[] { activeCamera }, nrOfVehicles, carPool, minDistanceToAdd, distanceToRemove, 1, -1, -1);
#endif
        }


        /// <summary>
        /// Initialize the traffic
        /// </summary>
        /// <param name="activeCamera">camera that follows the player</param>
        /// <param name="nrOfVehicles">max number of traffic vehicles active in the same time</param>
        /// <param name="carPool">available vehicles asset</param>
        /// <param name="minDistanceToAdd">min distance from the player to add new vehicle</param>
        /// <param name="distanceToRemove">distance at which traffic vehicles can be removed</param>
        /// <param name="masterVolume">[-1,1] used to control the engine sound from your master volume</param>
        public static void Initialize(Transform activeCamera, int nrOfVehicles, VehiclePool carPool, float minDistanceToAdd, float distanceToRemove, float masterVolume)
        {
#if USE_GLEY_TRAFFIC
            TrafficManager.Instance.Initialize(new Transform[] { activeCamera }, nrOfVehicles, carPool, minDistanceToAdd, distanceToRemove, masterVolume, -1, -1);
#endif
        }


        /// <summary>
        /// Initialize the traffic
        /// </summary>
        /// <param name="activeCamera">camera that follows the player</param>
        /// <param name="nrOfVehicles">max number of traffic vehicles active in the same time</param>
        /// <param name="carPool">available vehicles asset</param>
        /// <param name="minDistanceToAdd">min distance from the player to add new vehicle</param>
        /// <param name="distanceToRemove">distance at which traffic vehicles can be removed</param>
        /// <param name="greenLightTime">roads green light duration in seconds</param>
        /// <param name="yelloLightTime">roads yellow light duration in seconds</param>
        public static void Initialize(Transform activeCamera, int nrOfVehicles, VehiclePool carPool, float minDistanceToAdd, float distanceToRemove, float greenLightTime, float yelloLightTime)
        {
#if USE_GLEY_TRAFFIC
            TrafficManager.Instance.Initialize(new Transform[] { activeCamera }, nrOfVehicles, carPool, minDistanceToAdd, distanceToRemove, 1, greenLightTime, yelloLightTime);
#endif
        }


        /// <summary>
        /// Initialize the traffic
        /// </summary>
        /// <param name="activeCamera">camera that follows the player</param>
        /// <param name="nrOfVehicles">max number of traffic vehicles active in the same time</param>
        /// <param name="carPool">available vehicles asset</param>
        /// <param name="minDistanceToAdd">min distance from the player to add new vehicle</param>
        /// <param name="distanceToRemove">distance at which traffic vehicles can be removed</param>
        /// <param name="masterVolume">[-1,1] used to control the engine sound from your master volume</param>
        /// <param name="greenLightTime">roads green light duration in seconds</param>
        /// <param name="yelloLightTime">roads yellow light duration in seconds</param>
        public static void Initialize(Transform activeCamera, int nrOfVehicles, VehiclePool carPool, float minDistanceToAdd, float distanceToRemove, float masterVolume, float greenLightTime, float yelloLightTime)
        {
#if USE_GLEY_TRAFFIC
            TrafficManager.Instance.Initialize(new Transform[] { activeCamera }, nrOfVehicles, carPool, minDistanceToAdd, distanceToRemove, masterVolume, greenLightTime, yelloLightTime);
#endif
        }


        /// <summary>
        /// Initialize the traffic
        /// </summary>
        /// <param name="activeCameras">cameras that follows the player in multiplayer/split screen setup</param>
        /// <param name="nrOfVehicles">max number of traffic vehicles active in the same time</param>
        /// <param name="carPool">available vehicles asset</param>
        /// <param name="minDistanceToAdd">min distance from the player to add new vehicle</param>
        /// <param name="distanceToRemove">distance at which traffic vehicles can be removed</param>
        /// <param name="masterVolume">[-1,1] used to control the engine sound from your master volume</param>
        /// <param name="greenLightTime">roads green light duration in seconds</param>
        /// <param name="yelloLightTime">roads yellow light duration in seconds</param>
        public static void Initialize(Transform[] activeCameras, int nrOfVehicles, VehiclePool carPool, float minDistanceToAdd, float distanceToRemove, float masterVolume, float greenLightTime, float yelloLightTime)
        {
#if USE_GLEY_TRAFFIC
            TrafficManager.Instance.Initialize(activeCameras, nrOfVehicles, carPool, minDistanceToAdd, distanceToRemove, masterVolume, greenLightTime, yelloLightTime);
#endif
        }


        /// <summary>
        /// Update active camera that is used to remove vehicles when are not in view
        /// </summary>
        /// <param name="activeCamera">represents the camera or the player prefab</param>
        public static void SetCamera(Transform activeCamera)
        {
#if USE_GLEY_TRAFFIC
            TrafficManager.Instance.UpdateCamera(new Transform[] { activeCamera });
#endif
        }


        /// <summary>
        /// Update active cameras that is used to remove vehicles when are not in view
        /// this is used in multiplayer/split screen setups
        /// </summary>
        /// <param name="activeCameras">represents the cameras or the players from your game</param>
        public static void SetCameras(Transform[] activeCameras)
        {
#if USE_GLEY_TRAFFIC
            TrafficManager.Instance.UpdateCamera(activeCameras);
#endif
        }


        /// <summary>
        /// Modify max number of active vehicles
        /// </summary>
        /// <param name="nrOfVehicles">new max number of vehicles, needs to be less than the initialization max number of vehicles</param>
        public static void SetTrafficDensity(int nrOfVehicles)
        {
#if USE_GLEY_TRAFFIC
            TrafficManager.Instance.SetTrafficDensity(nrOfVehicles);
#endif
        }


        /// <summary>
        /// Remove all vehicles on a range
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        public static void ClearTrafficOnArea(Vector3 center, float radius)
        {
#if USE_GLEY_TRAFFIC
            TrafficManager.Instance.ClearTrafficOnArea(center, radius);
#endif
        }


        /// <summary>
        /// Disable all waypoints on the specified area to stop vehicles to go in a certain area for a limited amount of time
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        public static void DisableAreaWaypoints(Vector3 center, float radius)
        {
#if USE_GLEY_TRAFFIC
            TrafficManager.Instance.DisableAreaWaypoints(center, radius);
#endif
        }


        /// <summary>
        /// Enable all disabled area waypoints
        /// </summary>
        public static void EnableAllWaypoints()
        {
#if USE_GLEY_TRAFFIC
            TrafficManager.Instance.EnableAllWaypoints();
#endif
        }


        /// <summary>
        /// Turn all vehicle lights on or off
        /// </summary>
        /// <param name="on">if true, lights are on</param>
        public static void UpdateVehicleLights(bool on)
        {
#if USE_GLEY_TRAFFIC
            TrafficManager.Instance.UpdateVehicleLights(on);
#endif
        }


        /// <summary>
        /// Control the engine volume from your master volume
        /// </summary>
        /// <param name="volume">current engine AudioSource volume</param>
        public static void SetEngineVolume(float volume)
        {
#if USE_GLEY_TRAFFIC
            TrafficManager.Instance.SetMasterVolume(volume);
#endif
        }


        /// <summary>
        /// Set how far away active intersections should be -> default is 1
        /// If set is to 2 -> intersections will update on a 2 square distance from the player
        /// </summary>
        /// <param name="level">how many squares away should intersections be updated</param>
        public static void SetActiveSquares(int level)
        {
#if USE_GLEY_TRAFFIC
            TrafficManager.Instance.SetActiveSquaresLevel(level);
#endif
        }


        /// <summary>
        /// Remove a specific vehicle from scene
        /// </summary>
        /// <param name="vehicleIndex">index of the vehicle to remove</param>
        public static void RemoveVehicle(int vehicleIndex)
        {
#if USE_GLEY_TRAFFIC
            TrafficManager.Instance.RemoveVehicle(vehicleIndex);
#endif
        }
    }
}