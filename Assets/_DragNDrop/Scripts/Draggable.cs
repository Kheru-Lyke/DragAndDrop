using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.KherusEmporium.DragNDrop {

    /// <summary>
    /// Class for the objects dragged and contained
    /// </summary>
    public class Draggable : MonoBehaviour
    {
        [SerializeField] private bool canDropAnywhere = false;

        public bool CanDropAnywher => canDropAnywhere;

        public Container previousContainer = null;
        private int defaultLayer;
        private Vector3 defaultPos;

		private void Start() {
			defaultLayer = gameObject.layer;
            defaultPos = transform.position;
		}

		public void ReturnToPreviousContainer() {
            if (previousContainer != null) {
                transform.SetParent(previousContainer.transform);
                transform.localPosition = Vector3.zero;
            }
            else {
                transform.localPosition = defaultPos;
            }
        }

        public void Grab () {
            gameObject.layer = 2;
        }

        public void Drop() {
            gameObject.layer = defaultLayer;
            
            if (!canDropAnywhere) ReturnToPreviousContainer();
        }
    }
}
