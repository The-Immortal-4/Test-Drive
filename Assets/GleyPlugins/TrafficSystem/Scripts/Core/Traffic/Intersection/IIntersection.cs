namespace GleyTrafficSystem
{
    /// <summary>
    /// Used to set the intersection on waypoint
    /// </summary>
    public interface IIntersection
    {
        bool IsPathFree(int waypointIndex);
        void VehicleLeft(Waypoint waypoint);
        void VehicleEnter(Waypoint waypoint);
        void Initialize(WaypointManager waypointManager, float greenLightTime, float yellowLightTime);
        void UpdateIntersection();
    }
}