using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Terrarium.Components
{
    public class EndingController : MonoBehaviour
    {
        static EndingController instance;

        public static EndingController Instance => instance;

        public UnityEvent OnEndingStart = new();

        GameObject creditsVolume;

        protected void Awake()
        {
            instance = this;

            creditsVolume = transform.Find("Sector/LoadCreditsVolume").gameObject;

            GlobalMessenger.AddListener("ExitConversation", OnExitConversation);
        }

        protected void Start()
        {
            gameObject.SetActive(false);
            creditsVolume.SetActive(false);
        }

        protected void OnDestroy()
        {
            GlobalMessenger.RemoveListener("ExitConversation", OnExitConversation);
        }

        void OnExitConversation()
        {
            if (DialogueConditionManager.SharedInstance.GetConditionState("WW_TERRARIUM_START_ENDING"))
            {
                gameObject.SetActive(true);
                OnEndingStart.Invoke();
                StartCoroutine(DoEnding());
            }
        }
        IEnumerator DoEnding()
        {
            Locator.GetShipLogManager().RevealFact("WW_TERRARIUM_GOOD_ENDING", true, false);
            yield return new WaitForSeconds(10f);
            creditsVolume.SetActive(true);
        }
    }
}
