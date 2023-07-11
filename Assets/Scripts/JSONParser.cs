using UnityEngine;

[System.Serializable]
public static class JSONParser {
    [System.Serializable]
    public struct JSONColor {
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

    public static Color GetColor(JSONColor jsonColor) {
        float r = jsonColor.r / 255f;
        float g = jsonColor.g / 255f;
        float b = jsonColor.b / 255f;
        float a = jsonColor.a;
        return new Color(r, g, b, a);
    }
}
