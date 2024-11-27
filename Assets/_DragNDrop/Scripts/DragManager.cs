using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.KherusEmporium.DragNDrop {
	/// <summary>
	/// Handler for all drag and drop mouse interactions, singleton.
	/// </summary>
	public class DragManager : MonoBehaviour {
		[SerializeField] private MouseButton dragOn = MouseButton.Left;
		[SerializeField] private Camera cam = null;

		[Space]
		[SerializeField] private float distanceFromCam = 5;

		private Draggable drag = null;

		private void Start() {
			SetupSingleton();
		}

		private void Update() {
			if (Input.GetMouseButtonDown(((int)dragOn))) StartDrag();
			if (Input.GetMouseButtonUp(((int)dragOn)) && drag!= null) StopDrag();

			if (drag != null) Drag();
		}

		private Vector3 MousePosition {
			get {
				Vector3 startPos = Input.mousePosition;
				startPos.z = distanceFromCam;

				return startPos;
			}
		}

		private void Drag() {
			if (Physics.Raycast(cam.ScreenPointToRay(MousePosition), out RaycastHit hit)) {
				Debug.DrawLine(cam.transform.position, hit.point, Color.green);

				drag.transform.position = hit.point;
			}
			else {

				drag.transform.position = cam.ScreenToWorldPoint(MousePosition);

			}
		}

		private void StartDrag() {
			if (Physics.Raycast(cam.ScreenPointToRay(MousePosition), out RaycastHit hit)) {
				Debug.DrawLine(cam.transform.position, hit.point, Color.red, 5);

				drag = hit.collider.gameObject.GetComponent<Draggable>();
				if (CheckIfDragging()) return;

				Container container = hit.collider.GetComponentInChildren<Container>();
				drag = container?.Remove();
				CheckIfDragging();
			}
		}

		private bool CheckIfDragging() {
			if (drag != null) {
				drag?.Grab();
				return true;
			}
			return false;
		}

		private void StopDrag() {

			if (Physics.Raycast(cam.ScreenPointToRay(MousePosition), out RaycastHit hit)) {
				Debug.DrawLine(cam.transform.position, hit.point, Color.blue, 5);

				Container container = hit.collider.gameObject.GetComponentInChildren<Container>();

				if (container != null) {
					container.Add(drag);
				}
			}

			drag.Drop();
			drag = null;
		}


		// Singleton code
		static private DragManager instance;
		static public DragManager Instance => instance;
		private void SetupSingleton() {
			instance = this;
		}
	}

	public enum MouseButton {
		Left,
		Right,
		Middle
	}
}
