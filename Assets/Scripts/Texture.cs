using UnityEngine;

public static class Texture {
    public static void ApplyTextureToSpriteRenderer(ref SpriteRenderer spriteRenderer, ref Texture2D texture, float desiredSize) {
        if (spriteRenderer == null) {
            Debug.LogError("SpriteRenderer is not assigned");
            return;
        }

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

        float newSize = desiredSize / texture.width;

        spriteRenderer.sprite = sprite;
        spriteRenderer.transform.localScale = new Vector3(newSize, newSize, 1f);
    }
}
