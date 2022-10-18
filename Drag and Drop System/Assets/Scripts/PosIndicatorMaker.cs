using UnityEngine;

public class PosIndicatorMaker : MonoBehaviour
{
    // TARGET GAME OBJECT
    private static GameObject _gmObj;
    
    // POSITIONAL AND LOGICAL RELATED
    [SerializeField] private float distanceFromGround = 0.01f;
    public GameObject indicatorPlanePrefab;
    private GameObject _planeInstance;
    private static bool _hasInstantiated = false;
    
    // CHANGING COLORS RELATED
    private static Renderer _renderer;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material redMaterial;
    
    public static void Create(GameObject obj) =>  _gmObj = obj;
    public static void FinishIndicator() => _gmObj = null;

    private void ChangeIndicatorColor() 
    {   // IT ACTUALLY CHANGES THE PLANE MATERIAL BASED ON IF DROPPING IS ALLOWED OR NOT: set by the selected game obj on DragAndDropManager.cs
        _renderer = _planeInstance.GetComponent<Renderer>();
        _renderer.material = DragAndDropManager.SelectedObj.GetComponent<DraggableObjectController>().IsDropAllowed ? greenMaterial : redMaterial;
    }
    
    private void Indicate()
    {
        if (_gmObj != null)
        {
            if (!_hasInstantiated)
            {
                _planeInstance = Instantiate(indicatorPlanePrefab);
                _hasInstantiated = true;
            }
            Vector3 objPos = _gmObj.transform.position;
            _planeInstance.transform.position = new Vector3(objPos.x, distanceFromGround, objPos.z);
            ChangeIndicatorColor();
        }
        else if (_hasInstantiated)
        {
            Destroy(_planeInstance);
            _hasInstantiated = false;
        }
    }

    private void Update()
    {
        Indicate();
    }
}