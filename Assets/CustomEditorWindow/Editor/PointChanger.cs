using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PointChanger: EditorWindow
{
    // gameobjects in scene (we are just editing existing ones)
    GameObject objectPoint1;
    GameObject objectPoint2;
    GameObject objectPoint3;
    GameObject groundPlane; // the ground plane
    GameObject mainCamera;  // main camera
    
    float minMaxX; // range for "X" slider, this is simply x scale of ground plane / 2
    float minMaxZ; // range for "Z" slider (height as 3D perspective game), this is simply Z scale of ground plane / 2
    float maxHeight; // max height (we restrict to above ground plane and a bit below camera in slider)

    // waypoint vectors
    Vector3 point1;
    Vector3 point2;
    Vector3 point3;

    // create menu structure
    [MenuItem("Custom Tools/Change Waypoints")]
    
    public static void ShowWindow()
    {
        // show the window
        GetWindow(typeof(PointChanger));  // method from EditorWindow class
    }

    private void OnEnable()
    {
        // find our waypoints
        objectPoint1 = GameObject.FindGameObjectWithTag("Point1");
        objectPoint2 = GameObject.FindGameObjectWithTag("Point2");
        objectPoint3 = GameObject.FindGameObjectWithTag("Point3");
        
        groundPlane = GameObject.FindGameObjectWithTag("GroundPlane"); // needed to determine boundaries of playfield for slider scales
        mainCamera  = GameObject.FindGameObjectWithTag("MainCamera");  // needed to determine a range for heights

        // copy the existing values into vectors
        point1 = objectPoint1.transform.position;
        point2 = objectPoint2.transform.position;
        point3 = objectPoint3.transform.position;

        // get values used to restrict slider positions according to current size of playfield
        minMaxX = (groundPlane.GetComponent<Transform>().localScale.x / 2) *10;  // for Unity 10x10 squares * (0,0,0) is centre
        minMaxZ = (groundPlane.GetComponent<Transform>().localScale.z / 2) *10;  // for Unity 10x10 squares & (0,0,0) is centre
        maxHeight = mainCamera.GetComponent<Transform>().position.y -15F; // camera height minus 15f to keep in view!
    }

    private void OnGUI()
    {
        // does this when active
        GUILayout.Label("Waypoint Edit Window", EditorStyles.boldLabel);
        GUILayout.Label("Different Y values produces a 3D Obstacle Field", EditorStyles.boldLabel);
        GUILayout.Label("which may look a little odd due to perspective!", EditorStyles.boldLabel);

        // use sliders to restrict values able to be entered by designer

        GUILayout.Label("Point 1");
        point1.x = EditorGUILayout.Slider("X", point1.x, -minMaxX, minMaxX);
        point1.y = EditorGUILayout.Slider("Y", point1.y,     1f, maxHeight); // restrict to be always lower than camera
        point1.z = EditorGUILayout.Slider("Z", point1.z, -minMaxZ, minMaxZ);

        GUILayout.Label("Point 2");
        point2.x = EditorGUILayout.Slider("X", point2.x, -minMaxX, minMaxX);
        point2.y = EditorGUILayout.Slider("Y", point2.y,     1f, maxHeight);
        point2.z = EditorGUILayout.Slider("Z", point2.z, -minMaxZ, minMaxZ);

        GUILayout.Label("Point 3");
        point3.x = EditorGUILayout.Slider("X", point3.x, -minMaxX, minMaxX);
        point3.y = EditorGUILayout.Slider("Y", point3.y,     1f, maxHeight);
        point3.z = EditorGUILayout.Slider("Z", point3.z, -minMaxZ, minMaxZ);

        // now have a confirm button for the changes
        if (GUILayout.Button("Click to CONFIRM changes!"))
        {
            Vector3 newPosition1 = new Vector3(point1.x, point1.y, point1.z);
            Vector3 newPosition2 = new Vector3(point2.x, point2.y, point2.z);
            Vector3 newPosition3 = new Vector3(point3.x, point3.y, point3.z);

            // update scene
            objectPoint1.transform.position = newPosition1;
            objectPoint2.transform.position = newPosition2;
            objectPoint3.transform.position = newPosition3;
        }
    }
}
