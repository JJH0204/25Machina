using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.VisualScripting
{
    public class SetObjectText : ProcessBase
    {
        [SerializeField] private Transform target;
        [SerializeField, TextArea(3, 5)] private string objectDescription;

        public override void Execute()
        {
            Managers.GUIManager.Instance.ObjectText.text = objectDescription;

            if (target)
            {
                Managers.GUIManager.Instance.SetIndicatorTarget(target);
            }
        }
    }
}