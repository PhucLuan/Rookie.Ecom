// Write your JavaScript code.
$(".heart.fa").click(function () {
    var btnLike = this;
    $.ajax({
        url: "Home/LikeProduct",
        type: 'POST',
        //dataType: "json",
        data: { productid: btnLike.id },
        success: function (result) {
            $(btnLike).toggleClass("fa-heart fa-heart-o");
        },
        error: function () {
            alert("Bạn chưa đăng nhập!!!");
        }
    });
});
var productviewid;
$('.link-product-add-cart').click(function () {
    productviewid = this.id;
})
$('#modal-popUP').on('shown.bs.modal', function (e) {
    // do something...
    $.ajax({
        url: "/Home/QuickViewProduct",
        dataType: "html",
        data: { productId: productviewid },
        success: function (result) {
            $('#my-modal-quickview').empty();
            $('#my-modal-quickview').append(result);
        }
    });
});
var productaddcardid;

$('.btn-addcart').click(function () {
    productaddcardid = this.id;
})

$('#modal-addtocart').on('shown.bs.modal', function (e) {
    // do something...
    console.log(productaddcardid + "of " + "shown.bs.modal")
    $.ajax({
        url: "AddToCart",
        type: "get",
        dataType: "html",
        data: { product: productaddcardid },
        success: function (result) {
            $('#my-modal-cart').empty();
            $('#my-modal-cart').append(result);
        }
    });
});

//Get product comment
function checkVisible(elm) {
    if (elm != null) {
        var rect = elm.getBoundingClientRect();
        var viewHeight = Math.max(document.documentElement.clientHeight, window.innerHeight);
        return !(rect.bottom < 0 || rect.top - viewHeight >= 0);
    }
    else
        return false;
}

var isrender = false;

var commentbox = document.getElementById("commentbox");

window.addEventListener("scroll",
    () => {
        if (checkVisible(commentbox) && isrender === false) {

            var productid = document.getElementById("productid");
            isrender = true;
            $.ajax({
                url: "GetComment",
                type: "get",
                dataType: "html",
                data: { productID: productid.value },
                success: function (result) {
                    console.log(result)
                    $('#commentbox').empty();
                    $('#commentbox').append(result)
                }
            });

            console.log("Do some thing");
        }
    }
)
//Get product related

var isrenderproductrelate = false;

var productrelatedbox = document.getElementById("productrelated");

window.addEventListener("scroll",
    () => {
        if (checkVisible(productrelatedbox) && isrenderproductrelate === false) {

            var productid = document.getElementById("productid");
            isrenderproductrelate = true;
            $.ajax({
                url: "GetProductRelated",
                type: "get",
                dataType: "html",
                data: { productID: productid.value },
                success: function (result) {
                    console.log(result)
                    $('#productrelated').empty();
                    $('#productrelated').append(result)
                }
            });

            console.log("Do some thing");
        }
    }
)

