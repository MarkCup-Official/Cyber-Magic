using System.Collections;
using System.Collections.Generic;
using TripoForUnity;
using UnityEngine;

public class Magic : MonoBehaviour
{
    public GameObject SimpleModel;
    public TripoRuntimeCore core;

    public DButton b;
    public GameObject dice;
    public float ModelRotationSpeed = 50f;

    public void Generate(string str)
    {
        core.textPrompt = str;
        core.OnDownloadComplete.AddListener(OnFbxDownloadComplete);
        core.Text_to_Model_func();
    }
    void Update()
    {
        SimpleModel.transform.Rotate(0, ModelRotationSpeed * Time.deltaTime, 0);
    }/*
    void OnFbxDownloadComplete(string GltfUrl)
    {
        b.enabled = false;
        dice.SetActive(false);
        Vibration.VibratePattern(new long[] { 100, 100 }, -1);

        if (SimpleModel.GetComponent<GLTFast.GltfAsset>())
        {
            Destroy(SimpleModel.GetComponent<GLTFast.GltfAsset>());
        }
        var gltfModel = SimpleModel.AddComponent<GLTFast.GltfAsset>();
        gltfModel.Url = GltfUrl;
        gltfModel.GetMaterial();
    }*/

    void OnFbxDownloadComplete(string GltfUrl)
    {
        b.enabled = false;
        dice.SetActive(false);
        Vibration.VibratePattern(new long[] { 100, 100 }, -1);

        // 移除旧的 GLTF 组件
        if (SimpleModel.GetComponent<GLTFast.GltfAsset>())
        {
            Destroy(SimpleModel.GetComponent<GLTFast.GltfAsset>());
        }

        // 添加 GLTF 组件并加载新模型
        var gltfModel = SimpleModel.AddComponent<GLTFast.GltfAsset>();
        gltfModel.Url = GltfUrl;

        // 等待 GLTF 加载完成后再应用材质
        StartCoroutine(WaitForModelLoad(gltfModel));
    }

    IEnumerator WaitForModelLoad(GLTFast.GltfAsset gltfModel)
    {
        // 等待 GLTF 加载完成
        while (!gltfModel.IsDone)
        {
            yield return null;
        }

        // 遍历所有 Mesh 并手动设置材质
        Renderer[] renderers = SimpleModel.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            if (renderer.material == null || renderer.material.name == "Default-Material")
            {
                renderer.material = gltfModel.GetMaterial(0); // 取第一个材质
            }
        }
        Vibration.VibratePattern(new long[] { 100, 100 }, -1);
    }
}
