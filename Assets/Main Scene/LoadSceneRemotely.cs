using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneRemotely : MonoBehaviour
{
    private static string baseURL = "https://teadrinker.github.io/test-unity-webgl-build/build_bundles/WebGL/";

    public string scene_name = "test";

    private string ForceNoCache() { return "?id=" + Random.Range(1000000, 9999999);  } 
    IEnumerator Start()
    {
        //var baseURLf = "C:\\_UnityProj\\Bundletest\\build_bundles\\Windows\\";
        //var resources = AssetBundle.LoadFromFile(baseURLf + url);
        //var scene = AssetBundle.LoadFromFile(baseURLf + url +"s");

        var request = UnityEngine.Networking.UnityWebRequestAssetBundle.GetAssetBundle(baseURL + scene_name + ".bundle" + ForceNoCache());
        yield return request.SendWebRequest();        
        
        /*
        //var request = UnityEngine.Networking.UnityWebRequest.Get(baseURL + url + ForceNoCache());
        yield return request.SendWebRequest();
        var data = request.downloadHandler.data;
        Debug.Log(baseURL + url + " " + data.Length);

        var dataf = System.IO.File.ReadAllBytes(baseURLf + url);
        Debug.Log(baseURLf + url + " " + dataf.Length);
        */
        //var resources = AssetBundle.LoadFromMemoryAsync(request.downloadHandler.data);

        
        var resources = UnityEngine.Networking.DownloadHandlerAssetBundle.GetContent(request);
        var request2 = UnityEngine.Networking.UnityWebRequestAssetBundle.GetAssetBundle(baseURL + scene_name + "_s.bundle" + ForceNoCache());
        yield return request2.SendWebRequest();
        var scene = UnityEngine.Networking.DownloadHandlerAssetBundle.GetContent(request2);
        if (scene.isStreamedSceneAssetBundle)
        {
            string[] scenePaths = scene.GetAllScenePaths();
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePaths[0]);
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
        
        //AsyncOperationHandle<SceneInstance> handle1 = Addressables.LoadSceneAsync(url, LoadSceneMode.Additive, true);

        yield return null; 
    }
}
