using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.MonoBehaviours
{
    [RequireComponent(typeof(Button))]
    public class ReloadSceneButton : MonoBehaviour
    {
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(ReloadScene);
        }

        private void ReloadScene()
        {
            SceneManager.LoadScene(0);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}