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


		//-///////////////////////////////////////////////////////////
		/// 
		public float GetHeightRequirementForTierIndex(int argTierIndex)
		{
			float height = 0f;
			for (int i = 0; i < argTierIndex; i++)
			{
				height += progressionTiers[i].heightRequirement;
			}
			return height;
		}
	}
}