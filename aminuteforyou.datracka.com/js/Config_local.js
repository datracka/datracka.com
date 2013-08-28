
var Config = function() {

    this.videoListServiceUrl = "http://relax.local/php/getListVideos.php"
}

Config.prototype.getVideoListServiceUrl = function(){
    return  this.videoListServiceUrl;
}
