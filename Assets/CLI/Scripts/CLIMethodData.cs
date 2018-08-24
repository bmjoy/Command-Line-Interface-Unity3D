using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace CLI
{
    public class CLIMethodData
    {

        public MonoBehaviour monoBehaviour;
        public List<MethodInfo> methods;

        public CLIMethodData()
        {
            monoBehaviour = null;
            methods = new List<MethodInfo>();
        }

        public CLIMethodData(MonoBehaviour monoBehaviour, List<MethodInfo> methods)
        {
            this.monoBehaviour = monoBehaviour;
            this.methods = methods;
        }

    }
}
