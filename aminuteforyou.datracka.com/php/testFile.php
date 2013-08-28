<?php
/**
 * Created by JetBrains PhpStorm.
 * User: vfayos
 * Date: 27.03.13
 * Time: 14:31
 * To change this template use File | Settings | File Templates.
 */

// autoload function

@set_time_limit(0);
//error_reporting(0);

// SET SOCKET TIMEOUT

ini_set('default_socket_timeout', 10000000);

include "vimeo/vimeo.php";
include "Videos.php";

$a = new VimeoAPI();

$b = $a->getListOfVideos();

