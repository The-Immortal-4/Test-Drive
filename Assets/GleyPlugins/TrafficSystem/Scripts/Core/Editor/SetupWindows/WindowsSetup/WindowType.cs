namespace GleyTrafficSystem
{
    public enum WindowType
    {
        SettingsWindow,

        //main windows
        ImportPackages,
        RoadSetup,
        WaypointSetup,
        SceneSetup,
        Debug,

        //road setup
        CreateRoad,
        EditRoad,
        ConnectRoads,
        ViewRoads,

        //waypoint setup
        ShowAllWaypoints,
        ShowDisconnectedWaypoints,
        ShowCarTypeEditedWaypoints,
        ShowSpeedEditedWaypoints,
        ShowGiveWayWaypoints,
        ShowStopWaypoints,
        ShowVehiclePathProblems,
        EditWaypoint,

        //scene setup
        GridSetupWindow,
        SpeedRoutesSetupWindow,
        CarRoutesSetupWindow,
        LayerSetupWindow,

        //Intersection
        IntersectionSetup,
        PriorityIntersection,
        TrafficLightsIntersection,

        //Car
        CarTypes,
    }
}
