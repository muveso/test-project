using System.Collections;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace Util
{
    public static class UnityExtensionMethods
    {
        public static IEnumerator StartAnimation(this Transform target)
        {
            const float duration = .3f;
            var elapsedTime = 0f;
            var scale = target.localScale;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                target.localScale = Vector3.Lerp(scale, scale * 5, Easing.InBounce(elapsedTime / duration));
                yield return new WaitForEndOfFrame();
            }
            target.gameObject.SetActive(false);
            target.localScale = scale;
        }
        
        public static IEnumerator CreateBonusEffect(this Animator anim)
        {
            anim.gameObject.SetActive(true);
            anim.Play("BonusText");
            yield return new WaitForSeconds(1.5f);
            anim.gameObject.SetActive(false);
        }
    }
}