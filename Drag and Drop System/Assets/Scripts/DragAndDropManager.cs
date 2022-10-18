using UnityEngine;

public class DragAndDropManager : MonoBehaviour {
    
    // DRAG N' DROP RELATED
    private static bool Clicked => Input.GetMouseButtonDown(0);

    // RAY CAST RELATED
    private readonly MouseRayCaster _mouseRay = new MouseRayCaster();
    [SerializeField] private LineRenderer lineRenderer; // draws on game view the ray from the main camera
    
    // DRAGGABLE GAME OBJECT RELATED
    public static GameObject SelectedObj;
    public static DraggableObjectController SelectedObjScript;
    public static bool IsObjNull => SelectedObj == null;
    
    // DRAGGABLE GAME OBJECT POSITION RELATED
    [SerializeField] private float flyingHeight = 2f;
    [SerializeField] private float dropHeight = 0.5f;
    
    private void Update()
    {                                                                                           // TRIES TO ASSIGN A GM_OBJ WHEN MOUSE BTN IS CLICKED, WHEN BTN IS RELEASED, GM_OBJ IS DROPPED
        _mouseRay.CastNewRay();                                                                 // 1) creates a RaycastHit based on the mouse click to check if it has hit anything
        lineRenderer.SetPosition(1, _mouseRay.ray.direction * 1000);                 // 2) draws on secondary camara the casted ray
        if (IsObjNull && Clicked && _mouseRay.HasHitTag("Drag")) InitDrag(_mouseRay.gmObjHit);  // 3) if gmObj isn't assigned, mouse btn clicked and the ray has hit a Grag tagged gmObj: sets up things for dragging it
    }

    private void InitDrag(GameObject gmObj)
    {                                                                                           // INITS OR SETS ALL DEPENDENCIES FOR THE GAME OBJ BE DRAGGED
        SelectedObj = gmObj;                                                                    // 1) assigns the global field of the object 
        SelectedObjScript = SelectedObj.GetComponent<DraggableObjectController>();              // 2) holds the selected game obj script
        SelectedObjScript.Init(flyingHeight, dropHeight);                                       // 3) inits the hit Drag tagged Game Object to be dragged
    }
    
    public static void EndSelectedObj()
    {                                                                                           // UNASSIGNS AS NULL OR SET ALL DEPENDENCIES FOR THE GAME OBJ BE DROPPED AND NOT DRAGGED ANYMORE (Called in the Game Object Script)
        SelectedObjScript.AmITheOne = false;                                                    // 1) tells the obj script that it isn't the one to be dragged anymore (it's done twice, oce at the obj script and another here for double checking)
        SelectedObjScript = null;                                                               // 2) unassigns the game obj script
        SelectedObj = null;                                                                     // 3) unassings the game obj
    }
    
}