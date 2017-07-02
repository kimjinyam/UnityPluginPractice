using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AndroidPreviewCam : MonoBehaviour {

	/*
	private static AndroidPreviewCam _instance;
	public static AndroidPreviewCam Instance{
		get{
			if(_instance == null) _instance = new AndroidPreviewCam();
			return _instance;
		}
	}

	public void logAndroid(string tag,string message){
		AndroidJavaObject jo =  new AndroidJavaObject("com.nexxmobile.unityplugin.AndroidLogger");
		jo.Call("logAndroid",tag,message);
	}

	#if UNITY_ANDROID
	private static AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
	private static AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"); 


	public void makeToast(string message)
	{
		jo.Call("makeToast",message);
	}

	#endif
	void OnApplicationQuit() {
		_instance = null;
	}
*/

	//var plugin;



	string messge = "init";
	AndroidJavaClass myCls;
	AndroidJavaObject myObj;

	private Texture defaultbackground;
	private Texture2D RGB_tex;

	public AspectRatioFitter fitter;
	public RawImage background;

	private int[] pixels;
	private byte[] pixels_b;
	private byte[] Cam_YUV_Raw;
	private int Yb, Ub, Vb;
	private int R, G, B;
	private int YUV_data_length;

	private int texWidth;
	private int texHeight;

	// Use this for initialization
	void awake()
	{
		
	}

	void Start () {
		// 새로룬 오브젝트를 만들지 않고, com.unity3d.player.UnityPlayer의
		// static 멤버에 접근하기 위해 AndroidJavaClass를 사용한다.
		// (사실 Android UnityPlayer가 자동으로 인스턴스를 생성해준다)
		myCls = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		// 그리고 static 필드인 currentActivity를 접근한다.
		// 그리고 이경우 AndroidJavaObject를 사용한다.
		// 이유는 실제 필드 타입이 android.app.Activity이고
		// 이건 java.lang.Object를 상속받는다.
		// 그리고, non-primitive 타입은 무조껀 AndroidJavaObject로 접근해야한다.
		// 예외 : strings.
		myObj = myCls.GetStatic<AndroidJavaObject>("currentActivity");

		//this.GetComponent<Renderer> ().material.mainTexture = camTexture;





		//int a = myCls.Call<int>("get_M_start_stat");
		//defaultbackground = background.texture;
		defaultbackground = RGB_tex;
		//background.texture = Cam_YUV_Raw; // 이런식으로 되게
		//background.texture = pixels;
	}
	
	// Update is called once per frame
	void Update () {
		//textMesh.text = javaobject.Call<int> ("get_tecture_width").ToString();
		TextMesh textMesh = GetComponent<TextMesh> ();
		//textMesh.text = javaobject.Call<string> ("getTeststring");
		//textMesh.text = myObj.Call<int> ("get_M_start_stat").ToString();
		textMesh.text = myObj.Call<int> ("teststring").ToString();

		//camTexture = myObj.Call<RenderTexture> ("get_image");
		//Cam_YUV_Raw = myObj.Call<byte[]>("get_image");
		//Cam_YUV_Raw = myCls.Call<byte[]>("get_image"); // nv21 상태
		//Yb = myCls.Call<int> ("get_Yb");
		//Ub = myCls.Call<int> ("get_Ub");
		//Vb = myCls.Call<int> ("get_Vb");
		//YUV_data_length = myCls.Call<int> ("get_data_length");
		//texWidth = myCls.Call<int> ("get_texture_width");
		//texHeight = myCls.Call<int> ("get_texture_height");
		pixels = new int[texWidth * texHeight];

		// RGB로 변환
		decodeYUV420SP(pixels, Cam_YUV_Raw, texWidth, texHeight);

		pixels_b = toByteArray (pixels);

		// RGB 변환된 변수를 Texture2D에서 사용 할 수 있게 변환
		RGBbyte2Textrue2D(pixels_b, texWidth, texHeight);

		float ratio = (float)texWidth / (float)texHeight;
		fitter.aspectRatio = ratio;


		//textMesh.text = texHeight.ToString();
		//TextMesh textMesh = GetComponent<TextMesh> ();

	}

	//http://www.41post.com/3470/programming/android-retrieving-the-camera-preview-as-a-pixel-array
	void decodeYUV420SP(int[] rgb, byte[] yuv420sp, int width, int height) {  

		int frameSize = width * height;  

		for (int j = 0, yp = 0; j < height; j++) {       int uvp = frameSize + (j >> 1) * width, u = 0, v = 0;  
			for (int i = 0; i < width; i++, yp++) {  
				int y = (0xff & ((int) yuv420sp[yp])) - 16;  
				if (y < 0)  
					y = 0;  
				if ((i & 1) == 0) {  
					v = (0xff & yuv420sp[uvp++]) - 128;  
					u = (0xff & yuv420sp[uvp++]) - 128;  
				}  

				int y1192 = 1192 * y;  
				int r = (y1192 + 1634 * v);  
				int g = (y1192 - 833 * v - 400 * u);  
				int b = (y1192 + 2066 * u);  

				if (r < 0)                  r = 0;               else if (r > 262143)  
					r = 262143;  
				if (g < 0)                  g = 0;               else if (g > 262143)  
					g = 262143;  
				if (b < 0)                  b = 0;               else if (b > 262143)  
					b = 262143;  

				//rgb[yp] = 0xff000000 | ((r << 6) & 0xff0000) | ((g >> 2) & 0xff00) | ((b >> 10) & 0xff);  
				rgb[yp] = ((r << 6) & 0xff0000) | ((g >> 2) & 0xff00) | ((b >> 10) & 0xff);  
			}  
		}  
	}

	byte[] toByteArray(int[] ints)	{
		byte[] bytes = new byte[ints.Length * 4];
		for (int i = 0; i < ints.Length; i++) {
			bytes [i * 4] = (byte)(ints [i] & 0xFF);
			bytes [i * 4 + 1] = (byte)((ints [i] & 0xFF00) >> 8);
			bytes [i * 4 + 2] = (byte)((ints [i] & 0xFF0000) >> 16);
			bytes [i * 4 + 3] = (byte)((ints [i] & 0xFF000000) >> 24);

		}
		return bytes;
	}

	void RGBbyte2Textrue2D(byte[] RGBbyte, int width, int height)
	{
		var colorArray = new Color32[width * height / 4];
		for (int i = 0; i < width * height; i += 4) {
			var color = new Color32 (RGBbyte [i + 0], RGBbyte [i + 1], RGBbyte [i + 2], RGBbyte [i + 3]);
			colorArray [i / 4] = color;
		}
		RGB_tex.SetPixels32 (colorArray);
	}


}