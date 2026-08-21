using System;
using System.Collections.Generic;
using UnityEngine;

public enum ESceneID
{
    Title,
    MainGame,
    Result
}

[Serializable]
public class SceneInfo
{
    public ESceneID ID;
    public string sceneName;
}

public class SceneCatalog : MonoBehaviour
{
    #region 인스펙터
    [Header("씬 리스트")]
    [SerializeField] private List<SceneInfo> _scenes = new List<SceneInfo>();
    #endregion

    #region 내부 변수
    private Dictionary<string, ESceneID> _nameToId = new Dictionary<string, ESceneID>();
    private Dictionary<ESceneID, string> _idToName = new Dictionary<ESceneID, string>();
    #endregion

    public IReadOnlyList<SceneInfo> Scenes => _scenes;


    public void SettingDictionary()
    {
        if (_scenes == null || _scenes.Count == 0)
        {
            Debug.LogWarning("씬 null 혹은 0개 / 인스펙터 확인");

            return;
        }

        _idToName.Clear();
        _nameToId.Clear();

        for (int i = 0; i < _scenes.Count; i++)
        {
            SceneInfo sceneInfo = _scenes[i];

            if (sceneInfo == null)
            {
                continue;
            }

            if (_idToName.ContainsKey(sceneInfo.ID))
            {
                Debug.LogWarning($"Id {sceneInfo.ID} 중복으로 등록 실패 / {_idToName[sceneInfo.ID]} 등록됨");
                continue;
            }

            if (_nameToId.ContainsKey(sceneInfo.sceneName))
            {
                continue;
            }


            _idToName.Add(sceneInfo.ID, sceneInfo.sceneName);
            _nameToId.Add(sceneInfo.sceneName, sceneInfo.ID);
        }

        Debug.Log($"씬 리스트 카운트 {_scenes.Count}");
        Debug.Log($"맵 카운트 (ID → Name) {_idToName.Count}");
        Debug.Log($"맵 카운트 (Name → ID) {_nameToId.Count}");
    }

    public bool TryGetSceneName(ESceneID id, out string name)
    {
        return _idToName.TryGetValue(id, out name);
    }

    public bool TryGetSceneId(string name, out ESceneID id)
    {
        return _nameToId.TryGetValue(name, out id);
    }

    public IReadOnlyList<SceneInfo> GetScenes()
    {
        return Scenes;
    }
}
