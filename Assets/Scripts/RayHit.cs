using UnityEngine;

/**
 * In unity, we can set our default layers to object in the inspector's layer tab.
 * Unity uses these layers with bit masks. If the bits are : 00000....000100 then that means use the layer numbered 3. 
 * Positions of 1s will be used as layer numbers.
 * We can use these layers with Raycast. In the scene named Unity LayerMask demonstrates this situation. For instance, in this scene, cube uses layer cube(6),
 * capsule uses layer capsule(7) and sphere uses layer sphere(8). If we use "int layerMask = ~(1 << 7);" that means cast all layers except the 7th one. Raycast will stop casting when hit something first.
 * So check example with moving objects up or down in y axis. You will see that capsule is invisible for raycast. It passes through it and hits the sphere.
 */
public class RayHit : MonoBehaviour {
	// Start is called before the first frame update
	void Update() {
		RaycastHit hit;
		/**
		 * int layerMask = ~0; // means cast all layers
		 * int layerMask = ~(1 << 7); // means cast all layers except the layer 7
		 */
		int layerMask = ~(1 << 7);

		if(Physics.Raycast(transform.position , transform.forward , out hit , Mathf.Infinity , layerMask)) {
			Debug.DrawRay(transform.position , transform.TransformDirection(transform.forward) * hit.distance , Color.red);
			Debug.Log("Hit");
		} else {
			Debug.DrawRay(transform.position , transform.TransformDirection(transform.forward) * 1000f , Color.yellow);
			Debug.Log("Missed");
		}
	}


}
