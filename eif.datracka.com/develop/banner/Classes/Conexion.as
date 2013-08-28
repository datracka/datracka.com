package {

import flash.display.MovieClip;
import flash.events.*;
import flash.net.URLLoader;
import flash.net.URLRequest;
import flash.net.URLVariables;
import flash.net.URLRequestMethod;
import flash.net.URLRequestHeader;
	
public class Conexion extends EventDispatcher{
   
   //////VARIABLES GLOBALES/////
   private var miEvt:EventDispatcher;
   //private var global:Global = Global.getInstance();
   /////////////////////////////
   //////CONTANTES GLOBALES/////
   
   /////////////////////////////
   
   ////////CONSTANTES DE CARGA//////
   
   /////////////////////////////////////
	
	///////CONSTANTES DE EVENTO/////////
	
	////////////////////////////////////
	
   private static var instance:Conexion;
   private static var creatingSingleton:Boolean = false;
   
      public function Conexion() {
         if ( !creatingSingleton ) {
			 throw new Error( "Singleton and can only be accessed through Singleton.getInstance()" )
		 } else {
		 }
      }
      
	  
	  public static function getInstance():Conexion {
         if( !instance ){
            creatingSingleton = true;
            instance = new Conexion();
            creatingSingleton = false;			
         }
         return instance;
      }
	  
	public function peticion(url:String, obj:Object, handler:String,data:URLVariables,type:String = "POST"):void {
		var request:URLRequest = new URLRequest(url);
		//request.contentType = "text/xml"; 
		if(type=="POST"){
			request.method = URLRequestMethod.POST;
	    } else {
			request.method = URLRequestMethod.GET;
		}
		request.data = data;
		var loader:URLLoader = new URLLoader();
		loader.addEventListener(Event.COMPLETE, function(evt:Event) { loadHandler(evt, obj, handler); } );
		loader.addEventListener(IOErrorEvent.IO_ERROR, function(evt:Event) { errorHandler(evt, obj, handler); });
		loader.addEventListener(ProgressEvent.PROGRESS, progressHandler);
		loader.addEventListener(HTTPStatusEvent.HTTP_STATUS, httpStatusHandler);
		loader.load(request);
	  }
	  
	  public function loadHandler(evt:Event,obj:Object,handler:String):void {
		try {
			var xmlResponse:XML = new XML(evt.target.data);
			obj[handler](xmlResponse);
		} catch (err:TypeError) {
			//var msg:message = new message();
			//msg.setText(global.arrTxt["ERROR_9"]);
		}
	  }

	  public function errorHandler(evt:Event,obj:Object,handler:String):void{
			var xmlResponse:XML = <salida><estado>0</estado><error>ERROR DE CONEXION CON EL SERVIDOR</error></salida>;
			obj[handler](xmlResponse);
			//var msg:message = new message();
			//msg.setText(global.arrTxt["ERROR_9"]);	
      }

	  private function progressHandler(event:ProgressEvent):void {
           // trace("progressHandler loaded:" + event.bytesLoaded + " total: " + event.bytesTotal);
      }
	  
	  private function httpStatusHandler(event:HTTPStatusEvent):void {
           //s trace("httpStatusHandler: " + event);
      }

	 
}

}