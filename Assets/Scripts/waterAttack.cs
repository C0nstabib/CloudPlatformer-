using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterAttack : MonoBehaviour
{
    Transform waterAttackPoint;
    [SerializeField] GameObject projectilePrefab;
    public bool waterAttackStart;

    public IEnumerator waterAttackFunction()
    {
        waterAttackStart = true;
        Instantiate(projectilePrefab, waterAttackPoint.position, waterAttackPoint.rotation);
        yield return new WaitForSeconds(0.1f);
        waterAttackStart = false;
    }
}
