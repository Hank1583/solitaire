namespace Dance
{
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections.Generic;
	public class DancerData
	{
		private List<Vector2[]> track;
		private List<int> cardCount;
		private List<float> circleTime;
		public int Count {get{return track.Count;}}
		private float[] trackLength;
		private float[][] trackDeltaNode;
		private float[] currentTime;
		private bool[] isLoop;
		public bool[] IsLoop {get{ return isLoop;}}
		public DancerData ()
		{
			track = new List<Vector2[]> ();
			cardCount = new List<int> ();
			circleTime = new List<float> ();
		}
		public void Add (Vector2[] track, int cardCount, float circleTime)
		{
			this.track.Add (track);
			this.cardCount.Add (cardCount);
			this.circleTime.Add (circleTime);
		}
		public bool Init()
		{
			bool isInit = true;
			trackLength = new float[Count];
			trackDeltaNode = new float[Count][];

			currentTime = new float[Count];
			isLoop = new bool[Count];

			for (int index = 0; index < Count; index++)
			{
				int trackCardCount = cardCount [index];
				if (trackCardCount > 0)
				{
					trackDeltaNode [index] = TrackDeltaNode (index);
					trackLength [index] = TrackLength (index);
				}
				if (trackLength [index].Equals (0f) || trackCardCount.Equals(0) || circleTime[index].Equals(0f)) isInit = false;

				currentTime [index] = 0f;
				isLoop [index] = false;
			}
			return isInit;
		}
		private float TrackLength (int trackIndex)
		{
			float totalLength = 0f; 
			int countNodes = track [trackIndex].Length;
			for (int nodeIndex = 0; nodeIndex < countNodes; nodeIndex++)
				totalLength += trackDeltaNode [trackIndex] [nodeIndex];
			return totalLength;
		}
		private float[] TrackDeltaNode (int trackIndex)
		{
			int countNodes = track [trackIndex].Length;
			float[] deltaNode = new float[countNodes];

			for (int nodeIndex = 0; nodeIndex < (countNodes-1); nodeIndex++)
				deltaNode [nodeIndex] = Vector2.Distance(track [trackIndex][nodeIndex], track [trackIndex][nodeIndex+1]);
			deltaNode [countNodes-1] = Vector2.Distance (track [trackIndex] [countNodes - 1], track [trackIndex] [0]);

			return deltaNode;
		}

		public int CardCount(int treckIndex)
		{
			return cardCount [treckIndex];
		}
		public float CircleTime(int treckIndex)
		{
			return circleTime [treckIndex];
		}
		public List<Vector2[]> Track {get{ return track;}}
		public Vector2[] GetTrackPivots (int trackIndex, float normalize)
		{
			int cardTrackCount = cardCount [trackIndex];
			float deltaNormalize = 1f / cardTrackCount;

			Vector2[] pivots = new Vector2[cardTrackCount];
			for (int cardIndex = 0; cardIndex < cardTrackCount; cardIndex++)
			{
				pivots [cardIndex] = CalculatePosition (trackIndex, normalize);
				normalize += deltaNormalize;
				if (normalize > 1f) normalize -= 1f;
			}
			Vector2[] revertedPivots = RevertPivots (pivots);
			return revertedPivots;
		}

		private Vector2[] RevertPivots (Vector2[] pivots)
		{
			int pivotCount = pivots.Length;
			Vector2[] revertedPivots = new Vector2[pivotCount];
			for (int index = 0; index < pivotCount; index++)
				revertedPivots [index] = pivots [pivotCount - 1 - index];
			return revertedPivots;
		}

		private Vector2 CalculatePosition(int trackIndex, float normalize)
		{
			float destination = trackLength [trackIndex] * normalize;
			int countNodes = track [trackIndex].Length;
			float currentLength = 0f;
			for (int nodeIndex = 0; nodeIndex < countNodes; nodeIndex++)
			{
				float deltaNode = trackDeltaNode [trackIndex] [nodeIndex];
				if ((currentLength + deltaNode)>= destination)
				{
					float residual = destination - currentLength;
					float normalNodes = residual / deltaNode;
					Vector2 nodeLerp0 = track [trackIndex] [nodeIndex];
					Vector2 nodeLerp1 = (nodeIndex.Equals(countNodes-1)) ? track [trackIndex] [0] : track [trackIndex] [nodeIndex+1];
					return Vector2.Lerp (nodeLerp0, nodeLerp1, normalNodes);
				}
				else currentLength += deltaNode;
			}
			return Vector2.zero;
		}
		public void AddTimeToCircle(int trackIndex, float time)
		{
			currentTime [trackIndex] += time;
			if (currentTime [trackIndex] > circleTime [trackIndex])
			{
				int fullTimeCicle = (int) Mathf.Floor (currentTime [trackIndex] / circleTime [trackIndex]);
				currentTime [trackIndex] = currentTime [trackIndex] - (circleTime [trackIndex] * fullTimeCicle);
				isLoop [trackIndex] = true;
			}
		}
		public float NormalizeCircleTime(int trackIndex)
		{
			return currentTime [trackIndex] / circleTime [trackIndex];
		}
	}
}