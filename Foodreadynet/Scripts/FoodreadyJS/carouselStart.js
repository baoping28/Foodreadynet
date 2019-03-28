jQuery(document).ready(function ($) {
    var options = {
        $AutoPlay: true,                                  
        $AutoPlaySteps: 3,                         
        $AutoPlayInterval: 4000,                  
        $PauseOnHover: 3,                       

        $ArrowKeyNavigation: true,   
        $SlideDuration: 300,                             
        $MinDragOffsetToSlide: 20,                        
        $SlideWidth: 200,                                
        $SlideHeight: 100,                             
        $SlideSpacing: 34, 					             
        $DisplayPieces: 4,          
        $ParkingPosition: 0,      
        $UISearchMode: 1,                
        $PlayOrientation: 1,                             
        $DragOrientation: 1,                             

        $BulletNavigatorOptions: {                             
            $Class: $JssorBulletNavigator$,                    
            $ChanceToShow: 2,                            
            $AutoCenter: 0,                              
            $Steps: 1,                                 
            $Lanes: 1,                                
            $SpacingX: 0,                               
            $SpacingY: 0,                                
            $Orientation: 1                             
        },

        $ArrowNavigatorOptions: {
            $Class: $JssorArrowNavigator$,           
            $ChanceToShow: 1,                           
            $AutoCenter: 2,                            
            $Steps: 2                               
        }
    };

    var jssor_slider1 = new $JssorSlider$("slider1_container", options);

    //responsive code begin
    //you can remove responsive code if you don't want the slider scales while window resizes
    function ScaleSlider() {
        var bodyWidth = document.body.clientWidth;
        if (bodyWidth)
            jssor_slider1.$ScaleWidth(Math.min(bodyWidth, 1160));
        else
            window.setTimeout(ScaleSlider, 30);
    }
    ScaleSlider();

    $(window).bind("load", ScaleSlider);
    $(window).bind("resize", ScaleSlider);
    $(window).bind("orientationchange", ScaleSlider);
    //responsive code end
});