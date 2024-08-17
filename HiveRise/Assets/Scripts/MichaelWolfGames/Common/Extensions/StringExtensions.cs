using UnityEngine;

namespace MichaelWolfGames
{
	///-///////////////////////////////////////////////////////////
	///
	public static class StringExtensions
	{
		///-///////////////////////////////////////////////////////////
		///
		public static string RichText(this string argString, Color argTextColor)
		{
			return "<color=#" + UnityEngine.ColorUtility.ToHtmlStringRGB(argTextColor) + "ff>" + argString + "</color>";
		}
	}
}