/**
 * Created with JetBrains PhpStorm.
 * User: vfayos
 * Date: 01.02.13
 * Time: 16:58
 * To change this template use File | Settings | File Templates.
 */

/**
 * Video manager
 */

var Video = function () {

    this.Config = new Config();

}

/**
 * Get the random video to show on the screen
 *
 * @param videos
 */
Video.prototype.getVideoUrl = function (videos) {

    var v = null;
    var t = null;
    var rn = null;
    var ov = null;
    var av = [];

    //iterate all the videos json
    $.each(JSON.parse(videos),function(i,e){
        av.push(e.id);
    })


    //get a random video from the channel.
    var aLength = av.length;
    v = "22439234"; //default video
    if(av.length >0){
        rn = Math.floor(Math.random()*aLength);
        v = av[rn];
    }

    //set and return finakl url
    var videoUrl = 'http://www.vimeo.com/' + v;

    ov = {
        videoUrl: videoUrl,
        videoTiming: t
    }

    //Request video
    Video.prototype.getVideo(ov);
};

/**
 * Request Selected video
 *
 * Callback embedVideo
 *
 */
Video.prototype.getVideo =  function (oVideo) {

    var url = "http://www.vimeo.com/api/oembed.json" + '?url=' + encodeURIComponent("http://www.vimeo.com/" + oVideo.videoUrl) +
        '&callback=' + "Video.prototype.embedVideo" +
        '&width=' + (window.innerWidth) +
        '&height=' + (window.innerHeight) +
        '&autoplay=1' +
        '&t=' + oVideo.videoTiming;

    var js = document.createElement('script');
    js.setAttribute('type', 'text/javascript');
    js.setAttribute('src', url);
    document.getElementsByTagName('head').item(0).appendChild(js);

};

/**
 * gather list of videos of selected channel
 *
 * Callback getVideo
 *
 */
Video.prototype.loadVideosChannel =  function () {

    $.getScript(this.Config.getVideoListServiceUrl(), function (data, textStatus, jqxhr){
        Video.prototype.getVideoUrl(data);
    });

}

Video.prototype.embedVideo = function (video){
    document.getElementById('embed').innerHTML = decodeURI(video.html);
}







