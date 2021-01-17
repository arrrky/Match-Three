using UnityEngine;

namespace MiscTools
{
    public static class Tools
    {      
        public static Vector2 GetSpriteShift(GameObject tilePrefab)
        {
            SpriteRenderer spriteRenderer = tilePrefab.GetComponent<SpriteRenderer>();
            var bounds = spriteRenderer.bounds;
            return new Vector2(bounds.extents.x, bounds.extents.y);
        }
        
        public static Vector2 GetSpriteShift(Sprite sprite)
        {
            var bounds = sprite.bounds;
            return new Vector2(bounds.extents.x, bounds.extents.y);
        }
    }
}

