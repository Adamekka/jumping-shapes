using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
public class PlayerTexture : MonoBehaviour {
    [System.Serializable]
    private struct PlayerTextureJSON {
        public JSONParser.JSONColor primary;
        public JSONParser.JSONColor secondary;
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
                Color color = pixelValue == 1 ? JSONParser.GetColor(playerTextureJSON.primary) : JSONParser.GetColor(playerTextureJSON.secondary);
                texture.SetPixel(x, y, color);
            }
        }

        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;

        texture.Apply();

        return texture;
    }


    private void Start() {
        // Read JSON
        PlayerTextureJSON json = PlayerTextureJSON_init("Assets/Textures/Player/default.player.json");

        // Create texture from JSON
        Texture2D texture = CreateTexture(ref json);

        // Apply texture
        Texture.ApplyTextureToSpriteRenderer(ref spriteRenderer, ref texture, 100f);
    }
}
