var Main = (function(){

    //Constructor
    function Main(){

        // Get new library object for storing elements
        Main.library = new Storage();

        Main.prototype.fetchElements();
        Main.prototype.initializeEventListeners();

        Main.video = new Video();
        Main.video.loadVideosChannel();

        $("div.loading:nth-child(1)").center();


    }
    //atrributes
    Main.library = "";
    Main.video = null;
    Main.urlVars = "";

    Main.prototype = {

        fetchElements: function(){
            var hidedHeader     = $('#hidedHeader')
            var html            = $('html');

            Main.library.set('hidedHeader', hidedHeader);
            Main.library.set('html', html);
        },

        initializeEventListeners: function () {

            /** get Vars from URL */
            Main.urlVars = this.getUrlVars();

            /** document events **/
            var html = Main.library.get('html');
            var hidedHeader = Main.library.get('hidedHeader');

            html.mouseover(Main.onDocument);

            /** show iframe **/
            $(".wrapper").fadeIn(2000);

            /** set close button animation */
            $("#closeHeader").off().animate(
                {"top": "70px"},
                "slow",
                function(){
                    $("#closeHeader").off().delay(3000).animate(
                        {"top": "-70px"},
                        "slow",
                        function(){
                            //enable events

                            hidedHeader.mouseover(Main.prototype.showCloseHeader);
                            hidedHeader.mouseout(Main.prototype.hideCloseHeader);
                            hidedHeader.click(Main.prototype.closewindow)

                        }
                    );
                }
            );
        },

        showCloseHeader: function () {
            $("#closeHeader").off().animate({"top": "+70px"}, "slow");
        },
        hideCloseHeader: function () {
            $("#closeHeader").off().animate({"top": "-70px"}, "slow");
        },

        closewindow: function(){

            var d = Main.urlVars["a"];
            var p = Main.urlVars["p"];
            var url = p + "//" + d;

            var win = parent;
            win.postMessage("destroy_bookmarklet",url);

        },

        onDocument: function (event){
             alert("aa");
        },

        getUrlVars: function(){
            var vars = {};
            var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function(m,key,value) {
                vars[key] = value;
            });

           return vars;
        }

}
    return Main;
}());



