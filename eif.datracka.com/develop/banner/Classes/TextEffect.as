package
{
	import flash.display.MovieClip;
	import flash.display.Sprite;
	import flash.events.Event;
	import flash.events.ProgressEvent;
	import flash.net.*;
	import flash.text.TextField;
	import flash.geom.Rectangle;
	import flash.display.Graphics;
	import flash.text.TextLineMetrics; 
	

	public class TextEffect{
		
		//private var global:Global = Global.getInstance();
		//private var style:Style = Style.getInstance();
		
		public function TextEffect() {
			
		}
		
		public function blackBox(txt:String,textBox:TextField,color:Number=0x000000,desplazamiento:Number=0) {
			
			trace("text effect");
			
			var mistr:String = new String();			
			
			textBox.multiline = true;
			
			mistr = textBox.htmlText;
			
			var inicio:Number = textBox.text.indexOf("[caja]")+desplazamiento;
			var fin:Number = textBox.text.indexOf("[/caja]") - 7+desplazamiento;
			
			mistr = clear(textBox.htmlText);
			//clear(textBox.htmlText);
			
			textBox.htmlText = mistr;
			
			
			var lineaInicio:Number = textBox.getLineIndexOfChar (inicio);
			var lineaFin:Number = textBox.getLineIndexOfChar (fin);
			
			
			/////////// FALTA CREAR UN ARRAY DE INICIO Y FIN, PER EL CAS QUE N'HI HAGI MÉS D'UN.
			
			
			var numeroLineas:Number = lineaFin-lineaInicio;
			var caracterIniciLineasVariables:Number;
			var caracterFiLineasVariables:Number;			
			
			
			for (var i:Number = 0; i <= numeroLineas; i++) {
								
				switch (i){
					
					case 0:
						
						if(numeroLineas!=0){
							caracterIniciLineasVariables=inicio;
							caracterFiLineasVariables=textBox.getLineOffset(lineaInicio+i+1);
							caracterFiLineasVariables = caracterFiLineasVariables - 1;
						}else {
							caracterIniciLineasVariables=inicio;
							caracterFiLineasVariables = fin;
						}
					break;
					
					case (numeroLineas):
						if(numeroLineas!=0){							
							caracterIniciLineasVariables=textBox.getLineOffset(lineaInicio+i);
							caracterFiLineasVariables = fin;
						}
					break;
					
					default:						
						caracterIniciLineasVariables=textBox.getLineOffset(lineaInicio+i);
						caracterFiLineasVariables=textBox.getLineOffset(lineaInicio+i+1);
						caracterFiLineasVariables = caracterFiLineasVariables-1;					
				}	
				
				
				var ultimRect:Rectangle = textBox.getCharBoundaries(caracterFiLineasVariables);
				while ((ultimRect == null)&&(caracterFiLineasVariables>caracterIniciLineasVariables)) {
					caracterFiLineasVariables--;					
					ultimRect = textBox.getCharBoundaries(caracterFiLineasVariables);
				}
				
				trace("apunto de crear cuadro");
				if (ultimRect != null) {
					trace("creandoCuadro cuadro");
					var rect:Rectangle = textBox.getCharBoundaries(caracterIniciLineasVariables);					
					var xrect = rect.x;
					var yrect = rect.y;
					var hrect = rect.height;		
															
					var widthrect:Number = ultimRect.x -rect.x + ultimRect.width;			
										
					rect = new Rectangle(xrect, yrect, widthrect, hrect);
					
					var cont:Sprite = new Sprite;
					//textBox.parent.addChildAt(cont,textBox.parent.getChildIndex(textBox)-1);
					textBox.parent.addChildAt(cont,textBox.parent.getChildIndex(textBox));
					cont.x = textBox.x;
					cont.y = textBox.y+1;
					
					cont.graphics.beginFill(color,1);
					cont.graphics.drawRect(xrect-1, yrect, widthrect+2, hrect);
					cont.graphics.endFill();
				}		
			}

		}
		
		private function clear(txt:String):String {
			var mitxtClear:String;
			var myPattern:RegExp = /\[caja\]/i;  
			mitxtClear = txt.replace(myPattern, "");
			myPattern = /\[\/caja\]/i;
			mitxtClear.replace(myPattern, "");
			return mitxtClear.replace(myPattern, ""); 
		}
		
		public function subParrafo(textBox:TextField,despX:Number,despY:Number,color:Number=0x000000) {
			var metric:TextLineMetrics;
			
			for (var i = 0; i < textBox.numLines; i++) {
				metric = textBox.getLineMetrics(i);
				var cont:Sprite = new Sprite;
								
				textBox.parent.addChildAt(cont, textBox.parent.getChildIndex(textBox));
				cont.graphics.beginFill(color,1);
				cont.graphics.drawRect(metric.x+despX, (i*metric.height)+despY, metric.width+8, metric.height-2);
				cont.graphics.endFill();				
			}
			
		}
		
		public function subParrafo2(textBox:TextField,desp:Number,colorCaixa:Number) {
			var metric:TextLineMetrics;
			
			for (var i = 0; i < textBox.numLines; i++) {
				metric = textBox.getLineMetrics(i);
				var cont:Sprite = new Sprite;
				
				cont.x = textBox.x;
				
				textBox.parent.addChildAt(cont, textBox.parent.getChildIndex(textBox));
				cont.graphics.beginFill(colorCaixa,1);
				cont.graphics.drawRect(metric.x-2, (i*metric.height)+1, metric.width+8, metric.height-1);
				cont.graphics.endFill();				
				
				cont.y = textBox.y+4;
			}
			
		}
		
		
	}
	
}