<?php
/**
 * Created by JetBrains PhpStorm.
 * User: vfayos
 * Date: 27.03.13
 * Time: 13:41
 * To change this template use File | Settings | File Templates.
 */


class VimeoApi extends phpVimeo{

    protected $ck = "11ea28e89d369a475e41c255278f5a9ae47c61a4";
    protected $cs = "031045fc9da4d57d405f51ee63ca5761ae8c7264";
    protected $rt = "10bbc5bbfc5005ca32a5a95a0403b41f";
    protected $ts = "222efa6b3c8621dc201a3a1333f531293d866a3c";

    protected $userId = "user17237997";
    protected $channelId = "497601";

    /**
     * Construct with key and tokens given
     *
     */
    public function __construct()
    {
        parent::__construct($this->ck, $this->cs, $this->rt, $this->ts);
        //parent::enableCache(phpVimeo::CACHE_FILE, './cache', 3000);
    }

    public function getListOfVideos()
    {

        $videos = $this->call('vimeo.channels.getVideos',
            array(
                'channel_id' => $this->channelId,
                'user_id' => $this->userId)
        );

        return $videos;

    }
}