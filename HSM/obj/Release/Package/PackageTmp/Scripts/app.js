var App = function () 
{

    var iFrameHeight = function  () 
    {
        var headerHeight = $(".navbar").outerHeight();
        $("#iframe").height(($(window).height() - headerHeight));
    }

    var iFrameLoad = function ()
    {
        if (document.domain == 'live.mosaicpro.biz') {
            document.domain = "mosaicpro.biz";
        }

        $('#iframe').on('load', function()
        {
            $("#toggle-template-options").removeClass('hide').on('click', function(e){
                e.preventDefault();
                $("#iframe").contents().find("#template-options").collapse("toggle");
            });
            // $("#iframe").contents().find("#template-options").collapse("toggle");
            $("#iframe").contents().find("[target]").removeAttr("target");
        });
    }

    var handleEnvatoFrame = function()
    {
        if (window.location != window.parent.location)
            top.location.href = document.location.href;
    }
    
    var windowResize = function () 
    {
        $(window)
        .resize(function(){
            if (typeof Modernizr != 'undefined' && !Modernizr.touch)
                iFrameHeight();
        })
        .load(function(){
            iFrameHeight();
        });
    }

    return {

        init: function () 
        {
            handleEnvatoFrame();
            iFrameLoad();

            if (typeof Modernizr != 'undefined' && !Modernizr.touch)
                $('[data-toggle="tooltip"]').tooltip();
        }
        
    };

}();

(function($)
{
    App.init();
})(jQuery);