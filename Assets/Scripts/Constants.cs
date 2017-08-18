using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants {

	/// BARCODE CONSTANTS
	public static int BARCODE_HEIGHT = 256;
	public static int BARCODE_WIDTH = 256;
	public static float SCAN_RATE = 1;
	public static float THUMBNAIL_DISPLAY_TIME = 1.5f;

	/// SERVER REQUEST FIELDS
	public static string[] CAREGIVER_PAIRING_FIELDS = {"patientID", "platform"};
	public static string[] CAREGIVER_THUMBNAIL_URL_REQUEST_FIELDS = {"thumbnailID"};
	public static string[] CAREGIVER_EXPERIENCE_REQUEST_FIELDS = {"contentID", "sceneName", "assetBundle"};
	public static string[] CAREGIVER_MOVE_POSITION_VIEW_FIELDS = {"dir"};
	public static string[] PATIENT_SEND_VIEW_REQUEST_FIELDS = {"tex"};
	public static string[] PATIENT_SEND_PLATFORM_REQUEST_FIELDS = {"platform"};
	public static string[] CAREGIVER_SEND_AVATAR_REQUEST_FIELDS = {"avatar"};
	public static string[] CAREGIVER_SEND_AVATAR_FILE_FIELDS = {"avatar"};
	public static string[] CAREGIVER_DOWNLOAD_ANIMATION_REQUEST_FIELDS = {"anim"};

	/// MANIFEST FIELDS
	public static string MANIFEST_TAGS_FIELD = "tags";
	public static string MANIFEST_TAG_LIST_FIELD = "allTags";
	public static string MANIFEST_EXPERIENCE_ID = "key"; 
	public static string MANIFEST_EXPERIENCE_NAME = "name";
	public static string MANIFEST_EXPERIENCE_THUMBNAILS = "thumbnails";
	public static string MANIFEST_EXPERIENCE_SUMMARY = "info";
	public static string MANIFEST_EXPERIENCE_SCENE_NAME = "sceneName";
	

	/// SERVER RESPONSE FIELDS
	public static string THUMBNAIL_URL_FIELD = "url";
	public static string CONTENT_ID_FIELD = "contentID";
	public static string CONTENT_URL_FIELD = "contentURL";
	public static string SCENE_NAME_FIELD = "sceneName";
	public static string ASSET_BUNDLE_NAME_FIELD = "assetBundle";
	public static string MOVE_POSITION_CAMERA_FIELD = "dir";
	public static string AVATAR_FILE_FIELD = "avatar";
	public static string NUMBER_OF_ANIMATIONS_IN_LIBRARY_FIELD = "animNum";
	public static string ANIMATION_FIELD_PREFIX = "anim";
	public static string ANIMATION_DURATION_FIELD_SUFFIX = "duration";
	public static string ANIMATION_URL_FIELD_SUFFIX = "url";
	
	/// FIELDS TO UPLOAD ASSETBUNDLE
	public static string UPLOAD_ASSET_BUNDLE_NAME_FIELD = "fileName";
	public static string UPLOAD_ASSET_BUNDLE_INFO_FIELD = "info";
	public static string UPLOAD_ASSET_BUNDLE_TAGS_FIELD = "tags";
	public static string UPLOAD_ASSET_BUNDLE_SCENE_NAME_FIELD = "sceneName";
	public static string UPLOAD_ASSET_BUNDLE_PLATFORM_FIELD = "platformName";
	public static string UPLOAD_ASSET_BUNDLE_THUMBNAIL_NUMBER_FIELD = "thumbNum";
	public static string UPLOAD_ASSET_BUNDLE_BUNDLE_FIELD = "bundle";

	/// FIELDS TO UPLOAD ANIMATION ASSETBUNDLE
	public static string UPLOAD_ANIMATION_NUMBER_OF_BUNDLES_FIELD = "animNum";
	public static string UPLOAD_ANIMATION_BUNDLE_FIELD = "-bundle";
	public static string UPLOAD_ANIMATION_BUNDLE_TAGS_FIELD = "tags";
	public static string UPLOAD_ANIMATION_BUNDLE_NAME_FIELD = "bundleName";
	public static string UPLOAD_ANIMATION_BUNDLE_PLATFORM_FIELD = "platformName";


	/// SOCKET IO MESSAGES
	public static string CONNECTED_MESSAGE = "open";
	public static string CAREGIVER_THUMBNAIL_URL_REQUEST_MESSAGE = "caregiver:requestThumbnail";
	public static string CAREGIVER_THUMBNAIL_URL_RESPONSE_MESSAGE = "caregiver:thumbnailURL";
	public static string CAREGIVER_EXPERIENCE_MANIFEST_RESPONSE_MESSAGE = "caregiver:experienceManifest";
	public static string CAREGIVER_EXPERIENCE_REQUEST_MESSAGE = "caregiver:selectContent";
	public static string CAREGIVER_TOGGLE_ON_POSITION_VIEW = "caregiver:toggleOnPositionView";
	public static string CAREGIVER_TOGGLE_OFF_POSITION_VIEW = "caregiver:toggleOffPositionView";
	public static string CAREGIVER_MOVE_POSITION_VIEW_MESSAGE = "caregiver:movePositionView";
	public static string CAREGIVER_SELECT_POSITION_MESSAGE = "caregiver:selectPosition";
	public static string CAREGIVER_CONNECTED_MESSAGE = "caregiver:connected";
	public static string CAREGIVER_GET_CONTENT_MESSAGE = "caregiver:getContentInfo";
	public static string CAREGIVER_PAIRED_MESSAGE = "caregiver:paired";
	public static string CAREGIVER_RECEIVE_PATIENT_VIEW_MESSAGE = "caregiver:receivePatientView";
	public static string CAREGIVER_RECEIVE_POSITION_VIEW_MESSAGE = "caregiver:receivePositionView";
	public static string CAREGIVER_SEND_AVATAR_MESSAGE = "caregiver:sendAvatar";
	public static string CAREGIVER_SEND_AVATAR_DNA_MESSAGE = "caregiver:sendAvatarDNA";
	public static string CAREGIVER_SEND_AVATAR_COLORS_MESSAGE = "caregiver:sendAvatarColors";
	public static string CAREGIVER_SEND_AVATAR_FILE_MESSAGE = "caregiver:sendAvatarFile";
	public static string CAREGIVER_RECEIVE_ANIMATION_LIBRARY_MESSAGE = "caregiver:receiveAnimationLibrary";
	public static string CAREGIVER_DOWNLOAD_ANIMATION_MESSAGE = "caregiver:downloadAnimation";
	public static string CAREGIVER_DOWNLOAD_ANIMATION_RECEIVE_URL_MESSAGE = "caregiver:downloadAnimURL";
	public static string CAREGIVER_GET_ANIMATION_LIBRARY_MESSAGE = "caregiver:getAnimationLibrary";
	public static string CAREGIVER_SEND_ANIMATIONS_MESSAGE = "caregiver:sendAnimations";

	public static string PATIENT_PAIRED_MESSAGE = "patient:paired";
	public static string PATIENT_RECEIVE_CONTENT_URL_MESSAGE = "patient:contentURL";
	public static string PATIENT_REQUEST_CONTENT_URL_MESSAGE = "patient:requestContent";
	public static string PATIENT_CONNECTED_RESPONSE_MESSAGE = "patient:connected";
	public static string PATIENT_CONNECTED_MESSAGE = "patient:connected";
	public static string PATIENT_SEND_POSITIONING_VIEW_MESSAGE = "patient:sendPositionView";
	public static string PATIENT_SEND_VIEW_MESSAGE = "patient:sendPatientView";
	public static string PATIENT_RECEIVE_POSITION_VIEW_MOVE_MESSAGE = "patient:movePositionView";
	public static string PATIENT_SET_NEW_POSITION_VIEW_MESSAGE = "patient:setPositionView";
	public static string PATIENT_TOGGLE_ON_POSITION_VIEW_MESSAGE = "patient:toggleOnPositionView";
	public static string PATIENT_TOGGLE_OFF_POSITION_VIEW_MESSAGE = "patient:toggleOffPositionView";
	public static string PATIENT_RECEIVE_AVATAR_MESSAGE = "patient:receiveAvatar";
	public static string PATIENT_RECEIVE_AVATAR_DNA_MESSAGE = "patient:receiveAvatarDNA";
	public static string PATIENT_RECEIVE_AVATAR_COLORS_MESSAGE = "patient:receiveAvatarColors";
	public static string PATIENT_RECEIVE_AVATAR_FILE_MESSAGE = "patient:receiveAvatarFile";
	public static string PATIENT_RECEIVE_ANIMATIONS_MESSAGE ="patient:receiveAnimations";


	/// SERVER CONSTANTS
	public static string SERVER_ADDRESS = "52.11.191.18";// "192.168.1.4";
	public static string SERVER_PORT = "3030";
	public static string SERVER_URL = "http://" + SERVER_ADDRESS + ":" + SERVER_PORT;
	public static string SERVER_API_UPLOAD_BUNDLE = "/experience";
	public static string SERVER_API_UPLOAD_ANIMATION = "/animation";

	
/// AVATAR 
	public static List<string> AVATAR_DNA = new List<string>(){
		"height",
		"armLength",
		"armWidth",
		"forearmLength",
		"forearmWidth",
		"handsSize",
		"legsSize",
		"legSeparation",
		"feetSize",
		"upperMuscle",
		"upperWeight",
		"lowerMuscle",
		"lowerWeight",
		"belly",
		"waist",
		"gluteusSize"

	};

	/// ANIMATION PREVIEW 
	public static string ENTER_ANIMATIONS_BOOL = "animPreview";
	public static string TOGGLE_MODIFIED_ANIM_BOOL = "toggleModAnim";
	public static string DEFAULT_ANIM_1 = "BindPose";
	public static string DEFAULT_ANIM_2 = "ButtonPush_LH";

	///SENDING PATIENT VIEW
	public const float FRAME_RATE = 5;
	public static float CAMERA_MOVE_AMOUNT = 1;
	public static float PLACEMENT_DISTANCE = 25;
	public enum CameraControls
	{
		FORWARD = 0,
		BACKWARD = 1,
		LEFT = 2,
		RIGHT = 3,
		UP = 4,
		DOWN = 5,
		ZOOM_IN = 6,
		ZOOM_OUT = 7,
		ROTATE_PLUS_X = 8,
		ROTATE_MINUS_X = 9,
		ROTATE_PLUS_Y = 10,
		ROTATE_MINUS_Y = 11,
		ROTATE_PLUS_Z = 12,
		ROTATE_MINUS_Z = 13

	};

	/// MISC
	public static string ANIMATION_BUCKET_SUFFIX = "-animations";
	public static string UMA_OUT_FILE = "umaOut.txt";
}
