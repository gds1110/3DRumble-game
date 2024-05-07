using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public enum Grade
{
    Normal,
    Uncommon,
    Rare,
    Legend,
}
public enum Size
{
    POT64,
    POT128,
    POT256,
    POT512,
    POT1024,
}

public class Capture : MonoBehaviour
{

    public Camera _cam;
    public RenderTexture _rt;
    public Image _bg;

    public Grade _grade;
    public Size _size;

    public GameObject[] _objs;
    public int _nowCount;
    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
        SettingColor();
        SettingSize();
     
    }

    public void Create()
    {
        StartCoroutine(CoCaptureImage());   
    }
    public void AllCreate()
    {
        StartCoroutine(CoAllCapture());

    }

    IEnumerator CoCaptureImage()
    {
        yield return null;

        Texture2D tex = new Texture2D(_rt.width, _rt.height, TextureFormat.ARGB32, false, true);
        RenderTexture.active = _rt;
        tex.ReadPixels(new Rect(0, 0, _rt.width, _rt.height), 0, 0);
        yield return null;

        var data = tex.EncodeToPNG();
        string name = "Thumbnail";
        string extention = ".png";
        string path = Application.persistentDataPath+ "/Thumbnail/";

        Debug.Log(path);

        if(!Directory.Exists(path)) Directory.CreateDirectory(path);

        File.WriteAllBytes(path+name+extention,data);
        yield return null;

    }
    IEnumerator CoAllCapture()
    {
        while(_nowCount<_objs.Length)
        {
            var nowObj = Instantiate(_objs[_nowCount].gameObject);
            yield return null;

            Texture2D tex = new Texture2D(_rt.width, _rt.height, TextureFormat.ARGB32, false, true);
            RenderTexture.active = _rt;
            tex.ReadPixels(new Rect(0, 0, _rt.width, _rt.height), 0, 0);
            yield return null;

            var data = tex.EncodeToPNG();
            string name = $"Thumbnail_{_objs[_nowCount].gameObject.name}";
            string extention = ".png";
            string path = Application.persistentDataPath + "/Thumbnail/";

            Debug.Log(path);

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            File.WriteAllBytes(path + name + extention, data);

            yield return null;
            DestroyImmediate(nowObj);
            _nowCount++;
            yield return null;
        }
    }
    void SettingColor()
    {
        switch (_grade)
        {
            case Grade.Normal:
                _cam.backgroundColor = Color.white;
                _bg.color = Color.white;
                break;
            case Grade.Uncommon:
                _cam.backgroundColor = Color.green;
                _bg.color = Color.green;
                break;
            case Grade.Rare:
                _cam.backgroundColor = Color.red;
                _bg.color = Color.red;
                break;
            case Grade.Legend:
                _cam.backgroundColor = Color.blue;
                _bg.color = Color.blue;
                break;
        }
    }
    void SettingSize()
    {
        switch (_size)
        {
            case Size.POT64:
                _rt.width = 64;
                _rt.height =64;
                break;
            case Size.POT128:
                _rt.width = 128;
                _rt.height = 128;
                break;
            case Size.POT256:
                _rt.width = 256;
                _rt.height = 256;
                break;
            case Size.POT512:
                _rt.width = 512;
                _rt.height = 512;
                break;
            case Size.POT1024:
                _rt.width = 1024;
                _rt.height = 1024;
                break;
        }
    }
}
