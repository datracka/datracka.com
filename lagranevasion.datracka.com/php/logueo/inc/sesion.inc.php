<?
ini_set("session.use_cookies","1");
//ini_set("url_rewriter.tags","");
//ini_set("session.use_trans_sid", false);
//ini_set("session.referer_check","frenaelcambioclimatico");
//ini_set("session.cookie_lifetime","30");
session_name("frenaelcambioclimatico");
session_start();
session_register("sesion_inicio");
session_register("sesion_ultimo");
session_register("sesion_ip");
session_register("sesion_browser");
session_register("sesion_dame_tiempo");

?>