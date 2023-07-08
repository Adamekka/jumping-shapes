using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
public class PlayerTexture : MonoBehaviour
{
    [System.Serializable]
    private struct JSONColor
    {
        public byte r;
        public byte g;
        public byte b;
        public float a;
    }

    [System.Serializable]
    private struct PlayerTextureJSON
    {
        public JSONColor primary;
        public JSONColor secondary;
        public ushort size;
        public byte[,] texture;
    }

    void Start()
    {
        Debug.Log("Loading Player Texture...");
        // Read JSON file
        string json = System.IO.File.ReadAllText("Assets/Textures/Player/default.player.json");
        Debug.Log(json);
        PlayerTextureJSON playerTextureJSON = JsonConvert.DeserializeObject<PlayerTextureJSON>(json);

        Debug.Log($"Primary Color: RGB({playerTextureJSON.primary.r}, {playerTextureJSON.primary.g}, {playerTextureJSON.primary.b}), Alpha: {playerTextureJSON.primary.a}");
        Debug.Log($"Secondary Color: RGB({playerTextureJSON.secondary.r}, {playerTextureJSON.secondary.g}, {playerTextureJSON.secondary.b}), Alpha: {playerTextureJSON.secondary.a}");
        Debug.Log($"Size: {playerTextureJSON.size}");

        Debug.Log("Texture:");
        Debug.Log(playerTextureJSON.texture);
        for (ushort row = 0; row < playerTextureJSON.size; row++)
        {
            for (ushort col = 0; col < playerTextureJSON.size; col++)
            {
                Debug.Log(playerTextureJSON.texture[row, col]);
            }
        }
    }

    void Update() { }
}
