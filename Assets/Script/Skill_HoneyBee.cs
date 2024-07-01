using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_HoneyBee : MonoBehaviour
{
    public void OnClick()
    {
        // HoneyBee 주변 10m 반경 내에 있는 차량들을 찾습니다.
        Collider[] carColliders = Physics.OverlapSphere(transform.position, 10f);

        foreach (Collider hitCollider in carColliders)
        {
            // 차량 태그를 가진 오브젝트를 찾습니다.
            Car car = hitCollider.GetComponent<Car>();
            if (car != null)
            {
                car.curHP += 300;
            }
        }
    }
}
