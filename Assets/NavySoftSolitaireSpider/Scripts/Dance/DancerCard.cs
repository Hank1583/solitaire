namespace Dance
{
	using UnityEngine;
	public class DancerCard
	{
		private bool isFlyingToStart;
		private bool isFlyingInfinity;
		public bool IsFlyingToStart {get{ return isFlyingToStart;}}
		public bool IsFlyingInfinity {get{ return isFlyingInfinity;}}
		private Vector2 fromStartPosition;
		private Vector2 toStartPosition;
		private float totalTimeStartFlying;
		private float currentTimeStartFlying;
		public bool IsSleep {get{ return (!isFlyingToStart && !isFlyingInfinity);}}
		public DancerCard ()
		{
			isFlyingToStart = false;
			isFlyingInfinity = false;
		}
		public void SetFlyToStart(Vector2 from, Vector2 to, float time)
		{
			fromStartPosition = from;
			toStartPosition = to;
			totalTimeStartFlying = time;
			currentTimeStartFlying = 0f;
			isFlyingToStart = true;
		}
		public Vector2 NextFrameToStart(float time)
		{
			if (isFlyingToStart)
			{
				currentTimeStartFlying += time;
				if (currentTimeStartFlying > totalTimeStartFlying)
				{
					currentTimeStartFlying = totalTimeStartFlying;
					isFlyingToStart = false;
					//isFlyingInfinity = true;
				}
			}
			else
            {
#if UNITY_EDITOR
                throw new UnityException ("Flying mode is not activated.");
#endif
            }
			float normal = currentTimeStartFlying / totalTimeStartFlying;
			return Vector2.Lerp (fromStartPosition, toStartPosition, normal);
		}
	}	
}