package {
	import flash.display.Bitmap;
	import flash.display.BitmapData;
	import flash.display.DisplayObject;
	import flash.display.Shape;
	import flash.events.Event;
	import flash.filters.BlurFilter;
	import flash.filters.DisplacementMapFilter;
	import flash.filters.GlowFilter;
	import flash.geom.Matrix;
	import flash.geom.Point;
	import flash.geom.Rectangle;
	import flash.utils.getDefinitionByName;

	import gs.*;
	import gs.easing.*;
	import caurina.transitions.*;


	public class GlitchEffect extends Shape {

		// internal use only
		private var glitchMap:BitmapData;
		private var map:DisplacementMapFilter;
		private var glow:GlowFilter;
		private var filter:Bitmap;
		private var target:DisplayObject;

		public var count:Number=0;
		private var isDisplaceing:Boolean=true;
		public var xMod:Number=0;
		public var yMod:Number=0;
		private var killed:Boolean=false;
		public var frequeny:Number = 30;
		public var strength:Number = 50;

		// public

		public function GlitchEffect(_target:DisplayObject, picID:String,frq:Number,str:Number) {
			target = _target;
			frequeny = frq;
			strength = str;
			var imgClass:Class=getDefinitionByName(picID) as Class;
			glitchMap = new imgClass(0, 0);
			
			var miRect:Rectangle = new Rectangle(0, 0, target.width, target.height);
			var miBMD:BitmapData = new BitmapData(target.width, target.height);
			var miMatrix = new Matrix();
			miMatrix.scale(target.width/glitchMap.width,target.height/glitchMap.height);
			miBMD.draw(glitchMap,miMatrix);

			var blur:BlurFilter=new BlurFilter(2,2,1);
			//glitchMap.applyFilter(glitchMap,glitchMap.rect,new Point(0,0),blur);
			glitchMap.applyFilter(miBMD,miRect,new Point(0,0),blur);
			//map=new DisplacementMapFilter(glitchMap,new Point(0,0),2,2,xMod,yMod,"wrap",0x000000,0x000000);
			map=new DisplacementMapFilter(miBMD,new Point(0,0),2,2,xMod,yMod,"wrap",0x000000,0x000000);
			//target.filters = [map]


			
			glow=new GlowFilter(0x41fdff);
			glow.blurX=15;//28
			glow.blurY=15;//28
			glow.strength=0;//0.7
			glow.quality=1;
			//target.filters = [glow]
		}

		public function startGlitching():void {
			addEventListener(Event.ENTER_FRAME, update, false, 0, true);
		}

		public function stopGlitching(aph:Number = 1):void {
			removeEventListener(Event.ENTER_FRAME, update);
			target.filters = [];//[glow]
			target.alpha = aph;
		}

		public function sendShock(amount:Number):void {
			//trace("Shock");
			isDisplaceing=true;
			//TweenLite.to(this, 0.15, {xMod : (Math.random() * amount * 2)-amount, ease:Sine.easeIn});
			//TweenLite.to(this, 0.15, {yMod : (Math.random() * amount * 2)-amount, ease:Sine.easeIn, onComplete: goBack});
			
			//Tweener.addTween(this, {xMod:(Math.random() * amount * 2)-amount,time:0.15, transition:"easeOutSine"});  //DESCOMENTAR PARA DISPLACEMENT HORIZONTAL
			Tweener.addTween(this, {yMod:(Math.random() * amount * 2)-amount,time:0.15, transition:"easeOutSine", onComplete: goBack});

			//.addTween(this, "xMod", (Math.random() * amount * 2)-amount, 0.15, "easeOutSine");
			//Tweener.tween(this, "yMod", (Math.random() * amount * 2)-amount, 0.15, "easeOutSine", 0,{onFinish:goBack});
		}

		function goBack():void {
			//trace('GO BACK');
			//TweenLite.to(this, 0.2, {xMod : 0, ease:Sine.easeIn});
			//TweenLite.to(this, 0.2, {yMod :0, ease:Sine.easeIn, onComplete: onGlitchComplete});
			//Tweener.tween(this, "xMod", 0, 0.2, "easeOutSine");
			//Tweener.tween(this, "yMod", 0, 0.2, "easeOutSine", 0, {onFinish:onGlitchComplete});
			
			//Tweener.addTween(this, {xMod:0,time:0.2, transition:"easeOutSine"}); //DESCOMENTAR PARA DISPLACEMENT HORIZONTAL
			Tweener.addTween(this, {yMod:0,time:0.2, transition:"easeOutSine", onComplete: onGlitchComplete});
		}

		public function kill():void {
			removeEventListener(Event.ENTER_FRAME, update);
			target.filters=[];
			//glitchMap.dispose();
			
			map=null;
			glow=null;
			filter=null;
			target=null;
			killed=true;

		}
		// private

		private function update(evnt:Event):void {

			target.alpha += ((0.5 + Math.random()*0.6) - target.alpha)/5;
			count++;
			if (isDisplaceing==true) {

				//var aggres = Math2.convert(Math.sin(count/5), -1, 1, 0, 8);

				map.scaleX=xMod;// + Math2.random(-1, 1)*aggres
				map.scaleY=yMod;// + Math2.random(-1, 1)*aggres

				target.filters=[map,glow];


			}

			if (count==frequeny) {
				sendShock(strength);
				count=0;
			}
		}

		//

		private function onGlitchComplete():void {
			if (! killed) {
				isDisplaceing=false;
				target.filters=[glow];
			}
		}

	}
}