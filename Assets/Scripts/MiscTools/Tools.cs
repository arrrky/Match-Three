using UnityEngine;

namespace MiscTools
{
    public class Tools
    {      
        public static Vector2 GetSpriteShift(GameObject tilePrefab)
        {
            SpriteRenderer spriteRenderer = tilePrefab.GetComponent<SpriteRenderer>();
            return new Vector2(spriteRenderer.bounds.extents.x, spriteRenderer.bounds.extents.y);
        }
    }
}

