using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class Taps : MonoBehaviour {
    public static Touch touch;
    public GameObject tappedListItem;

    //private float tapTime = 0f;
    //private float shortTapMaxTime = 0.3f;
    //private float longTapMinTime = 0.4f;
    //private bool hitListItem;
    //private GraphicRaycaster raycaster;

    //void GetTappedItem() {
    //    PointerEventData pointer = new PointerEventData(EventSystem.current);
    //    pointer.position = touch.position;
    //    List<RaycastResult> raycastResults = new List<RaycastResult>();
    //    EventSystem.current.RaycastAll(pointer, raycastResults);

    //    // Get the list item that was hit
    //    //if (raycastResults.Count > 0) {
    //    //    if (raycastResults[0].gameObject.tag.Contains(TaskList.listItemTag) &&
    //    //        !raycastResults[0].gameObject.tag.Contains(TaskList.subgoalTag)) {
    //    //        tappedListItem = raycastResults[0].gameObject;
    //    //        hitListItem = true;
    //    //    }
    //    //}
    //}

    //    public TapType CheckForTaps() {
    //        TapType tapType = TapType.noTap;
    //        if (Input.touchCount > 0) {
    //            tapTime += Time.deltaTime;
    //            touch = Input.touches[0];
    //            if (touch.phase == TouchPhase.Began) {
    //                GetTappedItem();
    //            }
    //            if (hitListItem && touch.phase == TouchPhase.Ended && tapTime < shortTapMaxTime) {
    //                hitListItem = false;
    //                tapType = TapType.shortTap;
    //                Debug.Log("Short tap: " + tapType);
    //            } else if (hitListItem && touch.phase == TouchPhase.Ended && tapTime > longTapMinTime) {
    //                hitListItem = false;
    //                tapType = TapType.longTap;
    //                Debug.Log("Long tap: " + tapType);
    //            }
    //        } else {
    //            tapTime = 0f;
    //        }
    //        if (tapType == TapType.noTap) {
    //            Debug.Log("Tap type: " + tapType);
    //        }
    //        return tapType;
    //    }

    //    IEnumerator SingleOrDoubleTap() {
    //        float tapCooldown = 0.5f; // Half a second before reset
    //        int tapCount = 0;
    //        // In update
    //        if (Input.anyKeyDown()) {
    //            if (tapCooldown > 0 && tapCount == 1/*Number of Taps you want Minus One*/) {
    //                //Has double tapped
    //            } else {
    //                // Single tap
    //                tapCooldown = 0.5f;
    //                tapCount += 1;
    //            }
    //        }

    //        if (tapCooldown > 0) {
    //            // Countdown for double tap
    //            tapCooldown -= 1 * Time.deltaTime;
    //        } else {
    //            tapCount = 0;
    //        }

    //        //yield return new WaitForSeconds(0.3f);
    //        //if (Taps.touch.tapCount == 1) {
    //        //    print("single tap");
    //        //} else if (Taps.touch.tapCount == 2) {
    //        //    StopCoroutine("SingleOrDoubleTap");
    //        //    print("double tap");
    //        //}
    //    }
}

public enum TapType {
    shortTap, longTap, doubleTap, noTap
}

//public abstract class TapType {
//    public static string shortTap = "ShortTap";
//    public static string longTap = "LongTap";
//    public static string doubleTap = "DoubleTap";
//    public static string noTap = "NoTap";

//    public abstract string GetTapType();
//}

//public class ShortTap : TapType {
//    public ShortTap() { }

//    override public string GetTapType() {
//        return TapType.shortTap;
//    }
//}

//public class LongTap : TapType {
//    public LongTap() { }

//    override public string GetTapType() {
//        return TapType.longTap;
//    }
//}

//public class DoubleTap : TapType {
//    public DoubleTap() { }

//    override public string GetTapType() {
//        return TapType.doub;
//    }
//}

//public class NoTap : TapType {
//    public NoTap() { }

//    override public string GetTapType() {
//        return TapType.noTap;
//    }
//}