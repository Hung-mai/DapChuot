using UnityEngine;

public class DapChuot_Rotation : MonoBehaviour
{
    private void Update()
    {
        transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, transform.eulerAngles + Vector3.up * 90, 90 * Time.deltaTime);
    }
}
