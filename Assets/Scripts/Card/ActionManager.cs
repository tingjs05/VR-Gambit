using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Card
{
    public class ActionManager : MonoBehaviour
    {
        public CardObject cardPrefab;
        public static ActionManager Instance { get; private set; }

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                gameObject.SetActive(false);
        }
    }
}
