using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class WeaponInstance
    {
        public WeaponSO WeaponSO { get; private set; }

        public int MagazineAmount { get; private set; }

        public int AmmoAmount { get; private set; }

        public WeaponInstance(WeaponSO weaponSO,int magazineAmount, int ammoAmount)
        {
            WeaponSO = weaponSO;
            MagazineAmount = magazineAmount;
            AmmoAmount = ammoAmount;
        }

        public void SubstractAmmo()
        {
            if (AmmoAmount > 0)
                AmmoAmount--;

            Inventory.Instance.AmmoChanged();
        }

        public void FillAmmo() => AmmoAmount = WeaponSO.AmmoInMagazine;

    }
}
