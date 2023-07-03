using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class Enemy: MonoBehaviour, IDamageable
    {


        public void TakeDamage()
        {
            Debug.Log("Hit");
        }
    }
}
