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
            transform.Find("Sector").gameObject.SetActive(false);
            transform.Find("GravityWell").gameObject.SetActive(false);
        }

        protected void OnDestroy()
        {
            GlobalMessenger.RemoveListener("ExitConversation", OnExitConversation);
        }

        void OnExitConversation()
        {
            if (DialogueConditionManager.SharedInstance.GetConditionState("WW_TERRARIUM_START_ENDING"))
            {
                transform.Find("Sector").gameObject.SetActive(true);
                transform.Find("GravityWell").gameObject.SetActive(true);
                OnEndingStart.Invoke();
                StartCoroutine(DoEnding());
            }
        }
        IEnumerator DoEnding()
        {
            Locator.GetShipLogManager().RevealFact("WW_TERRARIUM_GOOD_ENDING", true, false);
            yield return new WaitForSeconds(20f);
            creditsVolume.transform.position = Locator.GetPlayerTransform().position;
        }
    }
}
