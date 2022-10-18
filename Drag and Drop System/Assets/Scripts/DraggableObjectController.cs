using System;
using UnityEngine;

public class DraggableObjectController : MonoBehaviour
{
    // =================================================================================================================
    // COLLISION CHECKING SECTION
    // =================================================================================================================
    
    [NonSerialized] public bool IsDropAllowed;
    [NonSerialized] public bool IsCollingWithAnotherObj;
    [NonSerialized] public bool IsHittingTheGround;
    [NonSerialized] public string OtherObjName;    // used only in DebuggingCanvas script
    [NonSerialized] public bool AmITheOne = false; // am i the selected one to be dragged
    
    private void AllowDrop(bool value) => IsDropAllowed = value;
   
    private void OnTriggerStay(Collider other)
    {
        if (AmITheOne)
        {
            if (other.gameObject.CompareTag("Drag"))
            {
                IsCollingWithAnotherObj = true;
                OtherObjName = other.gameObject.name;
            }
            if (other.gameObject.CompareTag("Ground")) IsHittingTheGround = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (AmITheOne)
        {
            if (other.gameObject.CompareTag("Drag")) IsCollingWithAnotherObj = false;
            if (other.gameObject.CompareTag("Ground")) IsHittingTheGround = false;
        }
    }
    
    // =================================================================================================================
    //  DRAG AND DROP BASED ON COLLISION CHECKING SECTION
    // =================================================================================================================
    
    private float _flyingHeight;
    private float _dropHeight;
    private Vector3 _previousPos;
    private Vector3 _screenPos;
    private Vector3 _worldPos;
    
    public void Init(float flyingHeight, float dropHeight) // INITS OR SETS ALL DEPENDENCIES FOR THE GAME OBJ BE DRAGGED: called at DragAndDropManager.cs
    {
        _flyingHeight = flyingHeight;
        _dropHeight = dropHeight;
        _previousPos = transform.position; 
        AmITheOne = true; 
        Cursor.visible = false;
        PosIndicatorMaker.Create(this.gameObject);
    }

    public void ResetCollisionChecker() // called when the gmObj is dropped at a prohibited zone and repositioned at previousPos in DragAndDropController.cs
    {                                   // it must have those value to be set in order to allowDrop, otherwise it will bug a lot
        IsHittingTheGround = true;
        IsCollingWithAnotherObj = false;
    }
    
    private void Stop() // AFTER HAVING THE OBJ DROPPED, CLEANS THE ROOM FOR ANOTHER ONE
    {
        AmITheOne = false;
        ResetCollisionChecker();
        OtherObjName = "null";
        Cursor.visible = true;
        PosIndicatorMaker.FinishIndicator();
        DragAndDropManager.EndSelectedObj(); // simply cuts the link with the manager calling the method that will unassign the SelectObj and SelectedObjScript
    }
    
    private void SetPos(Vector3 pos) => GetComponent<Rigidbody>().position = pos;

    private void FixedUpdate()
    {
        if (AmITheOne)
        {
            if (IsHittingTheGround && !IsCollingWithAnotherObj) AllowDrop(true);
            else AllowDrop(false);

            if (Input.GetMouseButton(0))                                          // when the mouse button is held down: moves it
            {   
                _screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
                _worldPos = Camera.main.ScreenToWorldPoint(_screenPos);
                SetPos(new Vector3(_worldPos.x, _flyingHeight, _worldPos.z));
            }
            else                                                                   // when not: drops it
            {
                if (!IsDropAllowed)                                                // 1) in case the player drops the obj in a not allowed position (red indicator color):
                {
                    SetPos(_previousPos);                                          // 2) sets the gmObj back to the previous valid pos 
                    ResetCollisionChecker();                                       // 3) reset the collision checker (bcs this breaks in its pos breaks the Checker a lot)
                }
                else SetPos(new Vector3(_worldPos.x, _dropHeight, _worldPos.z));   // 4) in case not, drops it where it was released
                Stop();                                                            // 5) cleans the room for the next and cuts the link with the DragAndDropManager.cs
            }
        }
    }
    
}