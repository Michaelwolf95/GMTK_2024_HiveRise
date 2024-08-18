using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HiveRise
{
	//-///////////////////////////////////////////////////////////
	/// 
	public class CardUIView : MonoBehaviour
	{
		[SerializeField] private Button _cardButton = null;
		public Button cardButton => _cardButton;
		[SerializeField] private CanvasGroup _canvasGroup = null;
		public CanvasGroup canvasGroup => _canvasGroup;
		[SerializeField] private Image cardImage = null;
		[SerializeField] private Color[] cardTintColors = null;
		[SerializeField] private TextMeshProUGUI cardTextLabel = null;
		
		public CardData pieceCardData { get; private set; }
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void SetData(CardData argCardData)
		{
			pieceCardData = argCardData;
			
			cardImage.sprite = CardDefinitions.instance.GetPieceDataForID(argCardData.pieceShapeID).pieceShapeSprite;
			cardImage.color = cardTintColors[(int) argCardData.color];
			cardTextLabel.text = argCardData.GetCardDescriptionForData();
		}
	}
}