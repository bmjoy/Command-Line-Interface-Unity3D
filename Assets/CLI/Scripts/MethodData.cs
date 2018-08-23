using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace CLI
{
    public class MethodData
    {

        public MonoBehaviour monoBehaviour;
        public List<MethodInfo> methods;

        public MethodData()
        {
            monoBehaviour = null;
            methods = new List<MethodInfo>();
        }

        public MethodData(MonoBehaviour monoBehaviour, List<MethodInfo> methods)
        {
            this.monoBehaviour = monoBehaviour;
            this.methods = methods;
        }

    }
}
