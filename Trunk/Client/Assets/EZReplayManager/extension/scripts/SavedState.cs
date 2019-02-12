using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using System;

/*
 * SoftRare - www.softrare.eu
 * This class represents a state of a single object in one single frame. 
 * You may only use and change this code if you purchased it in a legal way.
 * Please read readme.txt, included in this package, to see further possibilities on how to use/execute this code. 
 */

[Serializable()]
public class SavedState : ISerializable {
	//so far 3 state attributes are saved: position, rotation, and if the game object was emitting particles when being in this state
	public SerVector3 position;
	public SerVector3 localPosition;
	public SerQuaternion rotation;
	public SerQuaternion localRotation;
	
	private bool emittingParticles = false;
	
	//serialization constructor
	protected SavedState(SerializationInfo info,StreamingContext context) {
		
		this.position = (SerVector3)info.GetValue("position",typeof(SerVector3));
		this.localPosition = (SerVector3)info.GetValue("localPosition",typeof(SerVector3));
		this.rotation = (SerQuaternion)info.GetValue("rotation",typeof(SerQuaternion));
		this.localRotation = (SerQuaternion)info.GetValue("localRotation",typeof(SerQuaternion));	
		
		emittingParticles = info.GetBoolean("emittingParticles");
	}			
	
	//as this is not derived from MonoBehaviour, we have a constructor
	public SavedState(GameObject go) {

	//	if(go.GetComponent<ParticleEmitter>())
	//		emittingParticles = go.GetComponent<ParticleEmitter>().emit;
		
		this.position = new SerVector3(go.transform.position);
		this.rotation = new SerQuaternion(go.transform.rotation);
		this.localPosition = new SerVector3(go.transform.localPosition);
		this.localRotation = new SerQuaternion(go.transform.localRotation);
		
	}
	
	public Vector3 serVec3ToVec3(SerVector3 serVec3) {
		return new Vector3(serVec3.x,serVec3.y,serVec3.z);
	}
	
	public Quaternion serQuatToQuat(SerQuaternion serQuat) {
		return new Quaternion(serQuat.x,serQuat.y,serQuat.z,serQuat.w);
	}	
	
	public bool isDifferentTo(SavedState otherState) {
		bool changed = false;
		
		if (!changed && position.isDifferentTo(otherState.position) )
			changed = true;
		
		if (!changed && rotation.isDifferentTo(otherState.rotation) )
			changed = true;
		
		if (!changed && localPosition.isDifferentTo( otherState.localPosition) )
			changed = true;
		
		if (!changed && localRotation.isDifferentTo( otherState.localRotation) )
			changed = true;
		
		if (!changed && emittingParticles != otherState.emittingParticles)
			changed = true;
		
		return changed;
	}
	
	//called to synchronize gameObjectClone of Object2PropertiesMapping back to this saved state
	public void synchronizeProperties(GameObject go) {

		//lerping has not been integrated yet.
		//if (lerpInterval > 0f) {
			//MonoBehaviour.print("lerpInterval: "+lerpInterval);
			//MonoBehaviour.print("Time.time * lerpInterval: "+Time.time * lerpInterval);
			//MonoBehaviour.print("Time.time: "+Time.time);
			//float lerpInterval = 0.08f;	
		
			/*go.transform.position = Vector3.Lerp(go.transform.position, this.position.getVector3(), Time.deltaTime * (Time.time/lerpInterval));
			go.transform.localPosition = Vector3.Lerp(go.transform.localPosition, this.localPosition.getVector3(), Time.deltaTime * (Time.time/lerpInterval));
			go.transform.rotation = Quaternion.Lerp(go.transform.rotation, this.rotation.getQuaternion(), Time.deltaTime * (Time.time/lerpInterval));
			go.transform.localRotation = Quaternion.Lerp(go.transform.localRotation, this.localRotation.getQuaternion(), Time.deltaTime * (Time.time/lerpInterval));*/
		//} else {

			//go.transform.position = this.position;
			go.transform.position = serVec3ToVec3(this.position);
			go.transform.rotation = serQuatToQuat(this.rotation);
		
			go.transform.localPosition = serVec3ToVec3(this.localPosition);
			go.transform.localRotation = serQuatToQuat(this.localRotation);
		//}
		
	//	if (emittingParticles) 
	//		go.GetComponent<ParticleEmitter>().emit = true;
	//	else if ( go.GetComponent<ParticleEmitter>() ) 
	//		go.GetComponent<ParticleEmitter>().emit = false;
	}
	
	/*[SecurityPermissionAttribute(
	            SecurityAction.Demand,
	            SerializationFormatter = true)]		*/
	public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		
		info.AddValue("position",position);		
		info.AddValue("localPosition",localPosition);
		info.AddValue("rotation",rotation);		
		info.AddValue("localRotation",localRotation);		
		
		info.AddValue("emittingParticles", this.emittingParticles);
		//base.GetObjectData(info, context);
	}	
	
}
