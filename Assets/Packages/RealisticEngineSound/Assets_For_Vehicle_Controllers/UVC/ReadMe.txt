If you want to use Realistic Engine Sounds with Universal Vehicle Controller,
import Universal Vehicle Controller (make sure you done all required steps to make UVC work), then import Realistic Engine Sounds and later UVC_RES-Lite.unitypackage.
This unitypackage contain demo scenes, scripts for demo scenes and prefabs for Universal Vehicle Controller. This prefabs are ready to use with Universal Vehicle Controller.
UVC car will still play it's own engine sound, because it is using fmod and I not found any possibilities to turn it off inside the demo scenes without dissabling all other sounds too like surface sounds. If you found a better way or the asset got an update that will allow better sound customisation inside the demo scenes, let me know.
You can edit the fmod project to disable the stock engine sound or edit UVC's "CarSFX.cs" script by removing the following lines to stop sending rpm values to fmod, in this case the idle sound will still play in the background:

EngineEmitter.SetParameter (RPMID, Car.EngineRPM);
EngineEmitter.SetParameter (LoadID, Car.EngineLoad.Clamp (-1, 1));

At the time writing this tutorial, these line are 112 and 113. "CarSFX.cs" script can be found in: .\UniversalVehicleController\Scripts\GamePlay\VehicleComponents\Car\ folder.

If you don't own Universal Vehicle Controller, you can buy it here: https://assetstore.unity.com/packages/templates/systems/universal-vehicle-controller-176314?aid=1101l3r5B