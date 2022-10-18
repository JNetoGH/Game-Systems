using UnityEngine;

public class Grid {
    
    private int _width;
    private int _height;
    private float _cellSize;
    private Vector3 _originPos;
    
    private int[,] _gridArray;             // holds the logical grid
    private GameObject[,] _cells; // holds the text of the cell itself of the grid

    public Grid(int width, int height, float cellSize, int fontSize, Vector3 originPos = default, Transform parent = null) {
        
        this._width = width;
        this._height = height;
        this._cellSize = cellSize;
        this._originPos = originPos;
        
        _gridArray = new int[_width, _height];
        _cells = new GameObject[_width, _height];
        
        for (int x = 0; x < _gridArray.GetLength(0); x++) {
            for (int z = 0; z < _gridArray.GetLength(1); z++) {
               
                // without a offset the content will be rendered at the corner of each cell
                Vector3 cellOffset = new Vector3(_cellSize, 0, _cellSize) * 0.5f; // 0.5 in order to the offset be the middle
                _cells[x, z] = CreateWorldCell(_gridArray[x, z].ToString(), parent,
                    GetCellWorldPos(x, z) + cellOffset, fontSize: fontSize, color: Color.red,
                    textAnchor: TextAnchor.MiddleCenter, gmObjName: $"World_Text({x},{z})");
                
                // draw the matrix lines
                Debug.DrawLine(GetCellWorldPos(x,z), GetCellWorldPos(x,z+1), Color.white, 100f);
                Debug.DrawLine(GetCellWorldPos(x,z), GetCellWorldPos(x+1,z), Color.white, 100f);
            }
        }
        
        // the last lines are not rendered in the loop
        Debug.DrawLine(GetCellWorldPos(0, _height), GetCellWorldPos(_width, _height), Color.white, 100f);
        Debug.DrawLine(GetCellWorldPos(_width, 0), GetCellWorldPos(_width, _height), Color.white, 100f);
    }

    private Vector3 GetCellWorldPos(float x, float z) {  // gets a world position multiplying the index to the cells size
        return new Vector3(x, 0,z) * _cellSize + _originPos; // original(vertical one): return new Vector3(x, y) * _cellSize; 
    }
    
    // 2 out params because it needs to return 2 variables x and z
    private void GetWorldPosInGridCoordinates(Vector3 worldPos, out int x, out int z) {
        x = Mathf.FloorToInt((worldPos -  _originPos).x / _cellSize);
        z = Mathf.FloorToInt((worldPos -  _originPos).z / _cellSize);
    }
    
    public void SetCellValue(int x, int z, int value) {
        if (x >= 0 && z >= 0 && x < _width && z < _height) { // ignores invalid values (out of range, or smaller than 0)
            _gridArray[x, z] = value;
            _cells[x, z].GetComponent<TextMesh>().text = _gridArray[x, z].ToString();
        }
    }
    
    // (WITHOUT ORIGIN POSITION: JUST WORKS IF THE GRID IS AT 0,0) same as above, but you can pass a worl postion and the method checks if its inside a cell
    public void SetCellValue(Vector3 worldPos, int value) {
        int x, z;
        GetWorldPosInGridCoordinates(worldPos, out x, out z);
        SetCellValue(x, z, value);
    }

    public int GetCellValue(int x, int z) {
        if (x >= 0 && z >= 0 && x < _width && z < _height) { // ignores invalid values (out of range, or smaller than 0)
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
    
    private const int SortingOrderDefault = 5000;

    public GameObject CreateWorldCell(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = SortingOrderDefault, string gmObjName = "World_Text") {
        if (color == null) color = Color.white;
        return CreateWorldCell(gmObjName, parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }

    public GameObject CreateWorldCell(string gmObjName, Transform parent, string text, Vector3 localPos, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder) {
    
        GameObject gameObject = new GameObject(gmObjName, typeof(TextMesh), typeof(BoxCollider)/*I added this one*/);

        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPos;
        
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;

        // My addition for my laying version, rotates 90 degrees in x
        transform.rotation = Quaternion.Euler(transform.rotation.x + 90, transform.rotation.y, transform.rotation.z);
        // another change of mine to set the local in y to 0, so the numers do not get higher than their parent
        transform.localPosition = new Vector3(localPos.x, 0, localPos.z);
        // my changes to the collision
        gameObject.tag = "Cell";
        BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
        boxCollider.center = Vector3.zero;
        // its 1 in z not in y becase the text was turned arourd
        boxCollider.size = new(this._cellSize, this._cellSize, 1f);
        
        return gameObject;
    }

}
