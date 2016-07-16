# EnfluxSuitController
**Documentation and tutorial updating, check back frequently**

* License subject to change
* All code provided **AS IS** with **NO WARRANTY**
* Enflux Inc. is not responsible for lost work from program crashes. 
* **MAKE SURE ALL WORK IS SAVED BEFORE RUNNING**

## UnitySDK
* Check [releases tab](https://github.com/Enflux/EnfluxSuitController/releases)

## Instructions for setup w/o headset
1. Download EnfluxVR Unity SDK
2. Start or open a Unity project
3. Under the tab "Assets" select  "Import Package > Custom Package"
4. Navigate to folder containing "EnfluxVR.unitypackage" and select package
5. A window will open, select "Import"
6. Under "Project" tab, expand "EnfluxVR > Resources > Prefabs"
7. Drag and drop the following into "Hierarchy"
  * "[EnfluxVR]"
  * "[EnfluxVRHumanoid]"
8. If one is not already in the project, add a "Canvas"
  * From "EnfluxVR > Resources > Prefabs", drag "EnfluxPanel" onto "Canvas"
  * **Dragging "EnfluxPanel" into "Scene" rather than "Hierarchy" may have incorrect result**
9. Move camera as needed

## Instructions for setup w/ HTC Vive headset
1. Obtain and import SteamVR Unity SDK
2. Under "Project" tab, expand "SteamVR"
  * From "SteamVR > Prefabs" drag "[CameraRig]" into "Hierarchy"
  * **Dragging into "Scene" may produce incorrect result"**
3. If not completed already, follow all **Instructions for setup w/o headset**
4. From "EnfluxVR > Resources > Prefabs" drag "SteamVRAdapter" into "Hierarchy"
5. In "Hierarchy", expand "[EnfluxVRHumanoid]" and "[CameraRig]"
  * Select "[EnfluxVRHumanoid]", then drag "[CameraRig] > "Camera (head)" onto "Hmd" object of "EVRHumanoidLimbMap"
  * Select "[EnfluxVRHumanoid] > EVRUpperLimbMap" and drag "Camera (head)" onto "Hmd" object of "EVRUpperLimbMap"
  * Select "[EnfluxVRHumanoid] > EVRLowerLimbMap" and drag "Camera (head)" onto "Hmd" object of "EVRLowerLimbMap"
6. In "Hierarchy" expand "[EnfluxVRHumanoid] > mike5 > Armature "
7. In "Hierarchy" select "SteamVRAdapter"
  * drag "[EnfluxVRHumanoid] > mike5 > Armature > waist " onto "waist" object of "SteamVRAdapter"
  * drag "[CameraRig] > Camera (head)" onto "Hmd" object of "SteamVRAdapter"



