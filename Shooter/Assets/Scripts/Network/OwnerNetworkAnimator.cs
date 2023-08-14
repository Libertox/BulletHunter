using System;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;


namespace Unity.Multiplayer.Samples.Utilities.ClientAuthority
{
    public class OwnerNetworkAnimator : NetworkAnimator
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}
