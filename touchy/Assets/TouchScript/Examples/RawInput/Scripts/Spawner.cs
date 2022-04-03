/*
 * @author Valentin Simonov / http://va.lent.in/
 */

using UnityEngine;
using System.Collections.Generic;

namespace TouchScript.Examples.RawInput
{
    /// <exclude />
    public class Spawner : MonoBehaviour
    {
        public GameObject Prefab;
        Dictionary<Pointers.Pointer, GameObject> BallArray = new Dictionary<Pointers.Pointer, GameObject>();

        private void OnEnable()
        {
            if (TouchManager.Instance != null)
            {
                TouchManager.Instance.PointersPressed += pointersPressedHandler;
                TouchManager.Instance.PointersUpdated += pointersUpdateHandler;
                TouchManager.Instance.PointersReleased += pointersReleaseHandler;
            }

        }

        private void OnDisable()
        {
            if (TouchManager.Instance != null)
            {
                TouchManager.Instance.PointersPressed -= pointersPressedHandler;
                TouchManager.Instance.PointersUpdated += pointersUpdateHandler;
                TouchManager.Instance.PointersReleased -= pointersReleaseHandler;
            }
        }

        private GameObject spawnPrefabAt(Vector2 position)
        {
            var obj = Instantiate(Prefab) as GameObject;
            obj.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, 10));
            obj.transform.rotation = transform.rotation;
            return obj;
        }

        private void UpdatePrefabPos(GameObject target,Vector2 position)
        {
            Vector3 newPos = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, 10));
            target.transform.position = newPos;
        }

        private void pointersPressedHandler(object sender, PointerEventArgs e)
        {
            foreach (var pointer in e.Pointers)
            {
                BallArray.Add(pointer, spawnPrefabAt(pointer.Position));
            }
        }

        private void pointersUpdateHandler(object sender, PointerEventArgs e)
        {
            foreach (var pointer in e.Pointers)
            {
                GameObject target = BallArray[pointer];
                UpdatePrefabPos(target, pointer.Position);
            }
        }

        private void pointersReleaseHandler(object sender, PointerEventArgs e)
        {
            foreach (var pointer in e.Pointers)
            {
                BallArray[pointer].GetComponent<Ball>().selfDetroy();
                BallArray.Remove(pointer);
            }
        }
    }
}