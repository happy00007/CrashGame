using UnityEngine;

public static class WebGLImageUtils
{
    /// <summary>
    /// Converts a Texture2D to a Base64 string.
    /// </summary>
    /// <param name="texture">The Texture2D to convert.</param>
    /// <returns>Base64 string of the image.</returns>
    public static string TextureToBase64(Texture2D texture)
    {
        if (texture == null)
        {
            Debug.LogError("Texture is null. Cannot convert to Base64.");
            return null;
        }

        // Encode the texture to PNG format
        byte[] imageData = texture.EncodeToPNG();
        if (imageData == null)
        {
            Debug.LogError("Failed to encode Texture2D to PNG.");
            return null;
        }

        // Convert the PNG data to a Base64 string
        return System.Convert.ToBase64String(imageData);
    }
}
