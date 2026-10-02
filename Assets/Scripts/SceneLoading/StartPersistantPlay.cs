using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class StartPersistantPlay : MonoBehaviour
{
   // [SerializeField] private GameObject _loadingbarGo;
 //   [SerializeField] private Image _loadingbar;

    [SerializeField] private SceneField _persistantPlay;
    [SerializeField] private SceneField _level;

    private List<AsyncOperation> scenesToLoad = new();

  //  private void Awake()
    //{
    //    _loadingbarGo.SetActive(false);
    //}
    public void StartGameButton()
    {
        scenesToLoad.Add(SceneManager.LoadSceneAsync(_persistantPlay));

        scenesToLoad.Add(SceneManager.LoadSceneAsync(_level, LoadSceneMode.Additive));
        // StartCoroutine(ProgressLoad());
    }

   // private IEnumerator ProgressLoad()
   // {
   //     float progress = 0f;

      //  for (int i = 0; i < scenesToLoad.Count; i++)
//{
       //     while (!scenesToLoad[i].isDone)
           // {
              //  progress = 0f;
              //  _loadingbar.fillAmount = progress / scenesToLoad.Count;
              //  yield return null;
            //}
       // }

   // }
}
