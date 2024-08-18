using MichaelWolfGames;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HiveRise
{
	//-///////////////////////////////////////////////////////////
	/// 
	public class UIManager : SceneSingleton<UIManager>
	{
		[SerializeField] private RectTransform _handContainerRect = null;
		public RectTransform handContainerRect => _handContainerRect;
		
		[Header("Buttons")]
		[SerializeField] private Button submitButton = null;
		[SerializeField] private TextMeshProUGUI placementTrackerLabel = null;
		[SerializeField] private string placementTrackerFormatString = "{0}/{1} PLACED";
		
		//-///////////////////////////////////////////////////////////
		/// 
		protected override void Awake()
		{
			base.Awake();
			submitButton.onClick.AddListener(OnSubmitButtonPressed);
			submitButton.interactable = false;
		}

		//-///////////////////////////////////////////////////////////
		/// 
		private void OnSubmitButtonPressed()
		{
			if (GameBoardController.instance.CanAllPendingPiecesBeApplied())
			{
				GameManager.instance.PlayPendingPieces();
			}
		}

		//-///////////////////////////////////////////////////////////
		/// 
		public void OnPendingPiecePlaced()
		{
			UpdatePlacementTrackerLabel();
			submitButton.interactable = GameBoardController.instance.CanAllPendingPiecesBeApplied();
		}
		
		//-///////////////////////////////////////////////////////////
		/// 
		public void OnPendingPieceUpdated()
		{
			submitButton.interactable = GameBoardController.instance.CanAllPendingPiecesBeApplied();
		}

		//-///////////////////////////////////////////////////////////
		/// 
		public void OnPendingPieceReturnedToHand()
		{
			UpdatePlacementTrackerLabel();
			submitButton.interactable = GameBoardController.instance.CanAllPendingPiecesBeApplied();
		}

		//-///////////////////////////////////////////////////////////
		/// 
		private void UpdatePlacementTrackerLabel()
		{
			placementTrackerLabel.text = string.Format(placementTrackerFormatString, GameBoardController.instance.GetNumPendingPieces(), GameManager.MAX_CARDS_PER_PLAY);
		}
		
		
	}
}