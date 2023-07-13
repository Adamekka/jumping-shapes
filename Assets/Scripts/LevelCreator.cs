using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

[System.Serializable]
public class LevelCreator : MonoBehaviour {
    [System.Serializable]
    private struct Version {
        public uint major;
        public uint minor;
        public uint patch;
    }

    [System.Serializable]
    private struct Coords {
        public int x;
        public int y;
    }

    [System.Serializable]
    [JsonConverter(typeof(StringEnumConverter))]
    private enum BlockName {
        basic
    }

    [System.Serializable]
    private struct Block {
        public ulong id;
        public BlockName name;
        public Coords pos;
        public Coords size;
    }

    [System.Serializable]
    private struct LevelJSON {
        public ulong id;
        public string name;
        public string author;
        public Version version;
        public string description;
        public string song;
        public string background;
        public byte difficulty;
        public Coords playerStartingPos;
        public Block[] blocks;
    }

    private LevelJSON LevelJSON_init(string file) {
        Debug.Log("Loading level...");

        string json = System.IO.File.ReadAllText(file);
        Debug.Log(json);
        return JsonConvert.DeserializeObject<LevelJSON>(json);
    }

    private void CreateLevel(ref LevelJSON levelJSON) {
        SpriteRenderer spriteRenderer;
        Vector2 pivot = Vector2.one * 0.5f;

        foreach (var block in levelJSON.blocks) {
            Texture2D texture = new(block.size.x, block.size.y);
            byte[] image;

            switch (block.name) {
                case BlockName.basic:
                    image = System.IO.File.ReadAllBytes("Assets/Textures/Blocks/basic.png");
                    break;

                default:
                    Debug.LogError($"Block.name {block.name} doesn't exist");
                    continue;
            }

            texture.filterMode = FilterMode.Point;
            texture.LoadImage(image);

            GameObject gameObject = new("Block" + block.id);
            gameObject.transform.SetParent(transform);

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), pivot);

            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
            gameObject.GetComponent<SpriteRenderer>().material.mainTexture = texture;

            float desiredSize = 100f;
            float newScaleX = desiredSize / texture.width * block.size.x;
            float newScaleY = desiredSize / texture.height * block.size.y;

            gameObject.transform.localPosition = new Vector3(block.pos.x, block.pos.y, 0);
            gameObject.transform.localScale = new Vector3(newScaleX, newScaleY, 1);
        }
    }

    private void Start() {
        LevelJSON json = LevelJSON_init("Assets/Levels/first.level.json");
        CreateLevel(ref json);
    }
}
