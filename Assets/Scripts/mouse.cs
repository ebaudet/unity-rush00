using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class mouse : MonoBehaviour {

    public Texture2D cursor;

    void Start ()
    {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
	}
}
