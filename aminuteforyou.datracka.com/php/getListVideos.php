<?php

include "vimeo/vimeo.php";
include "VimeoApi.php";

$oVimeoApi = new VimeoApi();

$oVideos = $oVimeoApi->getListOfVideos();
echo json_encode($oVideos->videos->video);