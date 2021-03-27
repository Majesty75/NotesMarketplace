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
            .attr('data-tr-per-page','6');
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
        
        if (!target.is(':visible')) {
            //hide all other contex menus
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
            $(this).find('button img').attr('src', '/Content/imgs/icons/minus.png');
            ancestor.removeClass('card-header');
        } else {
            $(ancestor).find('.card-heading h3').css('font-weight','400');
            $(this).find('button img').attr('src', '/Content/imgs/icons/add.png');
            ancestor.addClass('card-header');
        }
        
    });
    
});



/*  =====================================
                Pagination
    ===================================== */

function paginateView() {

    $('.pagination-control').each(function () {

        //paginator element
        var pageControl = $(this);

        //element to be paginated
        var paginate = $(pageControl.attr('pagination-data'));

        //no of element shown per page
        var elementsShown = parseInt(paginate.attr('data-tr-per-page'), 10);

        //no of total elements
        var totalElements = paginate.children();

        //no of total pages
        var numPages = Math.ceil(totalElements.length / elementsShown);

        //Max page shown in paginator (incomplete function  to show limited pages link in paginator)
        var maxPage = parseInt(pageControl.attr('max-page-shown'));

        totalElements.hide();

        // add links to paginator for all pages
        for (i = numPages; i > 0; i--) {

            //max page (incomplte function)
            if (i > maxPage) {
                pageControl.find('nav ul>li:first').after('<li style="display:none;" class="page-item"><a class="page-no page-link" page="' + i + '">' + i + '</a></li>');
            } else {

                pageControl.find('nav ul>li:first').after('<li class="page-item"><a class="page-no page-link" page="' + i + '">' + i + '</a></li>');
            }
        }

        //click function to show page and hide current page
        pageControl.find('.page-no').on('click', function () {

            //active class change 
            pageControl.find('nav ul>li a').removeClass('active');
            $(this).addClass('active');

            var page = parseInt($(this).attr('page'));
            totalElements.hide();

            var showStart = (page - 1) * elementsShown;

            //calculate and show next max element per page
            totalElements.slice(showStart, ((showStart + elementsShown) < totalElements.length) ? (showStart + elementsShown) : totalElements.length).fadeIn('slow');
        });


        //trigger click on first page to show initial page
        pageControl.find('.page-no:first').trigger('click');

        //nprevious page trigger click on previos page link
        pageControl.find('nav ul>li:first').on('click', function () {

            pageControl.find('nav ul>li a.active').parent().prev().find('a.page-no').trigger('click');

        });

        //next page trigger click on next page link
        pageControl.find('nav ul>li:last').on('click', function () {

            pageControl.find('nav ul>li a.active').parent().next().find('a.page-no').trigger('click');

        });
    });
}

$(function () {
    paginateView();
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

            if (tableBody.find('.no-results').length > 0)
                return

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
                //var colSpan = tableBody.find('tr:first').children().length;
                var colSpan = 100;
                tableBody.append('<tr class="no-results"><td class="text-center" colspan="'+colSpan+'">No Records Found</td></tr>');
                setTimeout(function () {

                    if (tableBody.children.length > 1) {
                        tableBody.find('.no-results').remove();

                        //trigger click on first page to show initial page
                        $('.pagination-control[pagination-data="'+ tableBodyString + '"]').find('.page-no:first').trigger('click');
                    }

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
                $('.menu-btn').find('img').attr('src', '/Content/imgs/icons/bars-solid-rblue.png');
                $('body').find('#overlay').remove();
            } else {
                $('.menu-btn').find('img').attr('src', '/Content/imgs/icons/times-solid-rblue.png');
                //$('body').append('<div id="overlay"></div>');
            }
            $('#mobile-navigation-wrapper').slideToggle();
            $('body').toggleClass('disable-scroll');
            $('#mobile-navigation-wrapper').find('.dropdown-menu').fadeOut();
        });
    }
});

/*  =====================================
            Ajax Request For Notes
    ===================================== */
//only submit form for valid keys
var validKeys = new RegExp("[a-zA-Z0-9\b\r]{1}");

$(function () {
    $('#filter-notes').on('change', "select", function () {
        submitForm();
    });

    $('#filter-notes input[type="text"]').on('keyup', function () {
        if (validKeys.test(event.key)) {
            submitForm();
        }
    });
});

function submitForm() {
    $('#search-filter-notes').submit();
}

function showError() {
    alert("Something went wrong!!, please try different parameters");
}


/*  =====================================
  Change Profile Picture when user selects new one
    ===================================== */

$(function () {

    $('input[data-change]').change(event, function () {
        var inputEle = $(this);
        if (this.files && this.files[0]) {
            //if it's image load new picture to show on UI
            if (inputEle.attr('data-isimage') != null) {

                //console.log(this.files[0]);
                if (this.files[0]['type'].includes('image')) {

                    var reader = new FileReader();

                    reader.onload = function (e) {
                        $(inputEle.attr('data-change')).attr('src', e.target.result);
                    }
                    reader.readAsDataURL(this.files[0]);
                    $(inputEle.attr('data-change')).next().text(this.files[0].name);
                    //console.log($(inputEle.attr('data-change')).next(), this.files[0].name);
                }
                else {
                    $(inputEle.attr('data-change')).next().text("Invalid File Type");
                }
            }

            //else if it's note attachment show file names
            else {
                //show count or file name in this span ele
                var spanEle = $(inputEle.attr('data-change'));
                if (this.files[0]['type'] == "application/pdf") {
                    //if only one file show it's name
                    if (this.files.length == 1) {
                        spanEle.text(this.files[0].name);
                    }

                    //if multiple files show count
                    else {
                        spanEle.text(this.files.length + " files selected.");
                    }
                }
                else {
                    spanEle.text("Invalid File Type");
                }
            }
        }
    }); 

});