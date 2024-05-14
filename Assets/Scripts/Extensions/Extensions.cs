using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public static class Extensions
{
    /// <summary>
    /// Remap a value to given range
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="newMin"></param>
    /// <param name="newMax"></param>
    /// <returns></returns>
    public static float Remap(this float value, float min, float max, float newMin, float newMax)
    {
        if (Mathf.Approximately(max, min)) return value;

        return newMin + (value - min) * (newMax - newMin) / (max - min);
    }


    /// <summary>
    /// Remap a value to 0-1 range
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static float Remap01(this float value, float min, float max)
    {
        return value.Remap(min, max, 0f, 1f);
    }



    /// <summary>
    /// Convert RenderTexture to Texture2D
    /// </summary>
    /// <param name="rTex"></param>
    /// <returns></returns>
    public static Texture2D ToTexture2D(this RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
        var old_rt = RenderTexture.active;
        RenderTexture.active = rTex;

        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();

        RenderTexture.active = old_rt;
        return tex;
    }



    /// <summary>
    /// Take a "screenshot" of a camera's Render Texture.
    /// </summary>
    /// <param name="camera"></param>
    /// <returns>s</returns>
    public static Texture2D RTImage(this Camera camera)
    {
        // The Render Texture in RenderTexture.active is the one
        // that will be read by ReadPixels.
        var currentRT = RenderTexture.active;
        RenderTexture.active = camera.targetTexture;

        // Render the camera's view.
        camera.Render();

        // Make a new texture and read the active Render Texture into it.
        Texture2D image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
        image.Apply();

        // Replace the original active Render Texture.
        RenderTexture.active = currentRT;
        return image;
    }



    /// <summary>
    /// Set the parent of the GameObject without caching it. Also destroys it if you give destroy time a value
    /// </summary>
    /// <param name="Parent"></param>
    /// <returns></returns>
    public static GameObject ParentSetAndDestroy(this GameObject obj, Transform parent, float destroyTime = 0f)
    {
        obj.transform.SetParent(parent);
        if (destroyTime > 0) MonoBehaviour.Destroy(obj, destroyTime);
        return obj;
    }



    /// <summary>
    /// Get the center point of the Transforms in the given list
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetCenterPoint(this List<Transform> targets)
    {
        if (targets.Count == 1) return targets[0].position;

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }



    /// <summary>
    /// Get a random integer except the given one
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="except"></param>
    /// <returns></returns>
    public static int RandomExcept(int min, int max, int except)
    {
        int random = UnityEngine.Random.Range(min, max);
        if (random >= except) random = (random + 1) % max;
        return random;
    }



    /// <summary>
    /// Return height of an object based on its collider
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static float GetColliderHeight(this Transform obj)
    {
        return obj.GetComponent<Collider>().bounds.extents.y * 2;
    }



    /// <summary>
    /// Return height of an object based on its collider
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static float GetColliderWidth(this Transform obj)
    {
        return obj.GetComponent<Collider>().bounds.extents.x * 2;
    }



    /// <summary>
    /// Parses string to Int *fast*
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int ParseToInt(this string value)
    {
        int result = 0;
        for (int i = 0; i < value.Length; i++)
        {
            char letter = value[i];
            result = 10 * result + (letter - 48);
        }
        return result;
    }



    /// <summary>
    /// Returns mouse positions that hits the specified _layerMask
    /// </summary>
    /// <param name="layerMask"></param>
    /// <returns></returns>
    public static Vector3 GetMouseWorldPosition(LayerMask layerMask)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, layerMask)) return raycastHit.point;
        else return Vector3.zero;
    }



    /// <summary>
    /// Fastly change X of a Vector3
    /// </summary>
    /// <param name="curVector"></param>
    /// <param name="newX"></param>
    /// <returns></returns>
    public static Vector3 ChangeX(this Vector3 curVector, float newX)
    {
        curVector.x = newX;
        return curVector;
    }



    /// <summary>
    /// Fastly change Y of a Vector3
    /// </summary>
    /// <param name="curVector"></param>
    /// <param name="newY"></param>
    /// <returns></returns>
    public static Vector3 ChangeY(this Vector3 curVector, float newY)
    {
        curVector.y = newY;
        return curVector;
    }



    /// <summary>
    /// Fastly change Z of a Vector3
    /// </summary>
    /// <param name="curVector"></param>
    /// <param name="newZ"></param>
    /// <returns></returns>
    public static Vector3 ChangeZ(this Vector3 curVector, float newZ)
    {
        curVector.z = newZ;
        return curVector;
    }



    /// <summary>
    /// Fastly change Z of a Vector3
    /// </summary>
    /// <param name="curVector"></param>
    /// <param name="newZ"></param>
    /// <returns></returns>
    public static Vector2 ChangeY2D(this Vector2 curVector, float newZ)
    {
        curVector.y = newZ;
        return curVector;
    }



    /// <summary>
    /// Returns a negative number if B is left of A, positive if right of A, or 0 if they are perfectly aligned
    /// </summary>
    /// <param name="A"></param>
    /// <param name="B"></param>
    /// <returns></returns>
    public static float AngleDir(Vector2 A, Vector2 B)
    {
        return -A.x * B.y + A.y * B.x;
    }



    /// <summary>
    /// Changes the maximum number of particles in the particle system.
    /// </summary>
    /// <param name="particle">The ParticleSystem to modify.</param>
    /// <param name="newMaxParticle">The new maximum number of particles.</param>
    public static void ChangeMaxParticle(this ParticleSystem particle, int newMaxParticle)
    {
        var main = particle.main;
        main.maxParticles = newMaxParticle;
    }



    /// <summary>
    /// Deletes all child objects of the given transform.
    /// </summary>
    /// <param name="transform">The Transform whose children will be deleted.</param>
    public static void DeleteChildren(this Transform transform)
    {
        foreach (Transform child in transform) GameObject.Destroy(child.gameObject);
    }



    /// <summary>
    /// Appends the given text to a StringBuilder and returns the result.
    /// </summary>
    /// <param name="text">The text to append.</param>
    /// <returns>A string with the appended text.</returns>
    public static string StringBuilderAppend(this string text)
    {
        StringBuilder _builder = new();
        _builder.Append(text);
        return _builder.ToString();
    }



    /// <summary>
    /// Retrieves or creates a WaitForSeconds object for the given time.
    /// </summary>
    /// <param name="time">The time to wait.</param>
    /// <returns>A WaitForSeconds object for the given time.</returns>
    private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new();
    public static WaitForSeconds GetWaitForSeconds(this float time)
    {
        if (WaitDictionary.TryGetValue(time, out var wait)) return wait;

        WaitDictionary[time] = new WaitForSeconds(time);
        return WaitDictionary[time];
    }



    /// <summary>
    /// Gets the world position of a UI canvas element.
    /// </summary>
    /// <param name="element">The RectTransform of the canvas element.</param>
    /// <returns>The world position of the canvas element.</returns>
    public static Vector2 GetWorldPositionOfCanvasElement(this RectTransform element)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(element, element.position, Camera.main, out var result);
        return result;
    }



    /// <summary>
    /// Adds or retrieves a component of the specified type from the GameObject.
    /// </summary>
    /// <typeparam name="T">The type of component to add or retrieve.</typeparam>
    /// <param name="source">The GameObject to add or retrieve the component from.</param>
    /// <returns>The component of type T attached to the GameObject.</returns>
    public static T AddOrGetComponent<T>(this GameObject source) where T : Component
    {
        var obj = source.GetComponent<T>() ?? source.AddComponent<T>();
        return obj;
    }



    /// <summary>
    /// Assigns and plays the given AudioClip on the AudioSource.
    /// </summary>
    /// <param name="source">The AudioSource to play the AudioClip.</param>
    /// <param name="clip">The AudioClip to play.</param>
    public static void PlayClip(this AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }



    /// <summary>
    /// Creates a copy of the given Mesh.
    /// </summary>
    /// <param name="mesh">The Mesh to copy.</param>
    /// <returns>A copy of the Mesh.</returns>
    public static Mesh CopyMesh(this Mesh mesh)
    {
        Mesh newMesh = new()
        {
            vertices = mesh.vertices,
            triangles = mesh.triangles,
            uv = mesh.uv,
            normals = mesh.normals,
            colors = mesh.colors,
            tangents = mesh.tangents
        };
        return newMesh;
    }




    /// <summary>
    /// Writes a numeric value with the specified number of digits after the decimal point.
    /// </summary>
    /// <typeparam name="T">The numeric type of the value.</typeparam>
    /// <param name="number">The numeric value to format.</param>
    /// <param name="digit">The number of digits after the decimal point.</param>
    /// <returns>The numeric value formatted with the specified number of digits.</returns>
    public static float WriteNDigit<T>(T number, int digit) where T : struct, IConvertible
    {
        if (!typeof(T).IsNumericType())
            throw new ArgumentException("Type T must be numeric.");

        float convertedNumber = Convert.ToSingle(number);

        return float.Parse(convertedNumber.ToString("N" + digit));
    }



    /// <summary>
    /// Checks if the given type is a numeric type.
    /// </summary>
    /// <param name="type">The Type to check.</param>
    /// <returns>True if the type is numeric; otherwise, false.</returns>
    public static bool IsNumericType(this Type type)
    {
        switch (Type.GetTypeCode(type))
        {
            case TypeCode.Byte:
            case TypeCode.SByte:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Single:
                return true;
            default:
                return false;
        }
    }


    /// <summary>
    /// Set the position and local rotation of the transform
    /// </summary>
    /// <param name="_transform"></param>
    /// <param name="_position"></param>
    /// <param name="_localEulerAngles"></param>
    public static void SetPositionAndLocalEulerAngles(this Transform _transform, Vector3 _position, Vector3 _localEulerAngles)
    {
        _transform.position = _position;
        _transform.localEulerAngles = _localEulerAngles;
    }


    // Get a random index that is not in the usedIndexes list
    /// </summary>
    /// <param name="usedIndexes"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public static int GetUniqueRandomIndex(List<int> usedIndexes, int range)
    {
        if (usedIndexes.Count >= range) return -1;

        int randomIndex = UnityEngine.Random.Range(0, range);
        while (usedIndexes.Contains(randomIndex))
        {
            randomIndex = UnityEngine.Random.Range(0, range);
        }
        usedIndexes.Add(randomIndex);
        return randomIndex;
    }


}