using UnityEditor;
using UnityEngine;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("AssetBundles/Build For WebGL (will auto-assign asset bundle meta data!)")] static void BuildWebGL() { BuildAllAssetBundles(BuildTarget.WebGL, "WebGL"); }
    [MenuItem("AssetBundles/Build For Windows (will auto-assign asset bundle meta data!)")] static void BuildWin() { BuildAllAssetBundles(BuildTarget.StandaloneWindows, "Windows"); }


    static void BuildAllAssetBundles(UnityEditor.BuildTarget t, string name)
    {
        AutoAssignAssetBundleNames();

        string assetBundleDirectory = "build_bundles/" + name;
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, t);
    }

    static void AutoAssignAssetBundleNames()
    {
        var files = Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories);
        foreach (var filefull in files)
        {
            var relfile = "Assets/" + System.IO.Path.GetRelativePath(Application.dataPath, filefull).Replace('\\', '/');

            var asset = AssetImporter.GetAtPath(relfile);
            if (asset != null)
            {
                var index = relfile.IndexOf("bundle_");
                var bundleName = "";
                if (index != -1)
                {
                    var substr = relfile.Substring(index + 7);
                    bundleName = substr.Substring(0, substr.IndexOf('/'));
                    if (relfile.EndsWith(".unity", System.StringComparison.InvariantCultureIgnoreCase))
                        bundleName += "_s";
                    bundleName += ".bundle";
                }
                if (bundleName != asset.assetBundleName)
                {
                    Debug.Log(relfile + " : re-assign bundle name : " + asset.assetBundleName + " -> " + bundleName);
                    asset.SetAssetBundleNameAndVariant(bundleName, asset.assetBundleVariant);
                }
            }
        }
    }

}