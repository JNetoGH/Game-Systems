using TMPro;
using UnityEngine;

public class Grid {
    
    private int _width;
    private int _height;
    private float _cellSize;
    private Vector3 _originPos;
    
    private int[,] _gridArray;             // holds the logical grid
    private GameObject[,] _cellGameObjects; // holds the text of the cell itself of the grid

    public Grid(int width, int height, float cellSize, int fontSize, Vector3 originPos = default, Transform parent = null) {
        
        this._width = width;
        this._height = height;
        this._cellSize = cellSize;
        this._originPos = originPos;
        
        _gridArray = new int[_width, _height];
        _cellGameObjects = new GameObject[_width, _height];
        
        for (int x = 0; x < _gridArray.GetLength(0); x++) {
            for (int z = 0; z < _gridArray.GetLength(1); z++) {
                
                string cellInnerText = $"{z},{x}";
                string cellInnerText2 = _gridArray[x, z].ToString(); //content
               
                // without a offset the content will be rendered at the corner of each cell
                Vector3 cellOffset = new Vector3(_cellSize, 0, _cellSize) * 0.5f; // 0.5 in order to the offset be the middle
                _cellGameObjects[x, z] = CreateWorldCell($"Cell({x},{z})", parent, cellInnerText,(GetCellWorldPos(x, z) + cellOffset), fontSize, Color.red, 5000);
                
                // draw the matrix lines
                Debug.DrawLine(GetCellWorldPos(x,z), GetCellWorldPos(x,z+1), Color.white, 100f);
                Debug.DrawLine(GetCellWorldPos(x,z), GetCellWorldPos(x+1,z), Color.white, 100f);
            }
        }
        
        // the last lines are not rendered in the loop
        Debug.DrawLine(GetCellWorldPos(0, _height), GetCellWorldPos(_width, _height), Color.white, 100f);
        Debug.DrawLine(GetCellWorldPos(_width, 0), GetCellWorldPos(_width, _height), Color.white, 100f);
    }

    // Gets a world position multiplying the index to the cells size
    private Vector3 GetCellWorldPos(float x, float z) {  
        return new Vector3(x, 0,z) * _cellSize + _originPos; // original(vertical one): return new Vector3(x, y) * _cellSize; 
    }
    
    // 2 out params because it needs to return 2 variables x and z
    private void GetWorldPosInGridCoordinates(Vector3 worldPos, out int x, out int z) {
        x = Mathf.FloorToInt((worldPos -  _originPos).x / _cellSize);
        z = Mathf.FloorToInt((worldPos -  _originPos).z / _cellSize);
    }
    
    // Ignores invalid values (out of range, or smaller than 0)
    public void SetCellValue(int x, int z, int value) {
        if (x >= 0 && z >= 0 && x < _width && z < _height) { 
            _gridArray[x, z] = value;
            _cellGameObjects[x, z].GetComponent<TextMesh>().text = _gridArray[x, z].ToString();
        }
    }
    
    // (WITHOUT ORIGIN POSITION: JUST WORKS IF THE GRID IS AT 0,0) same as above, but you can pass a worl postion and the method checks if its inside a cell
    public void SetCellValue(Vector3 worldPos, int value) { 
        int x, z;
        GetWorldPosInGridCoordinates(worldPos, out x, out z);
        SetCellValue(x, z, value);
    }

    // Ignores invalid values (out of range, or smaller than 0)
    public int GetCellValue(int x, int z) {
        if (x >= 0 && z >= 0 && x < _width && z < _height) { 
            return _gridArray[x, z];
        }
        else {
            return -1;
        }
    }
    
    //(WITHOUT ORIGIN POSITION: JUST WORKS IF THE GRID IS AT 0,0)
    public int GetCellValue(Vector3 worldPos) { 
        int x, z;
        GetWorldPosInGridCoordinates(worldPos, out x, out z);
        return GetCellValue(x, z);
    }
    
    
    public GameObject CreateWorldCell(string gmObjName, Transform parent, string text, Vector3 localPos, int fontSize, Color color, int sortingOrder) {
    
        GameObject gameObject = new GameObject(gmObjName, typeof(TextMeshPro), typeof(BoxCollider)/*I added this one*/);

        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPos;
        gameObject.tag = "Cell"; // a addition of mine, used for the collision checking
        
        // my changes on the transform
        transform.rotation = Quaternion.Euler(transform.rotation.x + 90, transform.rotation.y, transform.rotation.z); // My addition for my laying version, rotates 90 degrees in x
        transform.localPosition = new Vector3(localPos.x, 0, localPos.z); // another change of mine to set the local in y to 0, so the numbers don't get higher than their parent
        
        // My implementation for a bo collider
        BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
        boxCollider.center = Vector3.zero;
        boxCollider.size = new(this._cellSize, this._cellSize, 1); // its 1 in z not in y because the text was rotated 90 degrees in x
        
        // adds a text
        TextMeshPro textMesh = gameObject.GetComponent<TextMeshPro>();
        /*textMesh.anchor = textAnchor;*/
        textMesh.alignment = TextAlignmentOptions.Center;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        
        return gameObject;
    }
    
}

