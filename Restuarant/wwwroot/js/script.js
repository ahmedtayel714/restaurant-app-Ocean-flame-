"use strict";

/***************** page Loading *****************/
$(window).on("load", function () {
    $("#loading").fadeOut();
})
$(document).ready(function () {

    /***************** Navbar active class*****************/
    $(".nav-defult .navbar ul").find("li").each(function () {
        var elm = $(this);
        elm.click(function () {
            elm.parent().find("li").removeClass("active");
            elm.addClass("active");

        });
        $("li#reservation").click(function () {
            elm.parent().find("li").removeClass("active");
        });

    });


    /***************** Mobile Navbar *****************/
    $("#responsive-menu-btn").click(function () {
        $("#responsive-menu-btn span.top").toggleClass("span-top-menu");
        $("#responsive-menu-btn span.main").toggleClass("span-main-menu");
        $("#responsive-menu-btn span.bottom").toggleClass("span-bottom-menu");
        $(".nav-mobile").slideToggle();
    });
    // hide navbar when click on link in navbar mobile and go The desired section
    $(".nav-mobile .navbar ul").find("li").each(function () {
        var elm = $(this);
        elm.click(function () {
            $(".nav-mobile").slideUp(10);
            $("#responsive-menu-btn span.top").removeClass("span-top-menu");
            $("#responsive-menu-btn span.main").removeClass("span-main-menu");
            $("#responsive-menu-btn span.bottom").removeClass("span-bottom-menu");
        });
    });


    /***************** Back to top Btn & scroll Event *****************/
    $(window).on("scroll", function () {
        var goTop = $("#back-to-top");
        var navbar = $("nav.nav-defult");
        if (document.body.scrollTop > 100 || document.documentElement.scrollTop > 100) {
            navbar.addClass("navbar-fixed");
            $("nav.nav-mobile").css("top", "50px");
            goTop.fadeIn();
            goTop.click(function (e) {
                e.preventDefault();
                document.body.scrollTop = 0;
                document.documentElement.scrollTop = 0;
            });
        } else {
            goTop.fadeOut();
            navbar.removeClass("navbar-fixed")
            $("nav.nav-mobile").css("top", "107px");
        }
    });


    /***************** Special Menu Btn *****************/
    $("a[data-specialBtn]").each(function () {
        var btn = $(this);
        var tabBox = $("#" + btn.attr("data-specialBtn"));
        btn.click(function (e) {
            e.preventDefault();
            btn.parent().parent().find("li").removeClass("bg-active");
            btn.parent().addClass("bg-active");
            btn.parent().parent().find(".btn-item").css("color", "#fff");
            btn.css("color", "#000")
            btn.parent().parent().parent().parent().parent().find(".special-content").hide();
            tabBox.show();
        })
    });


    /***************** Event Slider *****************/
    var swiper = new Swiper(".mySwiper", {
        spaceBetween: 45,
        // centeredSlides: true,
        pagination: {
            el: ".swiper-pagination",
            clickable: true,
        },
        loop: true,
        autoplay: {
            delay: 2500,
            disableOnInteraction: false,
        },
    });


    /***************** Member Comment Slider *****************/
    var swiper = new Swiper(".mySwiper1", {
        slidesPerView: 3,
        slidesPerGroup: 3,
        loop: true,
        pagination: {
            el: ".swiper-pagination",
            clickable: true,
        },
        autoplay: {
            delay: 2500,
            disableOnInteraction: false,
        },
        breakpoints: {
            200: {
                slidesPerView: 1,
                slidesPerGroup: 1,
            },
            640: {
                slidesPerView: 1,
                slidesPerGroup: 1,
            },
            768: {
                slidesPerView: 2,
                slidesPerGroup: 2,
            },
            1024: {
                slidesPerView: 3,
                slidesPerGroup: 3,
            },
        },
    });

    /***************** Menu Filter and active btn *****************/
    var mixer = mixitup('.box-list')

    $("#menu .menu-filter ul").find("li").each(function () {
        var elm = $(this);
        elm.click(function () {
            elm.parent().find("li").removeClass("active");
            elm.addClass("active");

        })
    });


    /***************** AOS animate plugin *****************/
    AOS.init({
        once: true,
        duration: 1200,
        easing: 'ease-in-out',
    });


    /***************** venoBox Gallery Plugin *****************/
    $('.venobox').venobox({
        framewidth: '800px',
        frameheight: '100%',
    });


    /***************** Form validate : Book Table & contact *****************/
    var emailRegex = /([a-z0-9_\.-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})/;
    var phoneNumberRegex = /[0-9]/;
    var peopleRegex = /^[1-9]{1,2}/;
    var nameRegex = /^[a-z\u0600-\u06ff]{3,18}/;

    $("#contactForm").submit(function (e) {
        var contactName = $("#contactName");
        if (!nameRegex.test(contactName.val())) {
            $("p#contactNameError").html("Please enter a valid name.").show();
        } else {
            $("p#contactNameError").hide();
        }
        var contactEmail = $("#contactEmail");
        if (!emailRegex.test(contactEmail.val())) {
            $("p#contactEmailError").html("Please enter a valid email address.").show();
        } else {
            $("p#contactEmailError").hide();
        }
        var contactSubject = $("#contactSubject");
        var contactText = $("#contactText");
        if (nameRegex.test(contactName.val()) && emailRegex.test(contactEmail.val())) {
            alert("Your message has been sent. \rThank you for your message");
            contactName.val("");
            contactEmail.val("");
            contactSubject.val("");
            contactText.val("");
        } else {
            e.preventDefault();
        }
    });



});

// Table Reservation
const bookForm = document.getElementById('bookTableForm');
bookForm.addEventListener('submit', async (e) => {
    e.preventDefault();

    const name = document.getElementById('bookTableName').value;
    const email = document.getElementById('bookTableEmail').value;
    const phone = document.getElementById('bookTablePhone').value;
    const people = document.getElementById('bookTablePeople').value;

    if (!name || !email || !phone || !people) {
        alert("Please fill in all fields.");
        return;
    }

    const reservation = {
        name: name,
        email: email,
        phone: phone,
        people: parseInt(people),
        message: document.getElementById('bookTableText').value
    };

    console.log('Sending reservation:', reservation);

    try {
        const response = await fetch('/api/reservation/book', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(reservation)
        });

        console.log('Response status:', response.status);

        if (response.ok) {
            const data = await response.json();
            console.log('Success data:', data);
            alert(`✅ ${data.message}`);
            bookForm.reset();
        } else {
            const errorText = await response.text();
            console.error('Error response:', errorText);
            alert("Error booking table: " + errorText);
        }
    } catch (error) {
        console.error('Fetch error:', error);
        alert("Failed to connect to server: " + error.message);
    }
});