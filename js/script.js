/*  ===========================
    Toggle password visibility
    =========================== */

$(function (){
    
    $('.show-pass').on('click', function() {
        var currentState = $(this).find('img').attr('toggle');
        
        if($(currentState).attr('type') == "password"){
            $(currentState).attr('type','text');
        } else{
            $(currentState).attr('type','password');
        }
        
    });
    
});

/*  =====================================
    Change Rows per page in smaller device 
    ===================================== */

$(function(){
    if($(window).height() < 991) {
        $('#sold-notes-table tbody, #rejected-notes-table tbody, #buyer-requests-table tbody')
            .attr('data-tr-per-page','5');
        
        $('#results-row')
            .attr('data-tr-per-page','3');
    }
});


/*  =====================================
    Show user profile picture contex menu
    and dots icon contex menu in table.
    ===================================== */

$(function() {
   
    $('.table, .site-header').on('click', '.dots-menu-button, .contex-menu-toggle',function(){
        
        //get target attribute and toggle display css
        var target = $(this).find('.dropdown-menu');
        
        if(!target.is(':visible')){
            //hide all other contex menus
            console.log('I hate ASP.NET');
            $('.dropdown-menu').slideUp();
        }
        target.slideToggle();
        
    });
    
});


/* ----------------------------- FAQs page ------------------------- */

/*  =====================================
    Toggle + sign and class of card onclick
    ===================================== */

$(function() {
    
    $('#faqs .card-heading').on('click', function() {
        
        //find ancestor with card class for clicked button
        var ancestor = $(this).parents('.card');
        
        //find if ancestor was collapsed or not via card holder class because we toggle it on alternate click 
        if(ancestor.hasClass('card-header')){
            $(ancestor).find('.card-heading h3').css('font-weight','600');
            $(this).find('button img').attr('src', './imgs/icons/minus.png');
            ancestor.removeClass('card-header');
        } else {
            $(ancestor).find('.card-heading h3').css('font-weight','400');
            $(this).find('button img').attr('src', './imgs/icons/add.png');
            ancestor.addClass('card-header');
        }
        
    });
    
});



/*  =====================================
                Pagination
    ===================================== */

$(function() {
    if($('.pagination-control').length) {
        $('.pagination-control').each(function() {
            
            //paginator element
            var pageControl = $(this);

            //element to be paginated
            var paginate = $(pageControl.attr('pagination-data'));

            //no of element shown per page
            var elementsShown = parseInt(paginate.attr('data-tr-per-page'),10);

            //no of total elements
            var totalElements = paginate.children();

            //no of total pages
            var numPages = Math.ceil(totalElements.length/elementsShown); 

            //Max page shown in paginator (incomplete function  to show limited pages link in paginator)
            var maxPage = parseInt(pageControl.attr('max-page-shown'));

            totalElements.hide();

            // add links to paginator for all pages
            for(i=numPages;i>0;i--){

                //max page (incomplte function)
                if(i>maxPage){
                    pageControl.find('nav ul>li:first').after('<li style="display:none;" class="page-item"><a class="page-no page-link" page="'+i+'">'+i+'</a></li>');
                } else {

                    pageControl.find('nav ul>li:first').after('<li class="page-item"><a class="page-no page-link" page="'+i+'">'+i+'</a></li>');
                }
            }

            //click function to show page and hide current page
            pageControl.find('.page-no').on('click',function() {

                //active class change 
                pageControl.find('nav ul>li a').removeClass('active');
                $(this).addClass('active');

                var page = parseInt($(this).attr('page'));
                totalElements.hide(); 

                var showStart = (page-1)*elementsShown;

                //calculate and show next max element per page
                totalElements.slice(showStart,((showStart+elementsShown)<totalElements.length) ? (showStart+elementsShown) : totalElements.length).fadeIn('slow');
            });


            //trigger click on first page to show initial page
            pageControl.find('.page-no:first').trigger('click');

            //nprevious page trigger click on previos page link
            pageControl.find('nav ul>li:first').on('click',function() {

                pageControl.find('nav ul>li a.active').parent().prev().find('a.page-no').trigger('click');

            });

            //next page trigger click on next page link
            pageControl.find('nav ul>li:last').on('click',function() {

                pageControl.find('nav ul>li a.active').parent().next().find('a.page-no').trigger('click');

            });
        });
    }
});


/*  =====================================
    Search in Table
    ===================================== */

$(function() {
        $('.search-in-table').each(function(){

        //table to be searched
        var tableBody = $($(this).attr('data-table-id')+' tbody');    
        var tableBodyString = $(this).attr('data-table-id')+' tbody'; 

        $(this).on('click', function() {

            var resultsFound = false;
            //search text
            var text = $($(this).attr('data-search-input')).val();
            if(!text.trim()){
                return
            }

            text = text.toLowerCase();
            var doesItContainText;

            //search tbody
            tableBody.find('tr').filter(function(){

                doesItContainText = $(this).text().toLowerCase().indexOf(text) > -1;
                $(this).toggle(doesItContainText);
                if(doesItContainText) {
                    resultsFound = true;
                }

            });

            if(!resultsFound) {
                var colSpan = tableBody.find('tr:first').children().length;
                tableBody.append('<tr class="no-results"><td class="text-center" colspan="'+colSpan+'">No Record Found</td></tr>');
                setTimeout(function() {

                    tableBody.find('.no-results').remove();

                    //trigger click on first page to show initial page
                    $('.pagination-control[pagination-data="'+tableBodyString+'"]').find('.page-no:first').trigger('click');

                },3000);
            }

        });

    });
});

/*  =====================================
    Initialize table sorter
    ===================================== */

$(function() {
    
    //initialize table sorter and sort on descending 1st column
    $('.tablesorter').each(function() {
        
        //disable sort on action column
        $(".tablesorter thead tr th:last-child").data("sorter", false);
        
        //get default sort type
        var sortType = $(this).attr('data-sort-on-col-and-order').split(',');
        var sortArr = sortType.map(Number);
        
        //sort on default sort type
        $(this).tablesorter({ 
            
            sortList: [sortArr],
            
            dateFormat : "ddmmyyyy"
            
        });
        
    });

});


/*  =====================================
    Modal - Set Note ID For Popup Action
    ===================================== */

$(function() {
   
    //show modal on click on reject button on notes under review section
    $('.table').on('click', '.btn-danger', function() {
        
        var button = $(event.target);
        var note = button.parents('tr');
        console.log(note);
        var noteIDInput = $($('#reject-popup').attr('data-noteid-element'));
        noteIDInput.val(note.attr('id'));
        $('#reject-popup').find('.horizontal-heading h4').text($(note).children().eq(1).text()+' - '+$(note).children().eq(2).text());
        $('#reject-popup').modal('show');
        
    });
    
    //get noteID from tr parent element of event target element and set it for modal
    $('#review-popup').on('show.bs.modal', function(event) {
        
        var button = $(event.relatedTarget);
        var noteID = button.parents('tr').attr('id');
        var noteIDInput = $($(event.target).attr('data-noteid-element'));
        noteIDInput.val(noteID);
        
        
    });
    
});

/*  =====================================
            Mobile Menu Toggle Button
    ===================================== */
$(function() {
    
    //set max height for mobile menu according to viewport
    if($(window).height() < ( $('#mobile-navigation-wrapper').height() + $('.site-header').height()) ){
        //console.log($('#mobile-navigation-wrapper').height(),$('.site-header').height(), $(window).height());
        
        $('#mobile-navigation-wrapper').css('max-height', $(window).height()-$('.site-header').height()-2);
        $('#mobile-navigation-wrapper').css('overflow-y','scroll');
        
    }
    
    if(!$('#home-page').length){
        $('.menu-btn').on('click',function() {
            if($('#mobile-navigation-wrapper').is(':visible')){
                $('.menu-btn').find('img').attr('src', './imgs/icons/bars-solid-rblue.png');
                $('body').find('#overlay').remove();
            } else {
                $('.menu-btn').find('img').attr('src', './imgs/icons/times-solid-rblue.png');
                $('body').append('<div id="overlay"></div>');
            }
            $('#mobile-navigation-wrapper').slideToggle();
            $('body').toggleClass('disable-scroll');
            $('#mobile-navigation-wrapper').find('.dropdown-menu').fadeOut();
        });
    }
});

/*  =====================================
            Rating Star Select
    ===================================== */
//$(function(){
//    
//    function hoverFun(ele){
//        ele.on('mouseenter',function(){
//                
//                $(this).css('content','url("../imgs/icons/star.png")');
//                $(this).prevAll().css('content','url("../imgs/icons/star.png")');
//                $(this).nextAll().css('content','url("../imgs/icons/star-white.png")');
//                
//        });
//        ele.on('mouseout',function() {
//                
//                $(this).css('content','url("../imgs/icons/star-white.png")');
//                $(this).prevAll().css('content','url("../imgs/icons/star-white.png")');
//                
//        });
//    }
//   
//    $('#review-star-select').each(function(){
//        
//        $(this).find('span.star,span.empty-star').each(function() {
//            
//            hoverFun($(this));
//            
//            $(this).on('click', function(){
//                $(this).css('content','url("../imgs/icons/star.png")');
//                $(this).prevAll().css('content','url("../imgs/icons/star.png")');
//                $(this).off('mouseenter mouseleave');
//                $(this).nextAll().css('content','url("../imgs/icons/star-white.png")');
//                $(this).siblings().each(function(){hoverFun($(this))});
//            });
//            
//        });
//        
//    });
//    
//});

// max page change function (incompte)
//            var startPage = parseInt($(this).attr('start-page'));
//            $(this).attr('start-page',startPage+1);    
//            pageControl.find('a[page="'+startPage+'"]').parent().hide();
//            pageControl.find('a[page="'+(startPage+maxPage)+'"]').parent().show();