using System;
using UnityEngine;
using UnityEngine.Serialization;

public class GridMaker: MonoBehaviour {
    
    public Vector3 originPos;
    public int width = 5;
    public int height = 5;
    public int fontSize = 10;
    public float cellSize = 2f;
    
    private Grid _grid;
    private MouseRayCaster _mouseRayCaster = new MouseRayCaster();
    
    private void Start()
    {
        _grid = new Grid(width: width, height: height, cellSize, fontSize, originPos: originPos, parent: this.transform);
    }

    private void Update() {
        
        _mouseRayCaster.CastNewRay();
        
        if (Input.GetMouseButton(0) && _mouseRayCaster.HasHitTag("Cell")) {
            _grid.SetCellValue(_mouseRayCaster.hit.point, 53);
        }
        else if (Input.GetMouseButton(1) && _mouseRayCaster.HasHitTag("Cell")) {
            Debug.Log($"acertou a cell {_mouseRayCaster.GameObjHit.name}:" +
                      $" {_grid.GetCellValue(_mouseRayCaster.hit.point)}");
        }
        
    }
    
}
