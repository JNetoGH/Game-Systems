using System.Globalization;
using TMPro;
using UnityEngine;

public class DebuggingCanvasController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameVar;
    [SerializeField] private TextMeshProUGUI isCollidingVar;
    [SerializeField] private TextMeshProUGUI isCollidingWithVar;
    [SerializeField] private TextMeshProUGUI isOnFloorVar;
    [SerializeField] private TextMeshProUGUI xVar;
    [SerializeField] private TextMeshProUGUI zVar;

    private void Update()
    {
        nameVar.text = DragAndDropManager.IsObjNull ? "null" : DragAndDropManager.SelectedObj.name;
        isCollidingVar.text = DragAndDropManager.IsObjNull ? "null" : DragAndDropManager.SelectedObjScript.IsCollingWithAnotherObj.ToString();
        isCollidingWithVar.text = DragAndDropManager.IsObjNull ? "null" : DragAndDropManager.SelectedObjScript.IsCollingWithAnotherObj ? DragAndDropManager.SelectedObjScript.OtherObjName : "null";
        isOnFloorVar.text = DragAndDropManager.IsObjNull ? "null" : DragAndDropManager.SelectedObjScript.IsHittingTheGround.ToString();
        xVar.text = DragAndDropManager.IsObjNull ? "null" : DragAndDropManager.SelectedObj.transform.position.x.ToString(CultureInfo.InvariantCulture);
        zVar.text = DragAndDropManager.IsObjNull ? "null" : DragAndDropManager.SelectedObj.transform.position.z.ToString(CultureInfo.InvariantCulture);
    }
}