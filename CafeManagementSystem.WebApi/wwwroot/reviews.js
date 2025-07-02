
//$(document).ready(function () {
//    const cafeId = $('#cafeId').val();

//    // Yorumları yükle
//    loadReviews(cafeId);

//    // Yorum formu submit
//    $('#reviewForm').submit(function (e) {
//        e.preventDefault();

//        const rating = $('#rating').val();
//        const comment = $('#comment').val();

//        $.ajax({
//            url: '/api/Reviews',
//            type: 'POST',
//            contentType: 'application/json',
//            data: JSON.stringify({
//                CafeId: cafeId,
//                Rating: rating,
//                Comment: comment
//            }),
//            success: function (review) {
//                // Yeni yorumu listeye ekle
//                prependReview(review);
//                // Formu temizle
//                $('#reviewForm')[0].reset();
//                $('#rating').val('5'); // Rating'i tekrar 5 yap
//            },
//            error: function (xhr) {
//                if (xhr.status === 401) {
//                    alert('Yorum yapmak için giriş yapmalısınız!');
//                } else {
//                    alert('Bir hata oluştu: ' + xhr.responseText);
//                }
//            }
//        });
//    });
//});

//function loadReviews(cafeId) {
//    $.ajax({
//        url: `/api/Reviews/${cafeId}`,
//        type: 'GET',
//        success: function (reviews) {
//            $('#reviewsContainer').empty();
//            reviews.forEach(review => {
//                $('#reviewsContainer').append(createReviewHtml(review));
//            });
//        },
//        error: function (xhr) {
//            console.error('Yorumlar yüklenirken hata oluştu:', xhr.responseText);
//        }
//    });
//}

//function prependReview(review) {
//    $('#reviewsContainer').prepend(createReviewHtml(review));
//}

//function createReviewHtml(review) {
//    return `
//        <div class="review-item">
//            <div class="review-header">
//                <strong>${review.user.userName}</strong>
//                <div class="rating">${'★'.repeat(review.rating)}${'☆'.repeat(5 - review.rating)}</div>
//                <small>${new Date(review.createdDate).toLocaleString()}</small>
//            </div>
//            <div class="review-comment">${review.comment}</div>
//        </div>
//    `;
//}