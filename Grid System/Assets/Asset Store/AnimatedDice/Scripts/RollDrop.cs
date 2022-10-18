using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script allows the user to pickup predefined dice and then roll them. When you pick up the dice and
public class RollDrop : MonoBehaviour
{
    // This list shows what dice are going to be and can be edited via code if you want or in the inspector. 
    [SerializeField] List<GameObject> diceGroup = new List<GameObject>();
    //Height in which the dice will be picked up at. 
    [SerializeField] float pickUpHeight = 2;
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        PickupDropBehavior();
    }
    void PickupDropBehavior()
    {
        //Sets the dice to face a random direction when rolled.
        if(Input.GetMouseButtonUp(0))
            for (int i = 0; i < diceGroup.Count; i++)
            {
                diceGroup[i].transform.rotation = Random.rotation;
            }
        // When a user holds down the mouse button the dice will move towards the position of the mouse. You can adjust how high the dice will be if you want.
        if (Input.GetMouseButton(0))
        {
            Vector3 target = new Vector3 (0,0,0);
            RaycastHit hit;
            float speed = 40f;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                    target = hit.point;
                    print(hit.transform.position);
                    target.y = pickUpHeight;
            }
            for (int i = 0; i < diceGroup.Count; i++)
            {
               diceGroup[i].transform.LookAt(target);
               diceGroup[i].GetComponent<Rigidbody>().velocity = diceGroup[i].transform.forward * speed;
            }
        }
    }
}
