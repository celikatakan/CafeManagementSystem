$(document).ready(function() {
    const cafeId = $('#cafeId').val();
    
    // Sayfa yüklendiğinde yorumları getir
    loadReviews();
    
    // Yorum formu submit edildiğinde
    $('#reviewForm').on('submit', function(e) {
        e.preventDefault();
        submitReview();
    });
    
    // Yorumları yükle
    function loadReviews() {
        $.ajax({
            url: `/api/Reviews/cafe/${cafeId}`,
            type: 'GET',
            headers: {
                'Authorization': 'Bearer ' + getToken()
            },
            success: function(data) {
                displayReviews(data);
            },
            error: function(xhr, status, error) {
                console.error('Yorumlar yüklenirken hata:', error);
                showAlert('Yorumlar yüklenirken bir hata oluştu.', 'error');
            }
        });
    }
    
    // Yorum gönder
    function submitReview() {
        const rating = $('#rating').val();
        const comment = $('#comment').val();
        
        if (!comment.trim()) {
            showAlert('Lütfen bir yorum yazın.', 'error');
            return;
        }
        
        const reviewData = {
            cafeId: parseInt(cafeId),
            rating: parseInt(rating),
            comment: comment.trim()
        };
        
        $.ajax({
            url: '/api/Reviews',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(reviewData),
            headers: {
                'Authorization': 'Bearer ' + getToken()
            },
            success: function(data) {
                showAlert('Yorumunuz başarıyla eklendi!', 'success');
                $('#comment').val('');
                $('#rating').val('5');
                loadReviews(); // Yorumları yeniden yükle
            },
            error: function(xhr, status, error) {
                let errorMessage = 'Yorum eklenirken bir hata oluştu.';
                
                if (xhr.responseJSON && xhr.responseJSON.message) {
                    errorMessage = xhr.responseJSON.message;
                } else if (xhr.status === 401) {
                    errorMessage = 'Lütfen giriş yapın.';
                }
                
                showAlert(errorMessage, 'error');
            }
        });
    }
    
    // Yorumları görüntüle
    function displayReviews(reviews) {
        const container = $('#reviewsContainer');
        
        if (reviews.length === 0) {
            container.html('<p class="no-reviews">Henüz yorum yapılmamış. İlk yorumu siz yapın!</p>');
            return;
        }
        
        let html = '';
        reviews.forEach(function(review) {
            const stars = getStarRating(review.rating);
            const date = new Date(review.createdDate).toLocaleDateString('tr-TR');
            
            html += `
                <div class="review-item">
                    <div class="review-header">
                        <div class="reviewer-info">
                            <strong>${review.userFullName}</strong>
                            <span class="review-date">${date}</span>
                        </div>
                        <div class="rating-stars">
                            ${stars}
                        </div>
                    </div>
                    <div class="review-content">
                        <p>${escapeHtml(review.comment)}</p>
                    </div>
                </div>
            `;
        });
        
        container.html(html);
    }
    
    // Yıldız rating'i oluştur
    function getStarRating(rating) {
        let stars = '';
        for (let i = 1; i <= 5; i++) {
            if (i <= rating) {
                stars += '<span class="star filled">★</span>';
            } else {
                stars += '<span class="star">☆</span>';
            }
        }
        return stars;
    }
    
    // HTML escape
    function escapeHtml(text) {
        const map = {
            '&': '&amp;',
            '<': '&lt;',
            '>': '&gt;',
            '"': '&quot;',
            "'": '&#039;'
        };
        return text.replace(/[&<>"']/g, function(m) { return map[m]; });
    }
    
    // Token'ı localStorage'dan al
    function getToken() {
        return localStorage.getItem('jwtToken') || sessionStorage.getItem('jwtToken');
    }
    
    // Alert göster
    function showAlert(message, type) {
        const alertClass = type === 'success' ? 'alert-success' : 'alert-danger';
        const alertHtml = `
            <div class="alert ${alertClass} alert-dismissible fade show" role="alert">
                ${message}
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </div>
        `;
        
        // Mevcut alert'leri temizle
        $('.alert').remove();
        
        // Yeni alert'i ekle
        $('.reviews-section').prepend(alertHtml);
        
        // 5 saniye sonra otomatik kapat
        setTimeout(function() {
            $('.alert').fadeOut();
        }, 5000);
    }
}); 