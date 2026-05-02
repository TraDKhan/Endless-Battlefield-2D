using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._01Version.Scripts.UI.Reward
{
    public class UISlotReward : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private CanvasGroup canvasGroup; // Dùng để làm hiệu ứng Fade
        [SerializeField] private Image backgroundIconImage;
        [SerializeField] private GameObject specialEffect; // Hiệu ứng đặc biệt

        private int targetAmount;
        private int currentDisplayedAmount = 0;

        public void Setup(Sprite icon, Sprite backgroundIcon, int amount, bool isSpecial)
        {
            iconImage.sprite = icon;
            backgroundIconImage.sprite = backgroundIcon;
            specialEffect.SetActive(isSpecial);
            //amountText.text = "+" + amount.ToString();

            targetAmount = amount;
            currentDisplayedAmount = 0;
            amountText.text = "+0";

            // Reset trạng thái trước khi diễn hiệu ứng
            transform.localScale = Vector3.zero;
            if (canvasGroup != null) canvasGroup.alpha = 0;

            // Hiệu ứng xuất hiện với DOTween
            PlayAppearAnimation();
        }

        private void PlayAppearAnimation()
        {
            // 1. Hiệu ứng phóng to ô chứa
            transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack).SetUpdate(true);

            if (canvasGroup != null)
                canvasGroup.DOFade(1f, 0.4f).SetUpdate(true);

            // 2. Hiệu ứng chạy số (Bắt đầu sau khi ô chứa hiện ra một chút)
            DOVirtual.DelayedCall(0.2f, StartCounter).SetUpdate(true);
        }

        private void StartCounter()
        {
            // DOTween.To sẽ thay đổi giá trị currentDisplayedAmount từ 0 đến targetAmount
            DOTween.To(() => currentDisplayedAmount, x => currentDisplayedAmount = x, targetAmount, 1f)
                .OnUpdate(() =>
                {
                    // Cập nhật Text mỗi Frame khi giá trị thay đổi
                    amountText.text = "+" + currentDisplayedAmount.ToString();
                })
                .SetEase(Ease.OutQuad) // Chậm dần về cuối để người chơi kịp nhìn con số
                .SetUpdate(true);      // Đảm bảo chạy được cả khi Pause game (Time.timeScale = 0)

            // Hiệu ứng "nảy" nhẹ con số mỗi khi chạy số (Tùy chọn)
            amountText.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 5, 0.5f).SetUpdate(true);
        }

        // Hàm thu thập phần thưởng bay về một vị trí (ví dụ góc màn hình)
        public void PlayCollectAnimation(Vector3 targetScreenPos)
        {
            transform.DOMove(targetScreenPos, 0.7f).SetEase(Ease.InExpo).SetUpdate(true);
            transform.DOScale(0.5f, 0.7f).SetUpdate(true).OnComplete(() => {
                Destroy(gameObject);
            });
        }
    }
}