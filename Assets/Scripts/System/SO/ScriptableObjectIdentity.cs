using Sirenix.OdinInspector;
using UnityEngine;

namespace System.SO
{
    public class ScriptableObjectIdentity : ScriptableObject
    {
        [ShowInInspector] public string ID => _id;
        
        private string _id = Guid.NewGuid().ToString();
    }
}