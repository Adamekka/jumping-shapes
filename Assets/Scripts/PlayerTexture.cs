using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
public class PlayerTexture : MonoBehaviour {
    [System.Serializable]
    private struct JSONColor {
        public byte r;
        public byte g;
        public byte b;
        public float a;

        public JSONColor(byte r, byte g, byte b, float a) {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }
    }

    [System.Serializable]
    private struct PlayerTextureJSON {
        public JSONColor primary;
        public JSONColor secondary;
        public ushort size;
        public byte[,] texture;
    }

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private PlayerTextureJSON PlayerTextureJSON_init(string file) {
        Debug.Log("Loading Player Texture...");

        string json = System.IO.File.ReadAllText(file);
        Debug.Log(json);
        PlayerTextureJSON playerTextureJSON = JsonConvert.DeserializeObject<PlayerTextureJSON>(json);

        Debug.Log($"Primary Color: RGB({playerTextureJSON.primary.r}, {playerTextureJSON.primary.g}, {playerTextureJSON.primary.b}), Alpha: {playerTextureJSON.primary.a}");
        Debug.Log($"Secondary Color: RGB({playerTextureJSON.secondary.r}, {playerTextureJSON.secondary.g}, {playerTextureJSON.secondary.b}), Alpha: {playerTextureJSON.secondary.a}");
        Debug.Log($"Size: {playerTextureJSON.size}");

        Debug.Log("Texture:");
        Debug.Log(playerTextureJSON.texture);
        for (ushort row = 0; row < playerTextureJSON.size; row++) {
            for (ushort col = 0; col < playerTextureJSON.size; col++) {
                Debug.Log(playerTextureJSON.texture[row, col]);
            }
        }

        return playerTextureJSON;
    }

    private Texture2D CreateTexture(ref PlayerTextureJSON playerTextureJSON) {
        ushort size = playerTextureJSON.size;
        byte[,] textureData = playerTextureJSON.texture;

        Texture2D texture = new(size, size);

        ushort offset = (ushort)(size - 1);

        for (ushort y = 0; y < size; y++) {
            for (ushort x = 0; x < size; x++) {
                Debug.Log(y);
                Debug.Log(x);
                byte pixelValue = textureData[-y + offset, x];
                Color color = pixelValue == 1 ? GetColor(ref playerTextureJSON.primary) : GetColor(ref playerTextureJSON.secondary);
                texture.SetPixel(x, y, color);
            }
        }

        // Scale texture
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;

        texture.Apply();

        return texture;
    }

    private Color GetColor(ref JSONColor jsonColor) {
        float r = jsonColor.r / 255f;
        float g = jsonColor.g / 255f;
        float b = jsonColor.b / 255f;
        float a = jsonColor.a;
        return new Color(r, g, b, a);
    }

    private void ApplyTextureToSpriteRenderer(ref Texture2D texture) {
        if (spriteRenderer == null) {
            Debug.LogError("SpriteRenderer is not assigned");
            return;
        }

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

        float desiredScale = 128f;
        float width = texture.width;

        float newWidth = desiredScale / width;

        spriteRenderer.sprite = sprite;
        spriteRenderer.transform.localScale = new Vector3(newWidth, newWidth, 1f);
    }

    private void Start() {
        // Read JSON
        PlayerTextureJSON json = PlayerTextureJSON_init("Assets/Textures/Player/default.player.json");

        // Create texture from JSON
        Texture2D texture = CreateTexture(ref json);

        // Apply texture
        ApplyTextureToSpriteRenderer(ref texture);
    }
}
