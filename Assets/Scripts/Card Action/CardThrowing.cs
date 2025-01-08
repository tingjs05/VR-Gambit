using UnityEngine;

namespace Card
{
    public class CardThrowing : MonoBehaviour
    {
        public float throwScale = 500f;

        // Method to throw a card
        public void ThrowCard(Vector3 outVector, Vector3 handPosition, Quaternion handRotation, float throwSpeed, bool charged, bool hand)
        {
            CardObject card = ActionManager.Instance.InstantiateCard(handPosition, handRotation);
            card.LaunchCard((Vector3.down + outVector).normalized, throwScale * throwSpeed, !charged, hand);
            AudioManager.instance.PlaySFX(charged ? AudioManager.instance.cardSFX.cardThrow_Charged : AudioManager.instance.cardSFX.cardThrow_Normal);
            card.chargedGlow.SetActive(charged);
            if (!charged) return;
            card.canSteer = true;
        }
    }
}
