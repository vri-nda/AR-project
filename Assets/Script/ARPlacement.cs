using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARSubsystems;

public class ARPlacement : MonoBehaviour
{
    public GameObject arObjectToSpawn;
    public GameObject placementIndicator;
    public GameObject spawnedObject;

    private Pose PlacementPose;
    private ARRaycastManager aRRaycastManager;

    private bool placementPoseIsValid = false;



    void Start()
    {
         aRRaycastManager = FindObjectOfType<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnedObject == null && placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            ARPlaceObject();
        }

        UpdatePlacementPose();
        UpdatePlacementIndicator();

    }
    void UpdatePlacementIndicator()
    {
        if(spawnedObject == null && placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    void UpdatePlacementPose()
    {
        // throw new NotImplementedExpection();
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        // arOrigin.Raycast(screenCenter, hitsList, TrackableType.Planes);
        // arOrigin.Raycast(screenCenter, hits, TrackableType.Planes);
        aRRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            PlacementPose = hits[0].pose;
            // var cameraForward = Camera.current.transform.forward;
            // var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            // placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }

    }

    void ARPlaceObject()
    {
        spawnedObject =  Instantiate(arObjectToSpawn, PlacementPose.position, PlacementPose.rotation);
    }
}
