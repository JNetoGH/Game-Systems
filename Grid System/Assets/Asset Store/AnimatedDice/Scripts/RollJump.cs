using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollJump : MonoBehaviour
{
    // This list shows what dice are going to be and can be edited via code if you want or in the inspector. 
    [SerializeField] List<GameObject> diceGroup = new List<GameObject>();
    //Set what button is pressed to make the dice jump.
    [SerializeField] string buttonToJump="space";
    [SerializeField] float forceAmount = 400f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        JumpBehavior();
    }
    void JumpBehavior()
    {
        Rigidbody rb;
        if (Input.GetKeyDown(buttonToJump))
        {
            for (int i = 0; i < diceGroup.Count; i++)
            {
                diceGroup[i].transform.rotation = Random.rotation;
                rb = diceGroup[i].GetComponent<Rigidbody>();
                rb.AddForce(Vector3.up * forceAmount);
                rb.AddTorque(new Vector3(Random.value * forceAmount, Random.value * forceAmount, Random.value * forceAmount));
            }
        }
    }
}
   
