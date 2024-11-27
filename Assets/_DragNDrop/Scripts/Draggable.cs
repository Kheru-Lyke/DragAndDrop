using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.KherusEmporium.DragNDrop {

	/// <summary>
	/// Class for the objects dragged and contained
	/// </summary>
	public class Draggable : MonoBehaviour {
		[SerializeField] private bool canDropAnywhere = false;

		public bool CanDropAnywher => canDropAnywhere;

		public Container previousContainer = null;
		private int defaultLayer;
		private Transform defaultParent;
		private Vector3 defaultPos;

		private void Start() {
			defaultLayer = gameObject.layer;
			defaultPos = transform.localPosition;
			defaultParent= transform.parent;
		}

		public void ReturnToPreviousContainer() {
			if (previousContainer != null) {
				previousContainer.Add(this);
			}
			else ReturnToStartPos();

		}

		public void ReturnToStartPos() {
			transform.SetParent(defaultParent);
			transform.localPosition = defaultPos;
		}

		public void Grab() {
			gameObject.layer = 2;
		}

		public void Drop(bool hasContainer) {
			gameObject.layer = defaultLayer;

			if (!canDropAnywhere && !hasContainer) ReturnToPreviousContainer();
		}
	}
}
