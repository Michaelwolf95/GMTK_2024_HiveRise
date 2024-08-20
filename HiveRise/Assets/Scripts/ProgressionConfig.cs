using System;
using UnityEngine;

namespace HiveRise
{
	//-///////////////////////////////////////////////////////////
	/// 
	[Serializable]
	public class TierData
	{
		public float heightRequirement = 15f;
	}
	
	//-///////////////////////////////////////////////////////////
	/// 
	[CreateAssetMenu(fileName="ProgressionConfig", menuName="HiveRise/ProgressionConfig")]
	public class ProgressionConfig : ScriptableObject
	{
		public TierData[] progressionTiers = null;

		public bool allowInfinite = true;
		public int infiniteScaleStartAmount = 30;
		public int infiniteScaleIncrementAmount = 5;
		public int infiniteScaleIncrementInterval = 3;
		

		//-///////////////////////////////////////////////////////////
		/// 
		public float GetHeightRequirementForTierIndex(int argTierIndex)
		{
			float height = 0f;
			for (int i = 0; i < argTierIndex + 1; i++)
			{
				if (i < progressionTiers.Length)
				{
					height += progressionTiers[i].heightRequirement;
				}
				else
				{
					int numInfiniteScaleTiers = (i - progressionTiers.Length);
					int numIntervals = (numInfiniteScaleTiers / infiniteScaleIncrementInterval);
					int scaleAmount = infiniteScaleStartAmount + (numIntervals * infiniteScaleIncrementAmount);
					height += scaleAmount;
				}
			}
			return height;
		}
	}
}