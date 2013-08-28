package
{
	import flash.display.MovieClip;
	import flash.display.Sprite;
	import caurina.transitions.Tweener;
	import flash.events.Event;	
	import flash.events.EventDispatcher;
	import GlitchEffect;
	
	public class Efectos extends EventDispatcher{
			
		//private var global:Global = Global.getInstance();
		
		public function Efectos() {
			
		}
		
		public function fadeInIntro(mc,map) {
			mc.fondo_intro.alpha = 0;
			mc.content.alpha = 0;
			mc.botella.alpha = 0;
			Tweener.addTween(mc.fondo_intro, { alpha:1, delay:0, time:1, transition:"linear", onComplete:function() {
				var glitch:GlitchEffect = new GlitchEffect(mc.content, map, 30, 50);
				var glitch2:GlitchEffect = new GlitchEffect(mc.botella,map,30,50);
				glitch.startGlitching();
				glitch2.startGlitching();
				Tweener.addTween(mc.content, { alpha:1, time:1.5, transition:"linear", onComplete:function() {
					glitch.stopGlitching();
				} } );
				Tweener.addTween(mc.botella, { alpha:1, time:1.5, transition:"linear", onComplete:function() {
					glitch2.stopGlitching();
				} } );
			} } );
			
		}
		
		public function fadeOutIntro(mc,map) {
			var glitch:GlitchEffect = new GlitchEffect(mc.content,map,30,50);
			glitch.startGlitching();
			var glitch2:GlitchEffect = new GlitchEffect(mc.botella,map,30,50);
			glitch2.startGlitching();
			Tweener.addTween(mc.content, { alpha:0, time:1.5, transition:"linear", onComplete:function() {
				mc.content.visible = false;
				glitch.stopGlitching();
				dispatchEvent( new Event("fin_prehome") );
			} } );
			Tweener.addTween(mc.botella, { alpha:0, time:1.5, transition:"linear", onComplete:function() {
				mc.botella.visible = false;
				glitch2.stopGlitching();
			} } );
			
		}
		
		public function parpadeo(mc,off:Boolean) {
			
			var glitch:GlitchEffect = new GlitchEffect(mc, "DistortionMap",10,50);
			glitch.startGlitching();
			var dur:Number = .1;
			if(off){
				Tweener.addTween(mc.estatic.bmp, { alpha:1, time:dur, delay:0, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc.estatic.bmp, { alpha:.8, time:dur, delay:dur, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc.estatic.bmp, { alpha:.6, time:dur, delay:dur*2, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc.estatic.bmp, { alpha:1, time:dur, delay:dur*3, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc.estatic.bmp, { alpha:.7, time:dur, delay:dur*4, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc.estatic.bmp, { alpha:.9, time:dur, delay:dur*5, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc.estatic.bmp, { alpha:.5, time:dur, delay:dur*6, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc.estatic.bmp, { alpha:.3, time:dur, delay:dur*7, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc.estatic.bmp, { alpha:.5, time:dur, delay:dur*8, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc.estatic.bmp, { alpha:.2, time:dur, delay:dur*9, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc.estatic.bmp, { alpha:0, time:dur, delay:dur * 10, transition:"linear", onComplete:function() { 
					glitch.stopGlitching();
				}} );
			} else {
				Tweener.addTween(mc, { alpha:1, time:dur, delay:0, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc, { alpha:.5, time:dur, delay:dur, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc, { alpha:.7, time:dur, delay:dur*2, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc, { alpha:.2, time:dur, delay:dur*3, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc, { alpha:.8, time:dur, delay:dur*4, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc, { alpha:1, time:dur, delay:dur * 5, transition:"linear", onComplete:function() { 
					glitch.stopGlitching();
				}} );		
			}
			
		}
		
		public function parpadeoIn(mc) {
			var glitch:GlitchEffect = new GlitchEffect(mc, "DistortionMap",10,80);
			glitch.startGlitching();
			var dur:Number = .1;
				Tweener.addTween(mc, { alpha:0, time:dur, delay:0, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc, { alpha:.3, time:dur, delay:dur, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc, { alpha:0, time:dur, delay:dur*2, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc, { alpha:.4, time:dur, delay:dur*3, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc, { alpha:.2, time:dur, delay:dur*4, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc, { alpha:.7, time:dur, delay:dur*5, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc, { alpha:.5, time:dur, delay:dur*6, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc, { alpha:.8, time:dur, delay:dur*7, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc, { alpha:.4, time:dur, delay:dur*8, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc, { alpha:.7, time:dur, delay:dur*9, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc, { alpha:1, time:dur, delay:dur * 10, transition:"linear", onComplete:function() { 
					glitch.stopGlitching(1);
				}} );
			
		}
		
		public function parpadeoOut(mc,fr:Number = 10,str:Number = 80) {
			var glitch:GlitchEffect = new GlitchEffect(mc, "DistortionMap",fr,str);
			glitch.startGlitching();
			var dur:Number = .1;
				Tweener.addTween(mc, { alpha:1, time:dur, delay:0, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc, { alpha:.7, time:dur, delay:dur, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc, { alpha:1, time:dur, delay:dur*2, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc, { alpha:.5, time:dur, delay:dur*3, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc, { alpha:.8, time:dur, delay:dur*4, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc, { alpha:.4, time:dur, delay:dur*5, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc, { alpha:.6, time:dur, delay:dur*6, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc, { alpha:.2, time:dur, delay:dur*7, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc, { alpha:.5, time:dur, delay:dur*8, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc, { alpha:.3, time:dur, delay:dur*9, transition:"linear", onComplete:function() { }} );
				Tweener.addTween(mc, { alpha:0, time:dur, delay:dur * 10, transition:"linear", onComplete:function() { 
					glitch.stopGlitching(0);
					mc.alpha = 0;
				}} );
			
		}
		
	}
	
}