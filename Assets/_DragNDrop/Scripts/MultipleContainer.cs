using Com.KherusEmporium.DragNDrop;
using System.Collections.Generic;
using UnityEngine;

namespace Com.KherusEmporium
{
    public class MultipleContainer : Container
    {
		[SerializeField] private List<Draggable> contained = new List<Draggable>();
		[SerializeField] private SortingMethod sortingMethod = SortingMethod.FIFO;

		[SerializeField] private int maxContained = -1;

		[Space]
		[SerializeField] private Vector3 containerDimensions = Vector3.one;
		[SerializeField] private SortingPattern visualSortingPattern = SortingPattern.LineX;

		public override Draggable Add(Draggable draggable) {
			if (contained.Contains(draggable)) return null;
			if (!isOpen) return draggable;
			if (maxContained>0 && contained.Count >= maxContained) return draggable;

			draggable.transform.SetParent(transform);
			draggable.transform.localPosition = Vector3.zero;

			contained.Add(draggable);
			SortVisuals();

			return null;
		}

		public override void Clear() {
			foreach (Draggable draggable in contained) {
				Destroy(draggable);
			}

			contained.Clear();
		}

		public override bool IsEmpty() {
			return contained.Count == 0;
		}

		public override Draggable Remove() {
			if (contained.Count <= 0) return null;
			Draggable toReturn = null;

			switch (sortingMethod) {
				case SortingMethod.FIFO:
					toReturn = contained[0];
					break;
				case SortingMethod.FILO:
					toReturn = contained[contained.Count - 1];
					break;
				case SortingMethod.RANDOM:
					toReturn = contained[Random.Range(0, contained.Count-1)];
					break;
			}

			contained.Remove(toReturn);
			toReturn.previousContainer = this;
			SortVisuals();

			return toReturn;
		}

		private void SortVisuals() {
			if (visualSortingPattern == SortingPattern.None) return;
			Draggable current = null;

			float max = contained.Count - 1;
			if (max <= 0) max = 1;

			float width = containerDimensions.x / 2;
			float height = containerDimensions.y / 2;
			float depth = containerDimensions.z / 2;

			for (int i = 0; i < contained.Count; i++) {
				current= contained[i];

				float percent = (float)i / max;


				switch (visualSortingPattern) {

					case SortingPattern.LineX:
						current.transform.localPosition= new Vector3(Mathf.Lerp(-width, width, percent), 0, 0);
						break;
					case SortingPattern.LineY:
						current.transform.localPosition = new Vector3(0, Mathf.Lerp(-height, height, percent), 0);
						break;
					case SortingPattern.LineZ:
						current.transform.localPosition = new Vector3(0, 0, Mathf.Lerp(-depth, depth, percent));
						break;
				}
			}

			
		}
    }

	[System.Serializable]
	public enum SortingMethod {
		FIFO,
		FILO,
		RANDOM
	}

	[System.Serializable]
	public enum SortingPattern {
		None,
		LineX,
		LineY,
		LineZ,
	}
}
