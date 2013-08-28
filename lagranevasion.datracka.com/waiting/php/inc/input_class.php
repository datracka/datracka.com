<?php
  
 
class input{
	var $tipo;
	function input(&$tipo){
		$this->tipo = &$tipo;
	}
	function setTypeInput(&$tipo){
		$this->tipo = &$tipo;
	}
	function get($variable){
		if(!isset($this->tipo[$variable])){
			//echo "La variable $variable no esta definida.\n";
		} elseif ($this->tipo[$variable]=="" || $this->tipo[$variable]==" "){
			//echo "La variable $variable esta vacia.\n";
		} else{
			//echo "La variable $variable vale ".$this->tipo[$variable];
		}
		//$varISO = $this->tipo[$variable];
		$varISO = mb_convert_encoding($this->tipo[$variable], "ISO-8859-1","UTF-8");
		$return = get_magic_quotes_gpc() ? $varISO : addslashes($varISO);
		$return = $this->limpia_cadena_xml($return);
		return ($return);   
	}
	function limpia_cadena_xml($c){
		$c=trim(ereg_replace("&","&amp;", $c));
		$c=trim(ereg_replace("\"","&quot;", $c));
		return $c;
	}	
}

?>