//profildən çıxış barina klikləmə
function profmenu() {
    $(".profMenu").toggle()
}
$("section").click(function () {
    $(".profMenu").hide()
})
//profil dostluq gonder,legv et
$(".gonder").click(function () {
    $(this).hide()
    $(".legv").show()
})
$(".legv").click(function () {
    $(this).hide()
    $(".gonder").show()
})
// şifrəni unutmuşam səhifəsində divlərin ardıcıllığı

// $(".mail").click(function(){
//     $(".forget").hide()
//     $(".kod").show()
// })



// Responsive bars panel
$(".resBars").click(function () {
    $(".resNav").toggleClass("navRes")
})
$(".axtar").click(function () {
    $(".axtaris form input")
})

// Kateqoriya

$(function () {
    $(".cat-filter li").click(function () {
        console.log()
        $(".all").hide();
        $("." + $(this).attr("cat")).show()
    })
})
//SLide
$(".carousel-item").eq(0).addClass("active")

//paylaşımlar hissəsində əlavə olunmuş şəkillərin silinməsi

$("#MixPhotoSelect").change(function () {
    console.log(this.files)
    let item = document.createElement('div')
    item.className = 'item'
    let img = document.createElement('img')
    img.src = URL.createObjectURL(this.files[0])
    item.appendChild(img)
    item.appendChild(this.cloneNode(true))
    this.value = null
    item.addEventListener('click', function () {
        this.remove()
    })
    document.querySelector(".photoarea").appendChild(item)
})
$("#ProfilPostPhotoUrl").change(function () {
    console.log(this.files)
    let item = document.createElement('div')
    item.className = 'item'
    let img = document.createElement('img')
    img.src = URL.createObjectURL(this.files[0])
    item.appendChild(img)
    item.appendChild(this.cloneNode(true))
    this.value = null
    item.addEventListener('click', function () {
        this.remove()
    })
    document.querySelector(".photoarea").appendChild(item)
})

window.onload = function () {
    document.querySelectorAll(".photoarea .item").forEach(function (x) {
        x.addEventListener('click', function () {
            this.remove()
            $.post('/Admin/RemoveImg/' + this.getAttribute('attrid'), function () {

            })
        })
    })
}
//small img
$('.postImg img').click(function () {
    $(this).parent().prev().attr('src', $(this).attr('src'))
})
//typr text and password
$("#bagliEnter").click(function () {
    $(this).hide()
    $("#aciqEnter").show()
    $("#password").attr("type", "text")
})
$("#aciqEnter").click(function () {
    $(this).hide()
    $("#bagliEnter").show()
    $("#password").attr("type", "password")
})
$("#bagli").click(function () {
    $(this).hide()
    $("#aciq").show()
    $("#parol").attr("type", "text")
})
$("#aciq").click(function () {
    $(this).hide()
    $("#bagli").show()
    $("#parol").attr("type", "password")
})

/

$(document).ready(function () {
    $('.comment').on('click', '.LikeNumber', function () {
        const icn = $(this)
        $.post('/Home/PostLike/' + $(this).attr('LikePostId'), function (res) {
            if ($(icn).find('i').hasClass('fa-solid')) {
                $(icn).find('span').text($(icn).text() - 1)
            } else {
                $(icn).find('span').text(Number($(icn).text()) + 1)
            }
            $(icn).find('i').toggleClass('text-danger').toggleClass('fa-solid')
        })
        console.log(icn)
    })
})

$(document).ready(function () {
    $('.solHisse').on('click', '.LikeNumber', function () {
        const icn = $(this)
        $.post('/Home/PostLike/' + $(this).attr('LikePostId'), function (res) {
            if ($(icn).find('i').hasClass('fa-solid')) {
                $(icn).find('span').text($(icn).text() - 1)
            } else {
                $(icn).find('span').text(Number($(icn).text()) + 1)
            }
            $(icn).find('i').toggleClass('text-danger').toggleClass('fa-solid')
        })
        console.log(icn)
    })

})

$(document).ready(function () {
    $('.solHisse').on('click', '.ProfilLikeNumber', function () {
        const icn = $(this)
        $.post('/Home/ProfilPostLike/' + $(this).attr('ProfilLikePostId'), function (res) {
            if ($(icn).find('i').hasClass('fa-solid')) {
                $(icn).find('span').text($(icn).text() - 1)
            } else {
                $(icn).find('span').text(Number($(icn).text()) + 1)
            }
            $(icn).find('i').toggleClass('text-danger').toggleClass('fa-solid')
        })
        console.log(icn)
    })

})

$("#formComment").submit(function (e) {
    e.preventDefault()
    /*    const a = $(this).parents(".posts")*/
    const post = $(this).parents(".xeberImg").find(".comment")
    let formData = new FormData(this)
    $.ajax({
        type: "POST",
        url: e.target.action,
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        success: (res) => {
            console.log(res)
            this.reset()
            $(post).append(
                ` 
                <div class="allComment mt-3">
                  <div class="userComment">
                                <div class="userPhoto">
                                    <a href="/Home/Profil/@sherh.SherhUser?UserNickName=${$('#profileimage').attr('alt')}">
                                        <img src="${$('#profileimage').attr('src')}" class="userimg">
                                    </a>
                                </div>
                                <div class="user-comment">
                                    <a href="/home/profil/@sherh.SherhUser?UserNickName=${$('#profileimage').attr('alt')}" style="color:black"> <h6>${$('#profileimage').attr('alt')}</h6> </a>
                                    <p>${res.sherhDescription}</p>
                                </div>
                                <div class="del"><a href="/Home/DeleteComment/@item.SherhId"><i class="fas fa-times"></i></a></div>
                 </div>  
                </div>  
                `
            )
        },
        error: function () {

        }
    })
})


$("#profilformcomment").submit(function (e) {
    e.preventDefault()
    /*    const a = $(this).parents(".posts")*/
    const post = $(this).parents(".profil").find(".profBody .comment")
    let formData = new FormData(this)
    $.ajax({
        type: "POST",
        url: e.target.action,
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        success: (res) => {
            console.log(res)    
            this.reset()
            $(post).append(
                ` 
                <div class="allComment mt-3">
                  <div class="userComment">
                                <div class="userPhoto">
                                    <a href="/Home/Profil/@comment.CommentUser?UserNickName=${$('#profileimage').attr('alt')}">
                                        <img src="${$('#profileimage').attr('src')}" class="userimg">
                                    </a>
                                </div>
                                <div class="user-comment">
                                    <a href="/home/profil/@comment.CommentUser?UserNickName=${$('#profileimage').attr('alt')}" style="color:black"> <h6>${$('#profileimage').attr('alt')}</h6> </a>
                                    <p>${res.commentDescription}</p>
                                </div>
                                <div class="del"><a href="/Home/DeleteProfilComment/@comment.CommentId"><i class="fas fa-times"></i></a></div>
                 </div>  
                </div>  
                `
            )
        },
        error: function () {

        }
    })
})

$(document).ready(function () {
    $('.xeberImg .allComment .userComment').on('click', '.del', function () {
        const btn = $(this)
        $.post('/Home/DeleteComment/' + $(this).attr('sherhid'), function (res) {
            $(btn).parents('.allComment').remove();
        })
    })

})

$(document).ready(function () {
    $('.profil .allComment .userComment').on('click', '.del', function () {
        const btn = $(this)
        $.post('/Home/DeleteProfilComment/' + $(this).attr('commentid'), function (res) {
            $(btn).parents('.allComment').remove();
        })
    })

})

$(".searchfriend input").keyup(function () {
    $(".searchinputdata").empty()
    if ($(this).val() != "") {
        $.post("/Home/Search?q=" + $(this).val(), function (res) {
            $(res).each(function () {
                $(".searchinputdata").append(
                    `
                     <div class="searchphoto my-2">
                        <img src="/img/${this.userProfilePhotoUrl}" />
                        <a href="/home/profil?UserNickName=${this.userNickName}" class="my-2">${this.userNickName}</a>
                    </div>
                `)
            })
        })
    }
})