using System;
using UnityEngine;

public class GridImplementation: MonoBehaviour {


    public Vector3 _originPos; 
    
    private Grid _grid;
    private MouseRayCaster _mouseRayCaster = new MouseRayCaster();
    
    private void Start()
    {
        _grid = new Grid(width: 10, height: 10, 2f, 10, originPos: _originPos, parent: transform);
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
