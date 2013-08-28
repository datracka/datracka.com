
var Config = function() {

    this.videoListServiceUrl = "http://aminutetorelax.com/php/getListVideos.php"
}

Config.prototype.getVideoListServiceUrl = function(){
    return  this.videoListServiceUrl;
}
