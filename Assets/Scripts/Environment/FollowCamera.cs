using UnityEngine;

namespace Environment
{
    public class FollowCamera : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            transform.position = new Vector3(Camera.main.transform.position.x, 0f, Camera.main.transform.position.z);
        }
    }
}
