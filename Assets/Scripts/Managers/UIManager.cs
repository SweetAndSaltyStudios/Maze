using UnityEngine;
using TMPro;

namespace Sweet_And_Salty_Studios
{
    public class UIManager : Singelton<UIManager>
    {
        

        public TextMeshProUGUI HintMessage
        {
            get;
            private set;
        }

        public ViewMask ViewMask
        {
            get;
            private set;
        }

        public CameraEngine MainCameraEngine
        {
            get;
            private set;
        }

        private void Awake()
        {
            HintMessage = GetComponentInChildren<TextMeshProUGUI>();
            MainCameraEngine = GetComponentInChildren<CameraEngine>();
        }

        private void Start()
        {
            ViewMask = Instantiate(ResourceManager.Instance.ViewMaskPrefab, MainCameraEngine.transform);

            HintMessage.enabled = false;        
        }
    }
}
